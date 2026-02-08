using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    GameObject candleLight;
    GameObject outLight;
    AudioSource noise;
    float objectTimer;
    bool wentOut = false;
    bool changeToObject = false;

    private void Start()
    {
        noise = GetComponent<AudioSource>();
        
        candleLight = this.gameObject.transform.GetChild(0).gameObject;
        outLight = this.gameObject.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (Mathf.Abs(rb.velocity.x) > 2f || Mathf.Abs(rb.velocity.y) > 2f)
        {
            if (!wentOut)
            {
                GoOut();
            }
        }
        
        if (candleLight.activeSelf)
        {
            if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
            {
                if (!GetComponent<LampController>().enabled)
                {
                    GetComponent<LampController>().enabled = true;
                }
            }
        }

        if (wentOut)
        {
            if (!changeToObject)
            {
                objectTimer += Time.deltaTime;

                if (objectTimer >= .3f)
                {
                    changeToObject = true;
                    this.gameObject.layer = 10;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (candleLight.activeSelf)
        {
            if (!wentOut)
            {
                GoOut();
            }
        }
    }

    void GoOut()
    {
        wentOut = true;
        noise.Play();

        candleLight.SetActive(false);
        outLight.SetActive(true);
    }
}
