using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WonScreenNumberSetter : MonoBehaviour
{
    public Text enemies;
    public Text timer;
    public Text savingGems;
    Tracker tracker;
    int enemiesKilled;
    int enemiesTotal;
    float bestTimer;
    float bestMinutes;
    float gemTimer;

    private void Awake()
    {
        if (GameObject.Find("Tracker"))
        {
            tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
        }

        enemiesKilled = tracker.totalEnemiesKilled;
        enemiesTotal = tracker.totalEnemies;
        bestTimer = tracker.timer;
        bestMinutes = tracker.minutes;
    }

    private void Start()
    {
        savingGems.text = "Saving gems.";
        enemies.text = enemiesKilled + " / " + enemiesTotal;

        if (bestTimer < 10f)
        {
            timer.text = $"{bestMinutes}:0" + bestTimer.ToString("F3");
        }
        else if (bestTimer >= 10f)
        {
            timer.text = $"{bestMinutes}:" + bestTimer.ToString("F3");
        }
    }

    private void Update()
    {
        gemTimer += Time.unscaledDeltaTime;

        if (gemTimer <= .66f)
        {
            savingGems.text = "Saving gems..";
        }
        else if (gemTimer > .66f && gemTimer <= 1.3f)
        {
            savingGems.text = "Saving gems...";
        }
        else if (gemTimer > 1.3f)
        {
            savingGems.text = "Gems saved.";
        }
        
        /*if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().NextLevel();
        }*/
    }
}
