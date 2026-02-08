using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BloodBall : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public Transform flashPoint;
    public Transform leftFirePoint;
    public Transform leftFlashPoint;
    public GameObject muzzleFlash;
    public GameObject leftMuzzleFlash;
    public GameObject bloodBall;
    public GameObject leftbloodball;
    DashMove dashScript;
    PlayerController PC;
    GameObject player;
    float attackRate;
    float nextAttackTime = 0f;
    bool facingRight;
    PlayerInput controls;
    PlayerMovement movement;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        dashScript = GetComponent<DashMove>();
        PC = GetComponent<PlayerController>();
        
        attackRate = GetComponent<PlayerMovement>().shootSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            facingRight = player.GetComponent<PlayerMovement>().facingRight;
        }
        else if (player == null)
        {
            facingRight = true;
        }

        if (player != null)
        {
            if (!PauseMenu.paused)
            {
                if (Time.time >= nextAttackTime)
                {
                    if (controls.actions["Shoot Left"].triggered)
                    {
                        DoShoot(true);
                    }
                    if (controls.actions["Shoot Right"].triggered)
                    {
                        DoShoot(false);
                    }
                }
            }
        }
    }

    void DoShoot(bool left)
    {
        if (player != null)
        {
            if (!PauseMenu.paused)
            {
                if (Time.time >= nextAttackTime)
                {
                    if (left)
                    {
                        LeftShoot();
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                    if (!left)
                    {
                        RightShoot();
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }
            }
        }
    }
    
    void RightShoot()
    {
        if (facingRight)
        {
            Instantiate(bloodBall, firePoint.position, firePoint.rotation);
            Instantiate(muzzleFlash, flashPoint.position, flashPoint.rotation);
        }
        if (!facingRight)
        {
            Instantiate(leftbloodball, leftFirePoint.position, leftFirePoint.rotation);
            Instantiate(leftMuzzleFlash, leftFlashPoint.position, leftFlashPoint.rotation);
        }

        if (dashScript.enabled && PC.dashProtect)
        {
            dashScript.DashDisabler(); dashScript.GravityReset();
            PC.dashProtect = false;
        }

        if (movement)
        {
            movement.HorizontalShoot(false);
        }
    }

    void LeftShoot()
    {
        if (facingRight)
        {
            Instantiate(leftbloodball, leftFirePoint.position, leftFirePoint.rotation);
            Instantiate(leftMuzzleFlash, leftFlashPoint.position, leftFlashPoint.rotation);
        }
        if (!facingRight)
        {
            Instantiate(bloodBall, firePoint.position, firePoint.rotation);
            Instantiate(muzzleFlash, flashPoint.position, flashPoint.rotation);
        }

        if (dashScript.enabled && PC.dashProtect)
        {
            dashScript.DashDisabler(); dashScript.GravityReset();
            PC.dashProtect = false;
        }

        if (movement)
        {
            movement.HorizontalShoot(true);
        }
    }



}
