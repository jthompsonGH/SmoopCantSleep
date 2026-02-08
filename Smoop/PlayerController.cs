using Cinemachine;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public Canvas canvas;
    public LayerMask enemyLayer;
    public HealthBar lifebar;
    public HealthBar healthbar;
    public HealthBar meleeBar;
    public GameObject smoopDeath;
    public GameObject smoopHeal;
    public GameObject smoopLifeUp;
    public GameObject meleeBlast;
    public GameObject smallBlast;
    public Transform healSpawn;
    public Transform lifeSpawn;
    public Transform blastPoint;
    public Transform headPoint;
    public CinemachineImpulseSource deadImpulse;
    public CinemachineImpulseSource damageImpulse;
    public Slider meleeSlider;
    public SoundObject soundObject;
    public bool dashProtect;
    public float meleeRate;
    public float meleeRange;
    public float dashLeaveRange;
    public float maxHealth;
    public float health;
    public float meleeReady;
    public float damageRate = .33f;
    DamageScript takeDamage;
    GameObject respawner;
    GameObject[] Swarmers;
    RespawnTracker livesLeft;
    int damage;
    int swarmerHit;
    float nextMeleeTime;
    float nextDamage = 0f;
    bool knightHit;
    bool dead;
    PlayerInput controls;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
        {
            transform.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        }
        
        int amountOfHealth = PlayerPrefs.GetInt("difficultySetting", 3);

        switch (amountOfHealth)
        {
            case 1:
                maxHealth = 9;
                break;
            case 2:
                maxHealth = 7;
                break;
            case 3:
                maxHealth = 5;
                break;
            case 4:
                maxHealth = 3;
                break;
            case 5:
                maxHealth = 1;
                break;
        }
        
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "LevelSelect")
        {
            GameObject.FindGameObjectWithTag("PlayerCanvas").SetActive(false);

            DontDestroyOnLoad(this.gameObject);
        }
        
        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            Cursor.visible = false;
        }

        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();

            if (!tracker.playerMoved)
            {
                tracker.PlayerStartPos(this.gameObject);
            }
        }
    }

    private void Start()
    {
        nextMeleeTime = 0f;
        meleeReady = 1f;

        dead = false;

        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            Cursor.visible = false;

            Swarmers = GameObject.FindGameObjectsWithTag("Swarmer");

            CinemachineVirtualCamera vCam = GameObject.Find("vCam").GetComponent<CinemachineVirtualCamera>();
            vCam.m_Follow = this.gameObject.transform;

            respawner = GameObject.FindGameObjectWithTag("Respawn");
            livesLeft = respawner.GetComponent<RespawnTracker>();

            takeDamage = GetComponent<DamageScript>();

            health = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            if (respawner != null)
            {
                if (livesLeft.numberOfLives == 3)
                {
                    lifebar.SetMaxHealth(3);
                    lifebar.SetHealth(livesLeft.numberOfLives);
                }
                else if (livesLeft.numberOfLives != 3)
                {
                    lifebar.SetHealth(livesLeft.numberOfLives + 1, true);
                    StartCoroutine(lifebar.Flash());
                    lifebar.SetHealth(livesLeft.numberOfLives);
                }
            }
            else if (respawner == null)
            {
                lifebar.SetHealth(3);
            }

            meleeBar.SetMaxHealth(1);
        }

        if (!GameManager.meleeUnlocked)
        {
            meleeReady = 0f;
            meleeSlider.value = 0f;
        }
    }

    private void Update()
    {
        if (GameManager.meleeUnlocked)
        {
            meleeSlider.value = meleeReady;

            if (meleeReady < 1f)
            {
                meleeReady += Time.deltaTime * meleeRate;
            }

            if (!dashProtect)
            {
                if (controls.actions["Melee"].triggered)
                {
                    if (Time.time >= nextMeleeTime)
                    {
                        /*Instantiate(meleeBlast, blastPoint.position, blastPoint.rotation);

                        nextMeleeTime = Time.time + 1f / meleeRate;
                        meleeReady = 0f;
                        swarmerHit = 0;
                        knightHit = false;*/

                        DoMelee();
                    }
                }
            }
        }
    }

    void DoMelee()
    {
        if (GameManager.meleeUnlocked)
        {
            if (!dashProtect)
            {
                if (Time.time >= nextMeleeTime)
                {
                    Instantiate(meleeBlast, blastPoint.position, blastPoint.rotation);

                    nextMeleeTime = Time.time + 1f / meleeRate;
                    meleeReady = 0f;
                    swarmerHit = 0;
                    knightHit = false;
                    MeleeAttack();
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (Time.time >= nextDamage)
        {
            if (!GameManager.playerHit)
            {
                GameManager.playerHit = true;
            }

            nextDamage = Time.time + damageRate / 1f;
            
            health -= damage;

            healthbar.SetHealth(health);
            StartCoroutine(healthbar.Flash());

            takeDamage.HurtFlash();

            damageImpulse.GenerateImpulse();

            if (health <= 0)
            {
                Die();
            }
            else
            {
                soundObject.PlaySound(0);
            }
        }
    }

    public void Heals(int bottleHeal)
    {
        health += bottleHeal;
        Instantiate(smoopHeal, healSpawn.position, Quaternion.identity);

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        healthbar.SetHealth(health);
        StartCoroutine(healthbar.Flash());
    }

    void Die()
    {
        if (!GameManager.playerDied)
        {
            GameManager.playerDied = true;
        }
        
        Swarmers = GameObject.FindGameObjectsWithTag("Swarmer");
        foreach (GameObject swarmy in Swarmers)
        {
            if (swarmy.GetComponent<SwarmerController>())
            {
                swarmy.GetComponent<SwarmerController>().ResetPathing();
            }
        }

        if (!dead)
        {
            takeDamage.DeathPop();

            Instantiate(smoopDeath, headPoint.position, Quaternion.identity);
            deadImpulse.GenerateImpulse();

            dead = true;
            Destroy(this.gameObject);
        }
    }

    void MeleeAttack()
    {
        bool dorpHit = false;
        
        damageImpulse.GenerateImpulse();
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(blastPoint.position, meleeRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            switch (enemy.tag)
            {
                case "dontReact":
                    VaseController vase = enemy.GetComponent<VaseController>();
                    if (vase)
                    {
                        vase.Shatter();
                    }
                    break;
                case "Dorp":
                    if (!dorpHit)
                    {
                        enemy.GetComponent<DorpController>().TakeDamage(3);
                        dorpHit = true;
                    }
                    break;
                case "Swarmer":
                    if (swarmerHit <= 3)
                    {
                        if (enemy.GetComponent<SwarmerController>())
                        {
                            enemy.GetComponent<SwarmerController>().Die();
                            swarmerHit++;
                        }
                        else if (enemy.GetComponent<ExploderController>())
                        {
                            enemy.GetComponent<ExploderController>().PlayShield();

                            if (enemy.transform.position.x < transform.position.x)
                            {
                                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2((-enemy.GetComponent<ExploderController>().knockback * 3f), 0f));
                            }
                            else if (enemy.transform.position.x > transform.position.x)
                            {
                                enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2((enemy.GetComponent<ExploderController>().knockback * 3f), 0f));
                            }
                        }
                    }
                    break;
                case "Caster":
                    if (enemy.GetComponent<CasterController>())
                    {
                        enemy.GetComponent<CasterController>().Die();
                    }
                    if (enemy.GetComponent<CasterLeftController>())
                    {
                        enemy.GetComponent<CasterLeftController>().Die();
                    }
                    break;
                case "Knight":
                    if (!knightHit)
                    {
                        KnightController knight = enemy.GetComponent<KnightController>();
                        bool shield = enemy.GetComponent<KnightController>().isShielded;
                        knightHit = true;

                        if (transform.position.x < knight.knockbackPoint.position.x)
                        {
                            knight.rb.AddForce(new Vector2((knight.knockback * 3), 0f));
                        }
                        else if (transform.position.x > knight.knockbackPoint.position.x)
                        {
                            knight.rb.AddForce(new Vector2(-(knight.knockback * 3), 0f));
                        }

                        if (shield)
                        {
                            knight.ShieldBreak();
                        }
                        else if (!shield)
                        {
                            knight.TakeDamage(2);
                        }
                    }
                    break;
                case "Ball":
                    if (enemy.GetComponent<DorpBallShoot>())
                    {
                        enemy.GetComponent<DorpBallShoot>().Animations();
                    }
                    break;
            }
            
            /*if (enemy.CompareTag("dontReact"))
            {
                VaseController vase = enemy.GetComponent<VaseController>();

                if (vase)
                {
                    vase.Shatter();
                }

            }
            else if (enemy.CompareTag("Dorp"))
            {
                if (!dorpHit)
                {
                    enemy.GetComponent<DorpController>().TakeDamage(3);
                    dorpHit = true;
                }

            }
            else if (enemy.CompareTag("Swarmer"))
            {
                if (swarmerHit <= 3)
                {
                    if (enemy.GetComponent<SwarmerController>())
                    {
                        enemy.GetComponent<SwarmerController>().Die();
                        swarmerHit++;
                    }
                    else if (enemy.GetComponent<ExploderController>())
                    {
                        enemy.GetComponent<ExploderController>().PlayShield();

                        if (enemy.transform.position.x < transform.position.x)
                        {
                            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2((-enemy.GetComponent<ExploderController>().knockback * 3f), 0f));
                        }
                        else if (enemy.transform.position.x > transform.position.x)
                        {
                            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2((enemy.GetComponent<ExploderController>().knockback * 3f), 0f));
                        }
                    }
                }
            }
            else if (enemy.CompareTag("Caster"))
            {
                if (enemy.GetComponent<CasterController>())
                {
                    enemy.GetComponent<CasterController>().Die();
                }
                if (enemy.GetComponent<CasterLeftController>())
                {
                    enemy.GetComponent<CasterLeftController>().Die();
                }
            }
            else if (enemy.CompareTag("Knight"))
            {
                if (!knightHit)
                {
                    KnightController knight = enemy.GetComponent<KnightController>();
                    bool shield = enemy.GetComponent<KnightController>().isShielded;
                    knightHit = true;

                    if (transform.position.x < knight.knockbackPoint.position.x)
                    {
                        knight.rb.AddForce(new Vector2((knight.knockback * 3), 0f));
                    }
                    else if (transform.position.x > knight.knockbackPoint.position.x)
                    {
                        knight.rb.AddForce(new Vector2(-(knight.knockback * 3), 0f));
                    }

                    if (shield)
                    {
                        knight.ShieldBreak();
                    }
                    else if (!shield)
                    {
                        knight.TakeDamage(2);
                    }
                }
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!dashProtect)
        {
            if (hitInfo.CompareTag("Ball"))
            {
                DorpBallShoot thisBall = hitInfo.GetComponent<DorpBallShoot>();

                Destroy(hitInfo.gameObject);

                this.damage = thisBall.damage;

                TakeDamage(damage);
                //thisBall.Animations();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(blastPoint.position, meleeRange);
    }

    public void AddLife()
    {
        respawner = GameObject.FindGameObjectWithTag("Respawn");

        if (respawner != null)
        {
            RespawnTracker livesLeft = respawner.GetComponent<RespawnTracker>();

            if (livesLeft.numberOfLives <= 2)
            {
                livesLeft.numberOfLives += 1;
            }

            Instantiate(smoopLifeUp, lifeSpawn.position, Quaternion.identity);

            StartCoroutine(lifebar.Flash());
            lifebar.SetHealth(livesLeft.numberOfLives);
        }
    }

    public void LifeBarFix()
    {
        lifebar.MoveLifeBar();
    }

    public void LeavingDash()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(blastPoint.position, dashLeaveRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            switch (enemy.tag)
            {
                case "dontReact":
                    VaseController vase = enemy.GetComponent<VaseController>();
                    if (vase)
                    {
                        vase.Shatter();
                    }
                    break;
                case "Dorp":
                    enemy.GetComponent<DorpController>().Die();
                    Instantiate(meleeBlast, blastPoint.position, Quaternion.identity);
                    break;
                case "Swarmer":
                    if (swarmerHit <= 3)
                    {
                        if (enemy.GetComponent<SwarmerController>())
                        {
                            enemy.GetComponent<SwarmerController>().Die();
                        }
                        else if (enemy.GetComponent<ExploderController>())
                        {
                            enemy.GetComponent<ExploderController>().HarmlessExplode();
                        }
                        Instantiate(smallBlast, blastPoint.position, Quaternion.identity);
                    }
                    break;
                case "Caster":
                    if (enemy.GetComponent<CasterController>())
                    {
                        enemy.GetComponent<CasterController>().Die();
                    }
                    if (enemy.GetComponent<CasterLeftController>())
                    {
                        enemy.GetComponent<CasterLeftController>().Die();
                    }
                    Instantiate(meleeBlast, blastPoint.position, Quaternion.identity);
                    break;
                case "Knight":
                    if (!knightHit)
                    {
                        KnightController knight = enemy.GetComponent<KnightController>();
                        bool shield = enemy.GetComponent<KnightController>().isShielded;
                        knightHit = true;

                        if (transform.position.x < knight.knockbackPoint.position.x)
                        {
                            knight.rb.AddForce(new Vector2((knight.knockback * 3), 0f));
                        }
                        else if (transform.position.x > knight.knockbackPoint.position.x)
                        {
                            knight.rb.AddForce(new Vector2(-(knight.knockback * 3), 0f));
                        }

                        if (shield)
                        {
                            knight.ShieldBreak();
                        }
                        else if (!shield)
                        {
                            knight.TakeDamage(2);
                        }
                        Instantiate(meleeBlast, blastPoint.position, Quaternion.identity);
                    }
                    break;
                case "Ball":
                    if (enemy.GetComponent<DorpBallShoot>())
                    {
                        enemy.GetComponent<DorpBallShoot>().Animations();
                    }
                    break;
            }
        }
    }
}


