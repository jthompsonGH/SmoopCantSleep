using Cinemachine;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;


public class HeadController : MonoBehaviour
{
    public GameObject respawnBlast;
    public GameObject playerRespawn;
    public GameObject sprite;
    public Rigidbody2D head;
    public CinemachineImpulseSource respawnImpulse;
    public CinemachineImpulseSource deadImpulse;
    public float jumpForce;
    public float headSpeed;
    public float respawnTimer;
    GameManager manager;
    GameObject thePlayer;
    GameObject respawner;
    GameObject[] swarmers;
    Rigidbody2D player;
    DamageScript takeDamage;
    Transform headPosition;
    Vector2 newPosition;
    float horizontal;
    float jumpRate = 2;
    float nextJump;
    int numRespawns;
    int damage;
    int health = 1;
    int respawnLimit = 1;
    bool facingRight = true;
    PlayerInput controls;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        CinemachineVirtualCamera vCam = GameObject.Find("vCam").GetComponent<CinemachineVirtualCamera>();
        vCam.m_Follow = GameObject.Find("TrackHead").transform;

        respawner = GameObject.FindGameObjectWithTag("Respawn");
        RespawnTracker plsRespawn = respawner.GetComponent<RespawnTracker>();
        numRespawns = plsRespawn.numberOfLives;

        takeDamage = GetComponent<DamageScript>();
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        player = thePlayer.GetComponent<Rigidbody2D>();

        CopyVelocity(player, head);
        respawnTimer = 0f;

        if (numRespawns <= respawnLimit)
        {
            Die();
        }

        if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
        {
            transform.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        }
    }

    private void Update()
    {
        respawnTimer += Time.deltaTime;
        
        headPosition = this.gameObject.transform;
        newPosition = new Vector2(headPosition.position.x, headPosition.position.y + 0.3f);

        horizontal = controls.actions["Move"].ReadValue<float>();

        SurvivalRespawn();
        
        if (controls.actions["Jump"].triggered)
        {
            DoJump();
        }
    }

    void DoJump()
    {
        if (Time.time >= nextJump)
        {
            head.AddForce(new Vector2(0f, jumpForce));
            nextJump = Time.time + 1f / jumpRate;
        }
    }

    private void FixedUpdate()
    {
        if (horizontal < 0f)
        {
            head.AddForce(new Vector2(-headSpeed * Time.deltaTime, 0f));

            if (facingRight)
            {
                sprite.transform.Rotate(0f, 180f, 0f);
                facingRight = !facingRight;
            }
        }
        if (horizontal > 0f)
        {
            head.AddForce(new Vector2(headSpeed * Time.deltaTime, 0f));

            if (!facingRight)
            {
                sprite.transform.Rotate(0f, 180f, 0f);
                facingRight = !facingRight;
            }
        }
    }
    void CopyVelocity(Rigidbody2D from, Rigidbody2D to)
    {
        Vector3 vFrom = from.velocity;
        Vector3 vTo = to.velocity;

        vTo.x = vFrom.x;
        vTo.y = vFrom.y;
        vTo.z = vFrom.z;

        to.velocity = vTo;
    }
    
    void SurvivalRespawn()
    {
        if (numRespawns > respawnLimit)
        {
            if (respawnTimer >= 3f)
            {
                respawnImpulse.GenerateImpulse();

                RespawnTracker plsRespawn = respawner.GetComponent<RespawnTracker>();
                plsRespawn.DecreaseCount();

                swarmers = GameObject.FindGameObjectsWithTag("Swarmer");

                foreach (GameObject swarmer in swarmers)
                {
                    swarmer.GetComponent<SwarmerController>();

                    if (swarmer.GetComponent<SwarmerController>() != null)
                    {
                        if (Vector2.Distance(transform.position, swarmer.transform.position) < 2f)
                        {
                            swarmer.GetComponent<SwarmerController>().Die();
                        }
                    }
                }
                
                Instantiate(respawnBlast, transform.position, transform.rotation);
                Instantiate(playerRespawn, newPosition, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= 1;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        manager.GameEnded();
        takeDamage.DeathPop();
        deadImpulse.GenerateImpulse();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ball"))
        {
            DorpBallShoot thisBall = hitInfo.GetComponent<DorpBallShoot>();
            thisBall.Animations();
            this.damage = thisBall.damage;
            Destroy(hitInfo.gameObject);
            TakeDamage(damage);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreLayerCollision(0, 9);
    }*/

}
