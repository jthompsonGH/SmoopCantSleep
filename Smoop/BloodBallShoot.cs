using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BloodBallShoot : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public GameObject impact;
    public GameObject shielded;
    public GameObject poweredLight;
    public int damage;
    public bool didDamage = false;
    GameObject player;
    float deleteTimer;

    // Start is called before the first frame update

    void Start()
    {
        damage = 1;
        
        if (DamageUI.poweredUP)
        {
            speed = 25;
            poweredLight.SetActive(true);
            damage = 3;
        }
        
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        
        player = GameObject.FindGameObjectWithTag("Player");

        rb.velocity = transform.right * speed;

        deleteTimer = 0f;
    }

    private void Update()
    {
        if (DamageUI.poweredUP)
        {
            damage = 3;
        }

        deleteTimer += Time.deltaTime;
        
        if (player != null)
        {
            CheckBall();
        }
        if (player == null)
        {
            Animations();
        }
    }

    public void Animations()
    {
        Instantiate(impact, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void ShieldUp()
    {
        Instantiate(shielded, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
    void CheckBall()
    {
        if (deleteTimer >= .6f || Vector2.Distance(transform.position, player.transform.position) > 7f)
        {
            Invoke("Animations", .2f);
        }
    }
}