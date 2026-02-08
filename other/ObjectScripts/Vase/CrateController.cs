using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    CullingManager cull;
    AudioSource noise;
    public AudioSource explosion;
    float lastV;
    
    private void Awake()
    {
        if (GameObject.Find("CullingManager"))
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
        }

        if (cull)
        {
            cull.AddThis(this.gameObject);
        }

        noise = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Rigidbody2D thisRigid = GetComponent<Rigidbody2D>();

        if (Mathf.Abs(thisRigid.velocity.x) > .3f && Mathf.Abs(thisRigid.velocity.y) <.1f)
        {
            if (!noise.isPlaying)
            {
                noise.Play();
            }
        }
        else
        {
            if (noise.isPlaying)
            {
                noise.Stop();
            }
        }

        lastV = Mathf.Abs(thisRigid.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && lastV >= 5f)
        {
            explosion.Play();
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Head") || collision.gameObject.CompareTag("dontReact") || collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }*/
}
