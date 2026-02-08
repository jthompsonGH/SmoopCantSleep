using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DorpBallShoot : MonoBehaviour
{
    public GameObject dorpBallPop;
    public Rigidbody2D rb;
    public float speed;
    public int damage = 1;
    GameObject Player;
    bool isDash;

    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
        {
            isDash = Player.GetComponent<PlayerController>().dashProtect;
        }
    }

    public void Animations()
    {
        Instantiate(dorpBallPop, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDash = collision.GetComponent<PlayerController>().dashProtect;
        }
       
        if (collision.CompareTag("Bullet"))
        {
            Animations();
        }
        
        if (!isDash)
        {
            if (!collision.CompareTag("Dorp") && !collision.CompareTag("Swarmer") && collision.gameObject.name != "Blast" && !collision.CompareTag("Ball"))
            {
                Animations();
            }
        }
        if (isDash)
        {
            if (!collision.CompareTag("Player") && !collision.CompareTag("Dorp") && collision.gameObject.name != "Blast" && !collision.CompareTag("Ball"))
            {
                Animations();
            }
        }
    }
}
