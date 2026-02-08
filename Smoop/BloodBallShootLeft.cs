using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBallShootLeft : MonoBehaviour
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
            damage = 3;
            poweredLight.SetActive(true);
        }

        GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        player = GameObject.FindGameObjectWithTag("Player");

        damage = 1;

        rb.velocity = transform.right * -speed;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Caster"))
        {
            ShieldUp();
        }
    }

    void CheckBall()
    {
        if (deleteTimer >= .6f || Vector2.Distance(transform.position, player.transform.position) > 7f)
        {
            Invoke("Animations", .2f);
        }
    }
}

