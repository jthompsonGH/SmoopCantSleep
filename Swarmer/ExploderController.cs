using Pathfinding;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;


public class ExploderController : MonoBehaviour
{
    public AIPath path;
    public LayerMask layerMask;
    public GameObject explode;
    public Rigidbody2D rb;
    public CinemachineImpulseSource impulse;
    public CinemachineImpulseSource deadImpulse;
    public UnityEngine.Rendering.Universal.Light2D _Light;
    public SoundObject soundObject;
    public float range;
    public float knockback;
    public float explodeAt;
    public float blastRange;
    public bool playerDetected;
    float explodeTimer;
    bool gonnaExplode;
    bool litUp = false;
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
        party = GetComponent<ParticleSystem>();
        party.Stop();

        if (GameObject.Find("CullingManager"))
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
            cull.AddThis(this.gameObject);
        }
    }

    private void Start()
    {
        /*smoop = GameObject.FindGameObjectWithTag("Player");
        smoopLoc = smoop.transform;
        GetComponent<AIDestinationSetter>().target = smoopLoc;*/
        animator = GetComponent<Animator>();
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
                    }
                }
            }

            if (playerDetected)
            {
                gonnaExplode = true;
                AITargetReset();
            }
        }

        if (gonnaExplode)
        {
            explodeTimer += Time.deltaTime;
            
            if (_Light.GetComponent<LightDim>().enabled != true)
            {
                _Light.GetComponent<LightDim>().enabled = true;
            }

            if (explodeTimer >= explodeAt - 0.2f)
            {
                if (!litUp)
                {
                    litUp = true;
                    _Light.intensity += 4f;
                    soundObject.PlaySound(2);
                }
            }
            
            if (explodeTimer >= explodeAt)
            {
                Explode();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            PlayShield();
        }
        if (collision.CompareTag("Bullet"))
        {
            impulse.GenerateImpulse();

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
            if (leftBall)
            {
                leftBall.Animations();
                PlayShield();
            }
            
        }
    }

    public void AITargetReset()
    {
        smoop = GameObject.FindGameObjectWithTag("Player");
        GetComponent<AIDestinationSetter>().target = smoop.transform;
    }

    public void Explode()
    {
        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }    
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, blastRange, layerMask);

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Player"))
            {
                hit.gameObject.GetComponent<PlayerController>().TakeDamage(3);
            }
            else if (hit.CompareTag("Swarmer"))
            {
                hit.gameObject.GetComponent<SwarmerController>().Die();
            }
            else if (hit.CompareTag("Dorp"))
            {
                hit.gameObject.GetComponent<DorpController>().Die();
            }
            else if (hit.CompareTag("Platform") && hit.GetComponent<PlatformController>())
            {
                hit.GetComponent<PlatformController>().Shatter();
            }
        }

        deadImpulse.GenerateImpulse();
        Instantiate(explode, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    public void PlayShield()
    {
        party.Play();

        soundObject.PlaySound(1);
    }

    public void HarmlessExplode()
    {
        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }

        deadImpulse.GenerateImpulse();
        Instantiate(explode, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
