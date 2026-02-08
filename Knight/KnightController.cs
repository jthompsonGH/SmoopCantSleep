using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public GameObject shield;
    public GameObject gemSpawner;
    public Animator animator;
    public Rigidbody2D rb;
    public LayerMask layerMask;
    public Transform attackPoint;
    public Transform shieldDrop;
    public Transform knockbackPoint;
    public HealthBar healthbar;
    public Canvas canvas;
    public CinemachineImpulseSource impulse;
    public CinemachineImpulseSource deadImpulse;
    public Collider2D shieldCollider;
    public SoundObject soundObject;
    public float speed;
    public float speedIncrease;
    public float range;
    public float attackRate;
    public float knockback;
    public float maxHealth = 5;
    public bool isShielded = true;
    public bool playerDetected;
    public bool canMove;
    float startSpeed;
    float attackTime = 0f;
    float health;
    float damage = 1;
    int shieldDropped;
    bool facingLeft;
    bool canAttack;
    bool attacked;
    bool blocked;
    bool playerDead;
    CullingManager cull;
    GameObject player;
    DamageScript takeDamage;
    Transform playerPos;

    private void Awake()
    {
        GameObject cullingMan = GameObject.Find("CullingManager");

        if (cullingMan)
        {
            cull = cullingMan.GetComponent<CullingManager>();
            cull.AddThis(this.gameObject);
        }

        switch (maxHealth)
        {
            case 3:
                speed = 13000f;
                speedIncrease = 30000f;
                attackRate = 1.4f;
                damage = 0.5f;
                break;
            case 4:
                speed = 10000f;
                speedIncrease = 25000f;
                attackRate = 1.2f;
                damage = 0.7f;
                break;
            case 5:
                speed = 7000f;
                speedIncrease = 20000f;
                attackRate = 1f;
                damage = 1;
                break;
            case 6:
                speed = 6000f;
                speedIncrease = 18000f;
                attackRate = .8f;
                damage = 1.5f;
                break;
            case 7:
                speed = 4000f;
                speedIncrease = 16000f;
                attackRate = .6f;
                damage = 2f;
                break;
            default:
                maxHealth = 5;
                speed = 7000f;
                speedIncrease = 20000f;
                attackRate = 1f;
                damage = 1f;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.totalEnemies++;
        }

        canvas.enabled = false;
        
        if (transform.rotation.y == 0f)
        {
            facingLeft = true;
        }
        else if (transform.rotation.y == 180f)
        {
            facingLeft = false;
        }
        
        canMove = true;
        canAttack = true;
        
        takeDamage = GetComponent<DamageScript>();
        health = maxHealth;
        startSpeed = speed;

        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            if (playerDead)
            {
                playerDead = false;
            }

            animator.SetBool("playerDead", false);
            
            playerPos = player.transform;

            RaycastHit2D rayCast = Physics2D.Raycast(this.gameObject.transform.position, -transform.right, range, layerMask);

            if (rayCast)
            {
                if (rayCast.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                }
            }

            if (Vector2.Distance(playerPos.position, transform.position) > 7f)
            {
                playerDetected = false;
            }
            
            if (playerDetected)
            {
                if ((playerPos.position.x > transform.position.x + 1f) && facingLeft)
                {
                    if (transform.position.y - playerPos.position.y <= 2f)
                    {
                        Flip();
                    }
                }
                else if ((playerPos.position.x < transform.position.x - 1f) && !facingLeft)
                {
                    if (transform.position.y - playerPos.position.y <= 2f)
                    {
                        Flip();
                    }
                }

                speed = speedIncrease;
                animator.SetBool("playerDetected", true);
            }
            if (!playerDetected)
            {
                speed = startSpeed;
                animator.SetBool("playerDetected", false);
            }

            if (Vector2.Distance(playerPos.position, transform.position) < .4f)
            {
                canMove = false;

                if (Time.time >= attackTime && !attacked)
                {
                    if (canAttack)
                    {
                        Invoke("Attack", .1f);
                        attackTime = Time.time + 1f / attackRate;
                    }
                }
            }
            else if (Vector2.Distance(playerPos.position, transform.position) >= .4f)
            {
                if (!attacked && !blocked)
                {
                    canMove = true;
                }
            }
            if (blocked || attacked)
            {
                canMove = false;
            }
        }

        /*if (animator.GetCurrentAnimatorStateInfo(0).IsTag("attacking") || animator.GetCurrentAnimatorStateInfo(0).IsTag("blocking"))
        {
            canMove = false;
        }*/

        if (player == null)
        {
            if (!playerDead)
            {
                playerDead = true;
                animator.SetBool("playerDead", true);
            }
            
            canMove = true;
            playerDetected = false;
            animator.SetBool("playerDetected", false);
            speed = startSpeed;
        }

        if (canMove)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("blocking"))
            {
                if (player != null)
                {
                    if (Vector2.Distance(playerPos.position, transform.position) >= .4f)
                    {
                        if (facingLeft)
                        {
                            rb.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
                        }
                        else if (!facingLeft)
                        {
                            rb.AddForce(new Vector2(speed * Time.deltaTime, 0f));
                        }
                    }
                }
                if (player == null)
                {
                    if (facingLeft)
                    {
                        rb.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
                    }
                    else if (!facingLeft)
                    {
                        rb.AddForce(new Vector2(speed * Time.deltaTime, 0f));
                    }
                }
            }

            animator.SetBool("isMoving", true);
        }

        if (!canMove)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("isMoving", false);
        }

        //animator.SetBool("running", true);
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        rb.velocity = new Vector2(0f, 0f);
        facingLeft = !facingLeft;
    }

    public void ShieldBreak()
    {
        takeDamage.HurtFlash();
        animator.SetTrigger("ShieldBroken");
        isShielded = false;
        canAttack = false;
        shieldCollider.enabled = false;

        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }

        if (!playerDetected)
        {
            playerDetected = true;
        }

        Invoke("AttackReset", .4f);

        if (shieldDropped == 0)
        {
            Instantiate(shield, shieldDrop.position, transform.rotation);
            shieldDropped++;
        }
    }

    void Attack()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, .2f, layerMask);
        animator.SetTrigger("isAttacking");

        if (playerHit)
        {
            if (player != null)
            {
                PlayerController smoop = player.GetComponent<PlayerController>();
                smoop.TakeDamage(damage);
            }
        }

        canAttack = false;
        canMove = false;
        attacked = true;

        Invoke("AttackReset", attackRate);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        takeDamage.HurtFlash();
        healthbar.SetHealth(health);
        StartCoroutine(healthbar.Flash());

        if (!playerDetected)
        {
            playerDetected = true;
        }

        if (health <= 0f)
        {
            Die();
        }
        else if (health != 0f)
        {
            impulse.GenerateImpulse();
            soundObject.PlaySound(0);
        }

    }

    void Die()
    {
        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.UpdateText("knight");
        }

        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }
        
        Instantiate(gemSpawner, transform.position, transform.rotation);

        deadImpulse.GenerateImpulse();
        takeDamage.DeathPop();
        Destroy(gameObject);
    }

    /*void BlockedShot()
    {
        canMove = false;
        canAttack = false;
        blocked = true;

        Invoke("ResetValues", 1f);
    }*/

    void AttackReset()
    {
        canAttack = true;
        attacked = false;

        animator.ResetTrigger("isAttacking");
    }
    
    void BlockReset()
    {
        blocked = false;

        animator.ResetTrigger("shotBlocked");
    }

    public void StompBlocked()
    {
        blocked = true;
        canAttack = false;

        animator.SetTrigger("shotBlocked");

        Invoke("BlockReset", .8f);
        Invoke("AttackReset", .8f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("noShield"))
        {
            if (collision.CompareTag("Bullet"))
            {
                if (DamageUI.poweredUP)
                {
                    ShieldBreak();
                }
                else if (!DamageUI.poweredUP)
                {
                    if ((playerPos.position.x > transform.position.x) && facingLeft)
                    {
                        Flip();
                    }
                    else if ((playerPos.position.x < transform.position.x) && !facingLeft)
                    {
                        Flip();
                    }

                    blocked = true;
                    canAttack = false;

                    animator.SetTrigger("shotBlocked");

                    soundObject.PlaySound(1);

                    Invoke("BlockReset", .4f);
                    Invoke("AttackReset", .6f);
                }

                BloodBallShoot rightBall = collision.GetComponent<BloodBallShoot>();
                BloodBallShootLeft leftBall = collision.GetComponent<BloodBallShootLeft>();

                if (rightBall)
                {
                    rightBall.ShieldUp();
                    Destroy(collision.gameObject);

                }
                else if (leftBall)
                {
                    leftBall.ShieldUp();
                    Destroy(collision.gameObject);
                }
            }
        }
       if (animator.GetCurrentAnimatorStateInfo(0).IsTag("noShield"))
        {
            if (collision.CompareTag("Bullet"))
            {
                BloodBallShoot rightBall = collision.GetComponent<BloodBallShoot>();
                BloodBallShootLeft leftBall = collision.GetComponent<BloodBallShootLeft>();

                if (collision.gameObject.transform.position.x < knockbackPoint.position.x)
                {
                    rb.AddForce(new Vector2(knockback, 0f));
                }
                else if (collision.gameObject.transform.position.x > knockbackPoint.position.x)
                {
                    rb.AddForce(new Vector2(-knockback, 0f));
                }

                if (rightBall)
                {
                    this.damage = rightBall.damage;
                    rightBall.Animations();
                    Destroy(collision.gameObject);

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
                    Destroy(collision.gameObject);

                    if (!leftBall.didDamage)
                    {
                        TakeDamage(damage);
                        leftBall.didDamage = true;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!playerDetected)
            {
                playerDetected = true;

                if (player)
                {
                    if (player.transform.position.x < transform.position.x)
                    {
                        Flip();
                    }
                    else if (player.transform.position.x > transform.position.x)
                    {
                        Flip();
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
