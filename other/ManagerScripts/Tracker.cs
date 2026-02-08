using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tracker : MonoBehaviour
{
    public GameObject dorp;
    public GameObject swarmer;
    public GameObject knight;
    public MoneyUI money;
    public Text dorpText;
    public Text swarmerText;
    public Text knightText;
    public Text timerText;
    public Text bestEnemies;
    public Text bestDifficulty;
    public Text bestTime;
    public string difficulty;
    public int dorps = 0;
    public int swarmers = 0;
    public int knights = 0;
    public int totalEnemies = 0;
    public int totalEnemiesKilled = 0;
    public int difficultySetting;
    public float timer = 0f;
    public float minutes = 0f;
    public float gameTime = 0f;
    public float parTime;
    public bool playerMoved = false;
    public bool parBonus = false;
    public bool clearedBonus = false;
    public bool noDmgBonus = false;
    public bool noDeathBonus = false;
    string newDifficulty;
    string username;
    string thisLevelName;
    int value;
    GameObject player;
    Vector2 startPos;
    Vector2 playerPos;


    private void Awake()
    {
        thisLevelName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        difficultySetting = PlayerPrefs.GetInt("difficultySetting");

        switch (difficultySetting)
        {
            case 1:
                difficulty = "CAKEWALK";
                break;
            case 2:
                difficulty = "EASY";
                break;
            case 3:
                difficulty = "NORMAL";
                break;
            case 4:
                difficulty = "HARD";
                break;
            case 5:
                difficulty = "INSANE";
                break;
        }
        
        dorps = KillManager.dorpsKilled;
        swarmers = KillManager.swarmersKilled;
        knights = KillManager.knightsKilled;

        int[] enemies = { dorps, swarmers, knights };

        if (enemies[0] > 0)
        {
            dorp.SetActive(true);
            dorpText.text = "0";
        }
        if (enemies[1] > 0)
        {
            swarmer.SetActive(true);
            swarmerText.text = "0";
        }
        if (enemies[2] > 0)
        {
            knight.SetActive(true);
            knightText.text = "0";
        }

        timerText.text = $"{minutes}:0" + timer.ToString("F3");
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (!playerMoved)
        {
            if (player)
            {
                playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

                if (Vector2.Distance(new Vector2(playerPos.x, 0f), new Vector2(startPos.x, 0f)) >  .05f)
                {
                    playerMoved = true;
                }
            }
        }

        if (playerMoved)
        {
            Timers();
        }

        if (timer >= 60f)
        {
            timer = 0f;
            minutes += 1;
        }
        
        if (timer < 10f)
        {
            timerText.text = $"{minutes}:0"+ timer.ToString("F3");
        }
        else if (timer >= 10f)
        {
            timerText.text = $"{minutes}:" + timer.ToString("F3");
        }
    }

    public void UpdateText(string enemy)
    {
        switch (enemy)
        {
            case "dorp":
                if (!dorp.activeSelf)
                {
                    dorp.SetActive(true);
                }
                dorps++;
                KillManager.dorpsKilled++;
                dorpText.text = dorps.ToString();
                break;

            case "swarmer":
                if (!swarmer.activeSelf)
                {
                    swarmer.SetActive(true);
                }
                swarmers++;
                KillManager.swarmersKilled++;
                swarmerText.text = swarmers.ToString();
                break;

            case "knight":
                if (!knight.activeSelf)
                {
                    knight.SetActive(true);
                }
                knights++;
                KillManager.knightsKilled++;
                knightText.text = knights.ToString();
                break;
        }

        if (totalEnemiesKilled + 1 == totalEnemies)
        {
            GameObject[] dorpBalls = GameObject.FindGameObjectsWithTag("Ball");

            foreach (GameObject ball in dorpBalls)
            {
                if (ball.GetComponent<DorpBallShoot>())
                {
                    ball.GetComponent<DorpBallShoot>().Animations();
                }
            }

            clearedBonus = true;
            
            StartCoroutine(GameObject.Find("vCam").GetComponent<CameraZoom>().ClearedCam());

            if (GameObject.Find("CullingManager"))
            {
                GameObject.Find("CullingManager").GetComponent<CullingManager>().ClearRemaining();
            }
        }

        totalEnemiesKilled++;
    }

    public void LevelBeaten(string name, bool par, bool cleared, bool dmg, bool death)
    {
        SaveBests bests = SaveSystem.LoadBests(name);

        bool saveKills = false;
        bool saveTime = false;
        bool saveDiff = false;

        if (bests != null)
        {

            if (bests.deadEnemies >= totalEnemiesKilled)
            {
                bestEnemies.text = "BEST: " + bests.deadEnemies.ToString() + " / " + totalEnemies.ToString();
                saveKills = false;
            }
            else if (totalEnemiesKilled > bests.deadEnemies)
            {
                bestEnemies.text = "NEW BEST: " + totalEnemiesKilled.ToString() + " / " + totalEnemies.ToString();
                saveKills = true;
            }


            if (bests.difficultyInt > difficultySetting)
            {
                saveDiff = false;
                bestDifficulty.text = $"{SavedDifficulty(bests.difficultyInt)}";
            }
            else if (bests.difficultyInt <= difficultySetting)
            {
                saveDiff = true;
                bestDifficulty.text = $"{difficulty}";
            }

            if (bests.minutes == this.minutes)
            {
                if (bests.seconds <= timer)
                {
                    if (bests.seconds < 10f)
                    {
                        bestTime.text = "BEST: " + $"{bests.minutes}:0" + bests.seconds.ToString("F3");
                    }
                    else if (bests.seconds >= 10f)
                    {
                        bestTime.text = "BEST: " + $"{bests.minutes}:" + bests.seconds.ToString("F3");
                    }

                    saveTime = false;
                }
                else if (bests.seconds > timer)
                {
                    if (timer < 10f)
                    {
                        bestTime.text = "NEW BEST: " + $"{this.minutes}:0" + timer.ToString("F3");
                    }
                    else if (bests.seconds >= 10f)
                    {
                        bestTime.text = "NEW BEST: " + $"{this.minutes}:" + timer.ToString("F3");
                    }

                    saveTime = true;
                }
            }
            else if (bests.minutes > this.minutes)
            {
                if (timer < 10f)
                {
                    bestTime.text = "NEW BEST: " + $"{this.minutes}:0" + timer.ToString("F3");
                }
                else if (bests.seconds >= 10f)
                {
                    bestTime.text = "NEW BEST: " + $"{this.minutes}:" + timer.ToString("F3");
                }

                saveTime = true;
            }
            else if (bests.minutes < this.minutes)
            {
                if (bests.seconds < 10f)
                {
                    bestTime.text = "BEST: " + $"{bests.minutes}:0" + bests.seconds.ToString("F3");
                }
                else if (bests.seconds >= 10f)
                {
                    bestTime.text = "BEST: " + $"{bests.minutes}:" + bests.seconds.ToString("F3");
                }

                saveTime = false;
            }
            if (!saveKills)
            {
                totalEnemiesKilled = bests.deadEnemies;
            }

            if (!saveTime)
            {
                timer = bests.seconds;
                this.minutes = bests.minutes;
            }

            if (!saveDiff)
            {
                difficultySetting = bests.difficultyInt;
            }

            if (!par)
            {
                this.parBonus = bests.parBonus;
            }

            if (!dmg)
            {
                this.noDmgBonus = bests.noDmgBonus;
            }

            if (!cleared)
            {
                this.clearedBonus = bests.clearedBonus;
            }

            if (!death)
            {
                this.noDeathBonus = bests.noDeathBonus;
            }
        }
        else if (bests == null)
        {
            bestEnemies.text = "NEW BEST: " + totalEnemiesKilled.ToString() + " / " + totalEnemies.ToString();
            bestDifficulty.text = $"{difficulty}";

            saveKills = true;
            saveDiff = true;

            if (timer < 10f)
            {
                bestTime.text = "NEW BEST: " + $"{this.minutes}:0" + timer.ToString("F3");
            }
            else if (timer >= 10f)
            {
                bestTime.text = "NEW BEST: " + $"{this.minutes}:" + timer.ToString("F3");
            }

            saveTime = true;
        }


        SaveSystem.SaveBests(name, this);
        #region Separate Saves
        /*SaveTime time = SaveSystem.LoadTime(name);
        SaveKills kills = SaveSystem.LoadKills(name);


        if (kills != null)
        {
            if (kills.difficultyInt == difficultySetting)
            {
                if (kills.deadEnemies >= totalEnemiesKilled)
                {
                    bestEnemies.text = "BEST: " + kills.deadEnemies.ToString() + " / " + totalEnemies.ToString();
                    bestDifficulty.text = $"({SavedDifficulty(kills.difficultyInt)})";
                    saveKills = false;
                }
                else if (totalEnemiesKilled > kills.deadEnemies)
                {
                    bestEnemies.text = "NEW BEST: " + totalEnemiesKilled.ToString() + " / " + totalEnemies.ToString();
                    bestDifficulty.text = $"({difficulty})";
                    saveKills = true;
                }
            }
            else if (kills.difficultyInt < difficultySetting)
            {
                bestEnemies.text = "NEW BEST: " + totalEnemiesKilled.ToString() + " / " + totalEnemies.ToString();
                bestDifficulty.text = $"({difficulty})";
                saveKills = true;
            }
            else if (kills.difficultyInt > difficultySetting)
            {
                bestEnemies.text = "BEST: " + kills.deadEnemies.ToString() + " / " + totalEnemies.ToString();
                bestDifficulty.text = $"({SavedDifficulty(kills.difficultyInt)})";
                saveKills = false;
            }
        }
        else if (kills == null)
        {
            bestEnemies.text = "NEW BEST: " + totalEnemiesKilled.ToString() + " / " + totalEnemies.ToString();
            bestDifficulty.text = $"({difficulty})";

            saveKills = true;
        }

        if (time != null)
        {
            if (time.minutes <= this.minutes)
            {
                if (time.seconds <= timer)
                {
                    if (time.seconds < 10f)
                    {
                        bestTime.text = "BEST: " + $"{time.minutes}:0" + time.seconds.ToString("F3");
                    }
                    else if (time.seconds >= 10f)
                    {
                        bestTime.text = "BEST: " + $"{time.minutes}:" + time.seconds.ToString("F3");
                    }

                    saveTime = false;
                }
                else if (time.seconds > timer)
                {
                    if (timer < 10f)
                    {
                        bestTime.text = "NEW BEST: " + $"{this.minutes}:0" + timer.ToString("F3");
                    }
                    else if (time.seconds >= 10f)
                    {
                        bestTime.text = "NEW BEST: " + $"{this.minutes}:" + timer.ToString("F3");
                    }

                    saveTime = true;
                }
            }
        }
        else if (time == null)
        {
            if (timer < 10f)
            {
                bestTime.text = "NEW BEST: " + $"{this.minutes}:0" + timer.ToString("F3");
            }
            else if (timer >= 10f)
            {
                bestTime.text = "NEW BEST: " + $"{this.minutes}:" + timer.ToString("F3");
            }

            saveTime = true;
        }

        if (saveKills)
        {
            SaveSystem.SaveKills(name, this);
        }
        if (saveTime)
        {
            SaveSystem.SaveTime(name, this);
        }*/
        #endregion
    }

    string SavedDifficulty(int difficultyInt)
    {
        switch (difficultyInt)
        {
            case 1:
                newDifficulty = "CAKEWALK";
                break;
            case 2:
                newDifficulty = "EASY";
                break;
            case 3:
                newDifficulty = "NORMAL";
                break;
            case 4:
                newDifficulty = "HARD";
                break;
            case 5:
                newDifficulty = "INSANE";
                break;
        }

        return newDifficulty;
    }

    public void PlayerStartPos(GameObject thePlayer)
    {
        startPos = new Vector2(thePlayer.transform.position.x, thePlayer.transform.position.y);
    }

    void Timers()
    {
        timer += Time.deltaTime;
        gameTime += Time.deltaTime;
    }
}
