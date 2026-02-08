using Cinemachine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerController : MonoBehaviour
{
    public AIPath path;
    public LayerMask layerMask;
    public GameObject deathPop;
    public GameObject gemDrop;
    public Rigidbody2D rb;
    public CinemachineImpulseSource impulse;
    public CinemachineImpulseSource deadImpulse;
    public SoundObject soundObject;
    public float shieldPower = 0f;
    public float range;
    public float attackRate;
    public float knockback;
    public bool playerDetected;
    float attackRange = .4f;
    float nextAttackTime;
    Animator animator;
    CullingManager cull;
    GameObject smoop;
    Transform playerLoc;
    Vector2 target;
    Vector2 Direction;
    ParticleSystem party;

    private void Awake()
    {
        playerDetected = false;

        if (shieldPower == 0f)
        {
            shieldPower = Random.Range(1, 4);
        } 
        party = GetComponent<ParticleSystem>();
        party.Stop();

        GameObject cullingMan = GameObject.Find("CullingManager");

        if (cullingMan)
        {
            cull = cullingMan.GetComponent<CullingManager>();

            cull.AddThis(this.gameObject);
        }
    }

    private void Start()
    {
        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.totalEnemies++;
        }

        animator = GetComponent<Animator>();
        nextAttackTime = 0f;

        smoop = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (path.desiredVelocity.x >= .01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (path.desiredVelocity.x <= -.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        smoop = GameObject.FindGameObjectWithTag("Player");

        if (smoop != null)
        {
            playerLoc = smoop.transform;

            if (Vector2.Distance(playerLoc.position, transform.position) < attackRange && !smoop.GetComponent<PlayerController>().dashProtect)
            {
                if (Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("Attack");
                    smoop.GetComponent<PlayerController>().TakeDamage(1);
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }

            target = new Vector2(playerLoc.position.x, playerLoc.position.y);
            Direction = (target - (Vector2)transform.position);

            if (!playerDetected)
            {
                RaycastHit2D rayCast = Physics2D.Raycast(this.gameObject.transform.position, Direction, range, layerMask);

                if (rayCast)
                {
                    if (rayCast.collider.CompareTag("Player"))
                    {
                        playerDetected = true;

                        soundObject.PlaySound(0);

                        GetComponent<SwarmerPathing>().useIntro = false;
                        AITargetReset();
                    }
                }
            }
        }

        if (playerDetected && GetComponent<AIDestinationSetter>().target != smoop.transform)
        {
            AITargetReset();
        }
    }

    public void Die()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (GameObject.Find("Tracker"))
        {
            Tracker tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
            tracker.UpdateText("swarmer");
        }
        
        if (player)
        {
            Instantiate(gemDrop, transform.position, Quaternion.identity);
        }

        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }
        
        Instantiate(deathPop, transform.position, transform.rotation);
        deadImpulse.GenerateImpulse();
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            party.Play();
        }
        if (collision.CompareTag("Bullet"))
        {
            impulse.GenerateImpulse();
            
            if (shieldPower == 1)
            {
                BloodBallShoot rightBall = collision.GetComponent<BloodBallShoot>();
                BloodBallShootLeft leftBall = collision.GetComponent<BloodBallShootLeft>();

                if (collision.gameObject.transform.position.x < transform.position.x)
                {
                    rb.AddForce(new Vector2(knockback, 0f));
                }
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    rb.AddForce(new Vector2(-knockback, 0f));
                }

                if (rightBall)
                {
                    rightBall.Animations();
                    PlayShield();
                }
                else if (leftBall)
                {
                    leftBall.Animations();
                    PlayShield();
                }
            }
            if (shieldPower > 1 || DamageUI.poweredUP)
            {
                BloodBallShoot rightBall = collision.GetComponent<BloodBallShoot>();
                BloodBallShootLeft leftBall = collision.GetComponent<BloodBallShootLeft>();

                if (leftBall)
                {
                    leftBall.Animations();
                }
                else if (rightBall)
                {
                    rightBall.Animations();
                }
                
                Die();
            }
        }
    }

    public void AITargetReset()
    {
        SwarmerPathing pathing = GetComponent<SwarmerPathing>();

        smoop = GameObject.FindGameObjectWithTag("Player");
        GetComponent<AIDestinationSetter>().target = smoop.transform;

        pathing.currentTarget = pathing.targetPos[pathing.whichPos];
    }

    public void PlayShield()
    {
        party.Play();

        soundObject.PlaySound(1);
    }

    public void ResetPathing()
    {
        playerDetected = false;

        if (GetComponent<SwarmerPathing>().enabled)
        {
            GetComponent<SwarmerPathing>().SetSavedTarget();
        }
    }
}
