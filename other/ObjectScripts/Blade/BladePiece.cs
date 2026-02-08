using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladePiece : MonoBehaviour
{
    public float force;
    Rigidbody2D rb;
    float timer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timer = 8f;

        rb.AddRelativeForce(new Vector2(0f, -force), ForceMode2D.Impulse);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
