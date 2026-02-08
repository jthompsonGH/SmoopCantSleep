using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashMove : MonoBehaviour
{
    public GameObject groundPound;
    public GameObject smallPound;
    public GameObject dashUI;
    public Material dashFlash;
    public Transform poundPoint;
    public PlayerController playerC;
    public PlayerMovement playerM;
    public CinemachineImpulseSource impulse;
    public SoundObject soundObject;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    ParticleSystem trail;
    ParticleSystem cape;
    Material matDefault;
    public float dashSpeed;
    public float dashRate;
    public float dashReady;
    float timesinceDash;
    float dashTime = 0f;
    float fallTime = 0f;
    float horizontalInput;
    float gravity;
    int groundPounded;
    bool fastfallCollision;
    bool reallyGrounded;
    bool dashed = false;
    PlayerInput controls;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.dashUnlocked)
        {
            this.enabled = false;
        }

        groundPounded = 0;
        reallyGrounded = true;

        dashReady = .3f;

        if (GameObject.Find("StillCape"))
        {
            cape = GameObject.Find("StillCape").GetComponent<ParticleSystem>();
        }

        trail = GameObject.Find("DashPoint").GetComponent<ParticleSystem>();
        trail.Stop();
        
        sprite = GetComponent<SpriteRenderer>();
        matDefault = sprite.material;

        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = controls.actions["Move"].ReadValue<float>();

        if (dashReady < .3f)
        {
            dashReady += Time.deltaTime;
        }

        //playerC = GetComponent<PlayerController>();
        //playerM = GetComponent<PlayerMovement>();

        timesinceDash += Time.deltaTime;

        if (dashed)
        {
            dashReady = 0f;

            if (timesinceDash >= .3f && !fastfallCollision)
            {
                GravityReset();
                DashDisabler();
                LeaveDash();
                dashed = false;
            }
            else if (fastfallCollision && reallyGrounded)
            {
                if (reallyGrounded || timesinceDash >= 3f)
                {
                    GravityReset();
                    DashDisabler();
                    LeaveDash();
                    dashed = false;
                }
            }
        }

        /*if (horizontalInput == 0f)
        {
            horizontal = 1f;
        }
        else
        {
            horizontal = horizontalInput;
        }*/

        if (!fastfallCollision)
        {
            if (Time.time >= dashTime)
            {
                if (controls.actions["Dash"].triggered)
                {
                    DoDash();
                }
            }
        }
        if (Time.time >= fallTime)
        {
            if (!playerM.IsGrounded())
            {
                if (controls.actions["Ground Pound"].triggered)
                {
                    DoPound();
                }
            }
        }

        if (fastfallCollision)
        {
            bool grounded = playerM.IsGrounded();

            if (grounded && groundPounded == 0)
            {
                BigBlast();
                groundPounded++;
            }
        }

        /*if (dashed)
        {
            playerM.isDashing = true;
            Invoke("DashDisabler", .2f);
        }

        if (dashProtect)
        {
            playerC.dashProtect = true;
        }*/
    }

    void DoDash()
    {
        if (!fastfallCollision)
        {
            if (Time.time >= dashTime)
            {
                soundObject.PlaySound(1);

                IgnoreCollisions();
                sprite.material = dashFlash;
                playerC.dashProtect = true;
                playerM.isDashing = true;

                dashed = true;
                rb.velocity = transform.right * dashSpeed;
                dashTime = Time.time + 1f / dashRate;
                rb.gravityScale = 0f;

                trail.Play();
                if (cape)
                {
                    cape.Stop();
                }
                timesinceDash = 0f;
            }
        }
    }

    void DoPound()
    {
        if (Time.time >= fallTime)
        {
            if (!playerM.IsGrounded())
            {
                if (playerC.dashProtect)
                {
                    DashDisabler();
                    LeaveDash();
                }

                soundObject.PlaySound(1);

                fastfallCollision = true;
                sprite.material = dashFlash;
                playerC.dashProtect = true;
                playerM.isDashing = true;

                dashed = true;
                reallyGrounded = false;
                rb.velocity = new Vector2(0f, 0f);
                rb.gravityScale = 30f;

                fallTime = Time.time + 1f / dashRate;
                trail.Play();
                if (cape)
                {
                    cape.Stop();
                }
                timesinceDash = 0f;
            }
        }
    }

    public void DashDisabler()
    {
        if (cape)
        {
            cape.Play();
        }

        if (trail.isPlaying)
        {
            trail.Stop();
        }

        playerM.isDashing = false;
        playerC.dashProtect = false;
        FixCollisions();
        fastfallCollision = false;
        sprite.material = matDefault;
        groundPounded = 0;
    }

    void LeaveDash()
    {
        if (playerC)
        {
            playerC.LeavingDash();
        }
    }

    public void GravityReset()
    {
        rb.gravityScale = gravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (fastfallCollision)
       {
            impulse.GenerateImpulse();

            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("dontReact") || collision.gameObject.CompareTag("Knight"))
            {
                if (!reallyGrounded)
                {
                    if (collision.gameObject.CompareTag("Platform") && collision.gameObject.name == "TilemapForeGround2")
                    {
                        reallyGrounded = false;
                    }
                    else
                    {
                        reallyGrounded = true;
                        BigBlast();
                    }
                }
            }
            else
            {
                reallyGrounded = false;
            }
            
            DorpController dorp = collision.gameObject.GetComponent<DorpController>();
            SwarmerController swarmer = collision.gameObject.GetComponent<SwarmerController>();
            KnightController knight = collision.gameObject.GetComponent<KnightController>();
            CasterController caster = collision.gameObject.GetComponent<CasterController>();
            CasterLeftController casterLeft = collision.gameObject.GetComponent<CasterLeftController>();
            //HeaderController header = collision.gameObject.GetComponent<HeaderController>();

            if (dorp)
            {
                if (transform.position.y > collision.gameObject.transform.position.y)
                {
                    dorp.Die();
                    BigBlast();
                }
            }
            else if (swarmer)
            {
                if (transform.position.y > collision.gameObject.transform.position.y)
                {
                    swarmer.Die();
                    SmallBlast();
                    groundPounded = 0;
                }
            }
            else if (knight)
            {
                bool shielded = knight.isShielded;
                if (shielded)
                {
                    knight.ShieldBreak();
                }
                if (!shielded)
                {
                    knight.StompBlocked();
                }
            }
            else if (caster)
            {
                caster.Die();
            }
            else if (casterLeft)
            {
                casterLeft.Die();
            }
            /*if (header)
            {
                header.Die();
            }*/
       }
        
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {

        if (fastfallCollision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("dontReact") || collision.gameObject.CompareTag("Knight"))
            {
                if (!reallyGrounded)
                {
                    reallyGrounded = true;
                    impulse.GenerateImpulse();
                    BigBlast();
                }
            }
            else
            {
                reallyGrounded = false;
            }
        }
    }*/

    void SmallBlast()
    {
        Instantiate(smallPound, poundPoint.position, Quaternion.identity);
    }

    void BigBlast()
    {
        Instantiate(groundPound, poundPoint.position, Quaternion.identity);
    }

    public void SetUIActive()
    {
        if (!dashUI.activeSelf)
        {
            dashUI.SetActive(true);
        }
    }

    void IgnoreCollisions()
    {
        Physics2D.IgnoreLayerCollision(0, 9, true);
        Physics2D.IgnoreLayerCollision(0, 16, true);
        Physics2D.IgnoreLayerCollision(0, 17, true);
    }

    void FixCollisions()
    {
        Physics2D.IgnoreLayerCollision(0, 9, false);
        Physics2D.IgnoreLayerCollision(0, 16, false);
        Physics2D.IgnoreLayerCollision(0, 17, false);
    }
}
