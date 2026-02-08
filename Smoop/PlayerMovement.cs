using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask platformLayerMask;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2D;
    public GameObject bloodJump;
    public GameObject lifeBar;
    public Transform jumpPoint;
    public bool facingRight = true;
    public float speed;
    public float startRunSpeed;
    public float accelerationFactor;
    public float maxSpeed;
    public float shootSpeed;
    public float jumpVelocity;
    public bool isDashing;
    public int multiJump;
    private Animator animator;
    private float shootTime = 0f;
    private float horizontal;
    private float horizontalShoot;
    private float coyoteTime;
    private float jumpRate;
    private float startSpeed;
    private float startJump;
    private float startGrav;
    private bool canCoyoteJump = true;
    PlayerInput controls;

    #region movement & attack with animation controls

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        animator = GetComponent<Animator>();

        startSpeed = startRunSpeed;
        startJump = jumpVelocity;
        startGrav = rb.gravityScale;
    }

    private void Start()
    {
        speed = startRunSpeed;
    }

    private void Update()
    {
        horizontal = controls.actions["Move"].ReadValue<float>();

        if (jumpRate >= 0f)
        {
            jumpRate -= Time.deltaTime;
        }

        if (isDashing)
        {
            multiJump = 0;
        }

        else if (!isDashing)
        {
            if (!IsGrounded())
            {
                coyoteTime -= Time.deltaTime;

                if (animator)
                {
                    animator.SetBool("isJumping", true);
                }

                if (canCoyoteJump)
                {
                    if (coyoteTime <= 0f)
                    {
                        canCoyoteJump = false;
                    }
                }

                if (jumpRate <= 0f)
                {
                    rb.gravityScale += Time.deltaTime * 7f;
                }
            }
            else if (IsGrounded())
            {
                coyoteTime = .1f;
                multiJump = 0;
                rb.gravityScale = startGrav;

                if (animator)
                {
                    animator.SetBool("isJumping", false);
                }

                if (!canCoyoteJump)
                {
                    canCoyoteJump = true;
                }
            }

            if (controls.actions["Jump"].triggered)
            {
                Jump();
            }

            /*else if (!IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                if (coyoteTime >= 0f)
                {
                    rb.AddForce(new Vector2(0f, jumpVelocity));
                }
            }
            
            if (GameManager.doubleJumpUnlocked)
            {
                if (!IsGrounded() && Input.GetKeyDown(KeyCode.Space) && multiJump == 0)
                {
                    if (jumpRate <= 0f)
                    {
                        Jump();
                        multiJump++;
                        Instantiate(bloodJump, jumpPoint);
                    }
                }
            }*/
        }
        /*if (animator)
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsTag("idle"))
            {
                if (controls.Smoop.ShootLeft.WasPerformedThisFrame())
                {
                    horizontalShoot = -1f;
                }
                else if (controls.Smoop.ShootRight.WasPerformedThisFrame())
                {
                    horizontalShoot = 1f;
                }
                else
                {
                    horizontalShoot = 0f;
                }
            }
        }*/

        if (CameraZoom.levelCleared)
        {
            if (speed <= startSpeed * 3)
            {
                speed += startSpeed * 3 * Time.deltaTime;
                jumpVelocity += startJump * 3 * Time.deltaTime;
            }
        }

        /*if (animator)
        {
            if (facingRight && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                animator.SetTrigger("backAttack");
            }
            else if (!facingRight && Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.SetTrigger("backAttack");
            }
        }*/

        //AttackButton();

    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            if (animator)
            {
                if (horizontal > 0f || horizontal < 0f)
                {
                    animator.SetFloat("Speed", 1f);
                }
                else if (horizontal == 0f)
                {
                    animator.SetFloat("Speed", 0f);
                }
            }

            if (jumpRate > 0f)
            {
                rb.AddForce(new Vector2(0f, jumpVelocity * Time.deltaTime));
            }
        }

        //ResetAttack();
        //PlayerAttack();
        
        KeyMovement(horizontal);
        Flip(horizontal);
    }

    private void KeyMovement(float horizontal)
    {
        if (horizontal != 0f && speed < maxSpeed)
        {
            speed += accelerationFactor * Time.fixedDeltaTime;

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }
        else if (horizontal == 0f && speed > startRunSpeed)
        {
            speed -= (accelerationFactor * 1.1f)  * Time.fixedDeltaTime;

            if (speed < startRunSpeed)
            {
                speed = startRunSpeed;
            }
        }

        if (horizontal > 0f)
        {
            rb.AddForce(new Vector2(speed * Time.deltaTime, 0f));
        }
        else if (horizontal < 0f)
        {
            rb.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
        }

        /*if (IsGrounded())
        {
            if (horizontal > 0f)
            {
                rb.AddForce(new Vector2(speed * Time.deltaTime, 0f));
            }
            else if (horizontal < 0f)
            {
                rb.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
            }
        }
        else if (!IsGrounded())
        {
            if (horizontal > 0f)
            {
                rb.AddForce(new Vector2(speed * Time.deltaTime, 0f));
            }
            else if (horizontal < 0f)
            {
                rb.AddForce(new Vector2(-speed * Time.deltaTime, 0f));
            }
        }*/
    }

    private void Jump()
    {
        if (canCoyoteJump && jumpRate <= 0f)
        {
            canCoyoteJump = false;

            jumpRate = 0.1f;
            rb.gravityScale = startGrav;
        }

        if (GameManager.doubleJumpUnlocked)
        {
            if (!IsGrounded() && multiJump == 0)
            {
                if (jumpRate <= 0f)
                {
                    multiJump++;
                    Instantiate(bloodJump, jumpPoint);

                    jumpRate = 0.1f;
                    rb.gravityScale = startGrav;
                }
            }
        }

        /*float xMovement = rb.velocity.x;
        float yMovement = rb.velocity.y;
        rb.velocity = new Vector2(xMovement, yMovement + jumpVelocity * Time.deltaTime);*/
    }

    /*private void PlayerAttack()
    {
        if (!isDashing)
        {
            if (attack)
            {
                animator.SetTrigger("playerAttack");
            }
        }
    }*/

    private void AttackButton()
    {
        if (!isDashing)
        {
            if (Time.time >= shootTime)
            {
                shootTime = Time.time + 1f / shootSpeed;
            }
        }
    }

    /*private void ResetAttack()
    {
        attack = false;
    }*/

    private void Flip(float horizontal)
    {
        if (!isDashing)
        {
            /*if (horizontalShoot != 0f)
            {
                if (horizontalShoot < 0 && facingRight || horizontalShoot > 0 && !facingRight)
                {
                    facingRight = !facingRight;

                    if (lifeBar)
                    {
                        lifeBar.transform.Rotate(0f, 180f, 0f);
                    }

                    transform.Rotate(0f, 180f, 0f);

                    horizontalShoot = 0f;
                }
            }*/
            if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
            {
                facingRight = !facingRight;

                if (lifeBar)
                {
                    lifeBar.transform.Rotate(0f, 180f, 0f);
                }
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    public bool IsGrounded()
    {
        float extraHeightTest = .05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightTest, platformLayerMask);

        return raycastHit.collider != null;
    }

    /*private void ShootDirection(float horizontalShoot)
    {
        if (horizontalShoot < 0 && facingRight || horizontalShoot > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }*/

    public void SlowMoGone()
    {
        jumpVelocity = startJump;
        speed = maxSpeed;
    }

    public void HorizontalShoot(bool left)
    {
        AttackButton();
        
        if (animator)
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsTag("idle"))
            {
                if (left)
                {
                    if (facingRight)
                    {
                        facingRight = !facingRight;

                        if (lifeBar)
                        {
                            lifeBar.transform.Rotate(0f, 180f, 0f);
                        }
                        transform.Rotate(0f, 180f, 0f);
                    }
                }
                else if (!left)
                {
                    if (!facingRight)
                    {
                        facingRight = !facingRight;

                        if (lifeBar)
                        {
                            lifeBar.transform.Rotate(0f, 180f, 0f);
                        }
                        transform.Rotate(0f, 180f, 0f);
                    }
                }
            }
            else
            {
                if (facingRight && left)
                {
                    animator.SetTrigger("backAttack");
                }
                else if (!facingRight && !left)
                {
                    animator.SetTrigger("backAttack");
                }
            }
        }
    }

   #endregion
}
