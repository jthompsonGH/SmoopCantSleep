using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ShieldGoFly : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 velocity;
    float velocityX;
    float velocityY;
    float despawnCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        velocityX = Random.Range(5f, 8f);
        velocityY = Random.Range(5f, 8f);

        velocity = new Vector2(velocityX, velocityY);

        rb.velocity = velocity;

        despawnCounter = 0f;
    }

    private void Update()
    {
        despawnCounter += Time.deltaTime;

        if (despawnCounter >= 10f)
        {
            Destroy(gameObject);
        }

        if (rb.angularVelocity > 20f)
        {
            Destroy(gameObject);
        }
    }
}
