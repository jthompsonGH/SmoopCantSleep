using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRed : MonoBehaviour
{
    public GameObject gemPop;
    public GameObject noSeek;
    GameObject player;
    MoneyUI moneyUI;
    Rigidbody2D rb;
    float velocityX;
    float velocityY;
    float deciderX;
    float despawnTimer;
    bool cashed = false;

    private void Awake()
    {
        moneyUI = GameObject.Find("MoneyTracker").GetComponent<MoneyUI>();
    }

    private void Start()
    {
        deciderX = Random.Range(-4f, 4f);

        if (deciderX < 0f)
        {
            velocityX = -3f;
        }
        else if (deciderX == 0f)
        {
            velocityX = 0f;
        }
        else if (deciderX > 0f)
        {
            velocityX = 3f;
        }

        velocityY = Random.Range(4f, 7f);

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(velocityX, velocityY);

        rb.AddTorque((Random.Range(1f, 2f)));
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= .5f)
            {
                if (!cashed)
                {
                    cashed = true;
                    moneyUI.AddGems(200);
                    Instantiate(gemPop, transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }

        despawnTimer += Time.deltaTime;

        if (despawnTimer >= 10f)
        {
            Instantiate(noSeek, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
