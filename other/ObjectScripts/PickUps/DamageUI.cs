using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    public static bool poweredUP;
    public Slider slider;
    public GameObject effects;
    float timer;

    private void Update()
    {
        if (timer > 0f && poweredUP)
        {
            timer -= Time.deltaTime;
        }

        if (poweredUP && timer <= 0f)
        {
            poweredUP = false;
            timer = 0f;
            effects.SetActive(false);
        }

        slider.value = timer;
    }

    public void PickedUp()
    {
        if (!effects.activeSelf)
        {
            effects.SetActive(true);
        }
        timer = 10f;
        poweredUP = true;
    }
}
