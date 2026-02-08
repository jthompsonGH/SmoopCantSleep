using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class VinePieces : MonoBehaviour
{
    public GameObject bottomVine;
    public GameObject particles;
    public VinePieces[] underVines;
    Transform particlePoint;
    HingeJoint2D joint;
    bool broke;
    float timer;

    private void Start()
    {
        particlePoint = transform.GetChild(0).transform;

        joint = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (broke)
        {
            timer += Time.deltaTime;

            if (timer >= 8f)
            {
                if (bottomVine)
                {
                    Destroy(bottomVine);
                }
                
                Destroy(this.gameObject);
            }
        }
    }

    private void OnJointBreak2D(Joint2D hingeJoint)
    {
        Disconnected();

        foreach (VinePieces vinePiece in underVines)
        {
            if (vinePiece != null)
            {
                vinePiece.Disconnected();
            }
        }
    }

    public void Disconnected()
    {
        broke = true;

        Instantiate(particles, particlePoint.position, transform.rotation);

        if (bottomVine)
        {
            bottomVine.layer = 10;
        }

        if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 4f));
        }
        
        this.gameObject.layer = 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" || collision.gameObject.tag == "Bullet")
        {
            if (joint)
            {
                joint.breakForce = 0f;
            }

            if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, 0f));
            }
        }
    }
}
