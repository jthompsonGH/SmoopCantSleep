using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterController : MonoBehaviour
{
    public Transform beamPoint;
    public Transform rayCastPoint;
    public Transform blastPoint;
    public Transform chargePoint;
    public LayerMask playerMask;
    public GameObject casterBeam;
    public GameObject staffBlast;
    public GameObject staffCharge;
    public GameObject deathPop;
    public ParticleSystem thisParty;
    public Animator animator;
    public CinemachineImpulseSource impulse;
    public CinemachineImpulseSource deathImpulse;
    public SoundObject soundObject;
    public float detectionRange;
    public float range;
    CullingManager cull;
    GameObject player;
    Vector2 castPoint;
    float rechargeTime;
    float resetTime;
    int rayCount;
    int staffPop;

    private void Awake()
    {
        castPoint = new Vector2(rayCastPoint.position.x, rayCastPoint.position.y);

        GameObject cullingMan = GameObject.Find("CullingManager");

        if (cullingMan)
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
        }

        if (cull)
        {
            cull.AddThis(this.gameObject);
        }
    }

    private void Start()
    {
        rechargeTime = 2f;
        resetTime = rechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        rechargeTime -= Time.deltaTime;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if ((Vector2.Distance(player.transform.position, transform.position) < detectionRange))
            {
                if (rechargeTime <= 0f)
                {
                    animator.SetTrigger("isCharging");

                    Instantiate(staffCharge, chargePoint.position, chargePoint.rotation);

                    rechargeTime = resetTime;

                    rayCount = 0;

                    staffPop = 0;
                }
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
            {
                if (staffPop == 0)
                {
                    Instantiate(staffBlast, blastPoint.position, blastPoint.rotation);
                    Instantiate(casterBeam, beamPoint.position, beamPoint.rotation);
                    impulse.GenerateImpulse();
                    staffPop++;
                }

                if (rayCount <= 1)
                {
                    RaycastHit2D rayCast = Physics2D.Raycast(castPoint, Vector2.left, range, playerMask);

                    if (rayCast)
                    {
                        if (rayCast.collider.CompareTag("Player"))
                        {
                            if (!player.GetComponent<PlayerController>().dashProtect)
                            {
                                player.GetComponent<PlayerController>().TakeDamage(1);
                                rayCount++;
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Ball"))
        {
            PlayShield();

            BloodBallShoot rightBall = collision.gameObject.GetComponent<BloodBallShoot>();
            BloodBallShootLeft leftBall = collision.gameObject.GetComponent<BloodBallShootLeft>();

            if (rightBall)
            {
                rightBall.Animations();
            }
            else if (leftBall)
            {
                leftBall.Animations();
            }
        }
    }

    public void Die()
    {
        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }
        
        Instantiate(deathPop, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void PlayShield()
    {
        thisParty.Play();

        soundObject.PlaySound(0);
    }
}
