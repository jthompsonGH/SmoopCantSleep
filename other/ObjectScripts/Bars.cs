using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bars : MonoBehaviour
{
    public GameObject topBar;
    public GameObject bottomBar;
    public static bool brokeOut;
    public float moveDistance;
    bool destroyed;
    float timer;
    Vector2 topY;
    Vector2 bottomY;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level1")
        {
            brokeOut = true;
        }
        
        if (!destroyed)
        {
            topY = topBar.transform.position;
            bottomY = bottomBar.transform.position;

            if (brokeOut)
            {
                topY.y += Time.deltaTime * moveDistance;
                bottomY.y -= Time.deltaTime * moveDistance;

                topBar.transform.position = topY;
                bottomBar.transform.position = bottomY;

                timer += Time.deltaTime;

                if (timer >= 2f)
                {
                    destroyed = true;
                    Destroy(topBar);
                    Destroy(bottomBar);
                }
            }
        }
    }
}
