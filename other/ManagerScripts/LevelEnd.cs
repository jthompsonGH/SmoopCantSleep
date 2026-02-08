using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LevelEnd : MonoBehaviour
{
    public GameObject[] trackerObjects;
    public GameObject[] bonusesFilled;
    public GameObject[] bonusesUnfilled;
    public GameObject[] moneyIcons;
    public GameObject endButtons;
    public GameObject endingText;
    public GameObject endingScreen;
    public Transform distanceCheck;
    public bool moneyHack;
    bool isShowing;
    string username;
    MoneyUI moneyUI;
    Tracker tracker;
    GameObject player;
    GameObject playerCanvas;
    MusicManager music;
    PlayerInput controls;
    Text levelEndText;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }
        
        if (GameObject.Find("MoneyTracker"))
        {
            moneyUI = GameObject.Find("MoneyTracker").GetComponent<MoneyUI>();
        }

        if (GameObject.Find("Tracker"))
        {
            tracker = GameObject.Find("Tracker").GetComponent<Tracker>();
        }

        music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
    }

    private void Start()
    {
        levelEndText = endingText.GetComponent<Text>();

        if (levelEndText)
        {
            if (SceneManager.GetActiveScene().name != "Level10")
            {
                levelEndText.text = $"{controls.actions["Interact"].GetBindingDisplayString()} to finish level";
            }
            else if (SceneManager.GetActiveScene().name == "Level10")
            {
                levelEndText.text = $"{controls.actions["Interact"].GetBindingDisplayString()} to finish Smoop Can't Sleep!";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            if (!playerCanvas)
            {
                playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
            }
            if (Vector2.Distance(player.transform.position, distanceCheck.transform.position) <= 2f)
            {
                if (!isShowing)
                {
                    isShowing = true;
                    endingText.SetActive(true);
                }
            }
            else
            {
                if (isShowing)
                {
                    isShowing = false;
                    endingText.SetActive(false);
                }
            }

            if (isShowing && !PauseMenu.paused && !PauseMenu.showingAbility && !PauseMenu.gameOver)
            {
                if (controls.actions["Interact"].triggered)
                {
                    Interacted();
                }
            }
        }
    }

    void Interacted()
    {
        if (!endingScreen.activeSelf)
        {
            NextLevelMenu();
        }
        endingText.SetActive(false);
    }
    
    void NextLevelMenu()
    {
        if (music)
        {
            music.LevelComplete();
        }
        
        bool savePar = false;
        bool saveClear = false;
        bool saveDmg = false;
        bool saveDeath = false;

        int difficultySelected = PlayerPrefs.GetInt("difficultySetting");

        playerCanvas.SetActive(false);

        foreach (GameObject thing in trackerObjects)
        {
            thing.SetActive(false);
        }

        PauseMenu.paused = true;
        PauseMenu.gameOver = true;
        Time.timeScale = 0f;

        GameObject clearedText = GameObject.Find("ClearedText");

        if (clearedText)
        {
            clearedText.SetActive(false);
        }

        Cursor.visible = true;

        GameObject.Find("GameManager").GetComponent<GameManager>().LevelWon();

        //MoneyManager.SetGems(moneyUI.currentGems);

        endingScreen.SetActive(true);

        string name = SceneManager.GetActiveScene().name;
        if (name != "RandomLevel")
        {
            SaveBests bonuses = SaveSystem.LoadBests(SceneManager.GetActiveScene().name);

            if (bonuses != null)
            {
                if (!GameManager.playerHit && !bonuses.noDmgBonus)
                {
                    tracker.noDmgBonus = true;

                    saveDmg = true;

                    bonusesUnfilled[0].SetActive(false);

                    bonusesFilled[0].SetActive(true);
                    moneyIcons[0].SetActive(true);

                    moneyUI.AddGems(5000);
                }
                else if (bonuses.noDmgBonus)
                {
                    saveDmg = false;

                    bonusesUnfilled[0].SetActive(false);

                    bonusesFilled[0].SetActive(true);
                }

                if (!GameManager.playerDied && !bonuses.noDeathBonus)
                {
                    tracker.noDeathBonus = true;

                    saveDeath = true;

                    bonusesUnfilled[1].SetActive(false);

                    bonusesFilled[1].SetActive(true);
                    moneyIcons[1].SetActive(true);

                    moneyUI.AddGems(3000);
                }
                else if (bonuses.noDeathBonus)
                {
                    saveDeath = false;

                    bonusesUnfilled[1].SetActive(false);

                    bonusesFilled[1].SetActive(true);
                }

                if (tracker.gameTime < tracker.parTime && !bonuses.parBonus)
                {
                    tracker.parBonus = true;

                    savePar = true;

                    bonusesUnfilled[2].SetActive(false);

                    bonusesFilled[2].SetActive(true);
                    moneyIcons[2].SetActive(true);

                    moneyUI.AddGems(2000);
                }
                else if (bonuses.parBonus)
                {
                    savePar = false;

                    bonusesUnfilled[2].SetActive(false);

                    bonusesFilled[2].SetActive(true);
                }

                if (tracker.clearedBonus && !bonuses.clearedBonus)
                {
                    saveClear = true;

                    bonusesUnfilled[3].SetActive(false);

                    bonusesFilled[3].SetActive(true);
                    moneyIcons[3].SetActive(true);

                    moneyUI.AddGems(2000);
                }
                else if (bonuses.clearedBonus)
                {
                    saveClear = false;

                    bonusesUnfilled[3].SetActive(false);

                    bonusesFilled[3].SetActive(true);
                }
            }
            else if (bonuses == null)
            {
                if (!GameManager.playerHit)
                {
                    tracker.noDmgBonus = true;
                    
                    saveDmg = true;

                    bonusesUnfilled[0].SetActive(false);

                    bonusesFilled[0].SetActive(true);
                    moneyIcons[0].SetActive(true);

                    moneyUI.AddGems(5000);
                }

                if (!GameManager.playerDied)
                {
                    tracker.noDeathBonus = true;

                    saveDeath = true;

                    bonusesUnfilled[1].SetActive(false);

                    bonusesFilled[1].SetActive(true);
                    moneyIcons[1].SetActive(true);

                    moneyUI.AddGems(3000);
                }

                if (tracker.gameTime < tracker.parTime)
                {
                    tracker.parBonus = true;

                    savePar = true;

                    bonusesUnfilled[2].SetActive(false);

                    bonusesFilled[2].SetActive(true);
                    moneyIcons[2].SetActive(true);

                    moneyUI.AddGems(2000);
                }

                if (tracker.clearedBonus)
                {
                    saveClear = true;

                    bonusesUnfilled[3].SetActive(false);

                    bonusesFilled[3].SetActive(true);
                    moneyIcons[3].SetActive(true);

                    moneyUI.AddGems(2000);
                }
            }
        }

        if (moneyHack)
        {
            moneyUI.AddGems(1000000);
        }
        
        tracker.LevelBeaten(name, savePar, saveClear, saveDmg, saveDeath);

        if (SceneManager.GetActiveScene().name == "Level10")
        {
            moneyUI.gameFinished = true;
        }
        
        moneyUI.SaveGems();
        
        endButtons.SetActive(true);
    }
}
