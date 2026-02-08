using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseController : MonoBehaviour
{
    public Sprite crackedVer;
    public SpriteRenderer sr;
    public GameObject shatteredVer;
    bool hitOnce = false;
    CullingManager cull;

    private void Awake()
    {
        if (GameObject.Find("CullingManager"))
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
            cull.AddThis(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag ("Bullet") || collision.CompareTag("Ball"))
        {
            if (collision.CompareTag("Bullet"))
            {
                BloodBallShoot rightBall = collision.GetComponent<BloodBallShoot>();
                BloodBallShootLeft leftBall = collision.GetComponent<BloodBallShootLeft>();

                if (rightBall)
                {
                    rightBall.Animations();
                }
                if (leftBall)
                {
                    leftBall.Animations();
                }
            }

            if (!hitOnce)
            {
                sr.sprite = crackedVer;
                hitOnce = true;
            }
            else if (hitOnce)
            {
                Shatter();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= 5 && collision.relativeVelocity.magnitude <= 10)
        {
            if (!hitOnce)
            {
                sr.sprite = crackedVer;
                hitOnce = true;
            }
            else if (hitOnce)
            {
                Shatter();
            }
        }
        if (collision.relativeVelocity.magnitude > 10)
        {
            Shatter();
        }

        if (collision.gameObject.CompareTag("Knight"))
        {
            Shatter();
        }
    }

    public void Shatter()
    {
        if (cull)
        {
            cull.RemoveThis(this.gameObject);
        }

        Instantiate(shatteredVer, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
