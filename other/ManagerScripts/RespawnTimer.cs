using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour
{
    GameObject head;
    Image image;
    
    // Update is called once per frame
    void Update()
    {
        head = GameObject.FindGameObjectWithTag("Head");

        if (head != null)
        {
            image = GetComponent<Image>();

            image.fillAmount = head.GetComponent<HeadController>().respawnTimer / 3f;
        }

        if (head == null)
        {
            Destroy(gameObject);
        }
    }
}
