using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepFocusedOnPlayer : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    GameObject head;
    GameObject player;

    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.Find("Player");
        head = GameObject.Find("smoopDeath");

        if (player != null)
        {
            vCam.m_LookAt = player.transform;
            vCam.m_Follow = player.transform;
            vCam.m_Priority = 11;
        }
        if (head != null)
        {
            vCam.m_LookAt = head.transform;
            vCam.m_Follow = head.transform;
            vCam.m_Priority = 11;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
