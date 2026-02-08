using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Material whiteMat;
    public Material imageMat;
    public float reductionSpeed;
    public bool isPlayer = false;
    public bool isLifeBar = false;
    Image image;
    Quaternion xRotation;
    Vector2 startPos;
    KnightController thisKnight;
    DorpController thisDorp;
    float health;
    bool healing = false;
    bool hurt = false;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        
        thisKnight = transform.GetComponentInParent<KnightController>();
        thisDorp = transform.GetComponentInParent<DorpController>();
    }

    private void Start()
    {
        startPos = transform.localPosition;

        if (thisKnight || thisDorp)
        {
            xRotation = transform.rotation;
        }

        if (isPlayer)
        {
            //regularMat = image.material;
        }

        if (isLifeBar)
        {
            if (!GameManager.meleeUnlocked)
            {
                transform.localPosition = new Vector2(-361f, startPos.y);
            }
        }
    }

    private void Update()
    {
        if (thisKnight || thisDorp)
        {
            transform.rotation = xRotation;
        }

        if (slider.value > health)
        {
            if (healing)
            {
                slider.value = health;
                healing = false;
            }
            else
            {
                ReduceHealth();
            }
        }
        else if (slider.value < health)
        {
            if (hurt)
            {
                slider.value = health;
                hurt = false;
            }
            else
            {
                IncreaseHealth();
            }
        }
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        health = maxHealth;
    }

    public void SetHealth(float newHealth, bool newLifeBar = false)
    {
        if (newLifeBar)
        {
            slider.value = newHealth;
        }
        
        health = newHealth;
    }

    void ReduceHealth()
    {
        slider.value -= Time.deltaTime * reductionSpeed;

        if (!hurt)
        {
            hurt = true;
        }
    }

    void IncreaseHealth()
    {
        slider.value += Time.deltaTime * reductionSpeed;

        if (!healing)
        {
            healing = true;
        }
    }

    public IEnumerator Flash()
    {
        image.material = whiteMat;

        yield return new WaitForSecondsRealtime(.1f);

        image.material = imageMat;
    }

    public void MoveLifeBar()
    {
        transform.localPosition = new Vector2(startPos.x, startPos.y);
    }
}
