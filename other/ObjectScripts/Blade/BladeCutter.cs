using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BladeCutter : MonoBehaviour
{
    public CinemachineImpulseSource impulse;
    public BladeSpinner motor;
    public BladeCutter otherBlade;
    public GameObject brokenImage;
    public GameObject bladeDrop;
    public GameObject bladeBreaks;
    public Transform particlePoint;
    bool hit = false;
    int swarmersHit = 0;

    private void OnCollisionEnter2D (Collision2D collision)
    {
        string thingHit = collision.gameObject.tag;

        if (motor.canSlice)
        {
            switch (thingHit)
            {
                case "Player":
                    if (!hit)
                    {
                        hit = true;
                        collision.gameObject.GetComponent<PlayerController>().TakeDamage(3);
                        Break();
                    }
                    break;
                case "Head":
                    if (!hit)
                    {
                        hit = true;
                        collision.gameObject.GetComponent<HeadController>().TakeDamage(3);
                        Break();
                    }
                    break;
                case "Knight":
                    if (!hit)
                    {
                        Animator knightAnim = collision.gameObject.GetComponent<Animator>();
                        hit = true;

                        if (knightAnim.GetCurrentAnimatorStateInfo(0).IsTag("noShield"))
                        {
                            collision.gameObject.GetComponent<KnightController>().TakeDamage(3);
                        }
                        else
                        {
                            collision.gameObject.GetComponent<KnightController>().ShieldBreak();
                        }
                        Break();
                    }
                    break;
                case "Swarmer":
                    SwarmerController isSwarmer = collision.gameObject.GetComponent<SwarmerController>();
                    ExploderController isExploder = collision.gameObject.GetComponent<ExploderController>();

                    if (isSwarmer)
                    {
                        swarmersHit++;
                        isSwarmer.Die();
                        if (swarmersHit == 3)
                        {
                            Break();
                        }
                    }
                    else if (isExploder)
                    {
                        isExploder.Explode();
                        Break();
                    }
                    break;
            }
        }
    }

    void Break()
    {
        motor.NotWork();
        DisableCollider();
        otherBlade.DisableCollider();
        brokenImage.SetActive(true);
        bladeDrop.SetActive(true);
        impulse.GenerateImpulse();
        Instantiate(bladeBreaks, particlePoint.position, transform.rotation);
        Destroy(this.gameObject);

    }

    public void DisableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        otherBlade.gameObject.transform.GetChild(0).GetComponent<LightDim>().enabled = true;
    }
}
