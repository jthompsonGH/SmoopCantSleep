using Cinemachine;
using UnityEngine;

public class DorpController : MonoBehaviour
{
    public GameObject gemSpawner;
    public HealthBar healthbar;
    public Animator animator;
    public Canvas canvas;
    public CinemachineImpulseSource impulse;
    public CinemachineImpulseSource deadImpulse;
    public SoundObject soundObject;
    public int health;
    public int maxHealth;
    public bool dropGems = true;
    int damage;
    bool facingLeft = true;
    Rigidbody2D rb;
    CullingManager cull;
    GameObject player;
    DamageScript takeDamage;

    private void Awake()
    {
        GameObject cullingMan = GameObject.Find("CullingManager");

        if (cullingMan)
        {
            cull = cullingMan.GetComponent<CullingManager>();
            cull.AddThis(this.gameObject);
        }

        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    private void Start()
    {
        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.totalEnemies++;
        }

        takeDamage = GetComponent<DamageScript>();
        canvas.enabled = false;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (player.transform.position.x > transform.position.x + 1f)
            {
                if (facingLeft)
                {
                    transform.Rotate(0f, 180f, 0f);
                    facingLeft = false;
                }
            }
            if (player.transform.position.x < transform.position.x - 1f)
            {
                if (!facingLeft)
                {
                    transform.Rotate(0f, 180f, 0f);
                    facingLeft = true;
                }
            }
        }

        if (!canvas.enabled)
        {
            if (health != maxHealth)
            {
                canvas.enabled = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        takeDamage.HurtFlash();
        healthbar.SetHealth(health);
        StartCoroutine(healthbar.Flash());

        if (health <= 0f)
        {
            Die();
        }
        else if (health != 0f)
        {
            soundObject.PlaySound(0);
            impulse.GenerateImpulse();
        }

    }

    public void Die()
    {
        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.UpdateText("dorp");
        }

        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }
        
        deadImpulse.GenerateImpulse();
        takeDamage.DeathPop();
        DorpKill();
    }

    void DorpKill()
    {
        if (dropGems)
        {
            Instantiate(gemSpawner, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Bullet"))
        {
            BloodBallShoot rightBall = hitInfo.GetComponent<BloodBallShoot>();
            BloodBallShootLeft leftBall = hitInfo.GetComponent<BloodBallShootLeft>();

            if (rightBall)
            {
                this.damage = rightBall.damage;
                rightBall.Animations();
                Destroy(hitInfo.gameObject);

                if (!rightBall.didDamage)
                {
                    TakeDamage(damage);
                    rightBall.didDamage = true;
                }

            }
            if (leftBall)
            {
                this.damage = leftBall.damage;
                leftBall.Animations();
                Destroy(hitInfo.gameObject);

                if (!leftBall.didDamage)
                {
                    TakeDamage(damage);
                    leftBall.didDamage = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Head"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    /*private void DisableDorp()
    {
        this.enabled = false;
    }*/
}
