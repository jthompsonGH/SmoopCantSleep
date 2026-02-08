using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DestroyProjectiles : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
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
        if (collision.CompareTag("Ball"))
        {
            collision.GetComponent<DorpBallShoot>().Animations();
        }
    }
}
