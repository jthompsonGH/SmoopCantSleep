using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsController : MonoBehaviour
{
    public Text tipText;
    public GameObject[] arrows;
    public string[] tips;
    float timer;
    int nextTip;

    private void Awake()
    {
        nextTip = 0;

        tipText.text = tips[nextTip];
    }

    private void Update()
    {
        if (PauseMenu.paused)
        {
            tipText.enabled = true;

            foreach (GameObject arrowButton in arrows)
            {
                if (!arrowButton.activeSelf)
                {
                    arrowButton.SetActive(true);
                }
            }
        }
        else if (!PauseMenu.paused)
        {
            tipText.enabled = false;

            foreach (GameObject arrowButton in arrows)
            {
                if (arrowButton.activeSelf)
                {
                    arrowButton.SetActive(false);
                }
            }
        }
        
        timer += Time.unscaledDeltaTime;

        if (timer >= 8f)
        {
            NextTip();
        }
    }

    public void NextTip()
    {
        timer = 0f;
        
        nextTip++;

        if (nextTip > tips.Length - 1)
        {
            nextTip = 0;
        }

        tipText.text = tips[nextTip];
    }

    public void LastTip()
    {
        timer = 0f;

        nextTip--;

        if (nextTip < 0)
        {
            nextTip = tips.Length - 1;
        }

        tipText.text = tips[nextTip];
    }
}
