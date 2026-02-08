using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CoffinController : MonoBehaviour
{
    public GameObject pieces;
    public GameObject player;
   // public GameObject particles;
    public CinemachineImpulseSource thump;
    public CinemachineImpulseSource breakout;
    public Transform spawnPoint;
    public AudioClip knock;
    public AudioClip explode;
    public AudioSource source;
    CinemachineVirtualCamera vCam;
    SpriteRenderer sr;
    bool brokeOut;
    float timer;
    int maxLevel;
    int timesThumped = 0;
    string username;

    private void Awake()
    { 
        source.clip = knock;
        
        if (Bars.brokeOut)
        {
            Bars.brokeOut = false;
        }
        
        vCam = GameObject.Find("vCam").GetComponent<CinemachineVirtualCamera>();
        if (vCam)
        {
            vCam.m_Follow = this.gameObject.transform;
        }
        
        brokeOut = false;

        SaveMoney money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            maxLevel = money.maxLevel;
        }
        else if (money == null)
        {
            maxLevel = 3;
        }

        timer = 0f;

        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (maxLevel != 3)
        {
            timer = 1f;
            timesThumped = 2;
        }
    }

    private void Update()
    {
        if (!brokeOut)
        {
            timer += Time.deltaTime;

            if (timer >= 2f)
            {
                timer = 0f;

                if (timesThumped != 2)
                {
                    Thump();
                }
                else if (timesThumped == 2)
                {
                    Breakout();
                }
            }
        }
    }

    void Thump()
    {
        timesThumped++;
        source.Play();

        thump.GenerateImpulse();
    }
    
    void Breakout()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().Coffin();
        
        Bars.brokeOut = true;

        source.clip = explode;
        source.Play();
        
        brokeOut = true;
        sr.enabled = false;

        //Instantiate(particles, transform.position, Quaternion.identity);
        pieces.SetActive(true);

        Instantiate(player, spawnPoint.position, Quaternion.identity);
        
        breakout.GenerateImpulse();

        this.enabled = false;
    }
}
