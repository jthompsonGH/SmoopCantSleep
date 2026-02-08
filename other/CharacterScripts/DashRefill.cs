using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashRefill : MonoBehaviour
{
    public GameObject dashUI;
    GameObject player;
    Image image;

    private void Start()
    {
        if (!GameManager.dashUnlocked)
        {
            dashUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        image = GetComponent<Image>();

        if (player != null)
        {
            image.fillAmount = player.GetComponent<DashMove>().dashReady * 3.3f;
        }
    }
}
