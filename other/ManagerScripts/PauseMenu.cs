using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public static bool gameOver = false;
    public static bool showingAbility = false;
    public GameObject playerCanvas;
    public Text currentDifficulty;
    public GameObject pauseMenu;
    public GameObject wonMenu;
    public GameManager manager;
    public GameObject pauseMain;
    public GameObject pauseControls;
    public GameObject[] pauseSures;
    GameObject player;
    //string username;

    private void Awake()
    {
        //username = AchievementManager.ReturnUser().DisplayName;
    }

    private void Start()
    {
        switch (PlayerPrefs.GetInt("difficultySetting"))
        {
            case 1:
                currentDifficulty.text = "CAKEWALK";
                break;
            case 2:
                currentDifficulty.text = "EASY";
                break;
            case 3:
                currentDifficulty.text = "NORMAL";
                break;
            case 4:
                currentDifficulty.text = "HARD";
                break;
            case 5:
                currentDifficulty.text = "INSANE";
                break;
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
        }

        if (!gameOver && !showingAbility)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    if (!pauseControls.activeSelf)
                    {
                        Resume();
                    }
                }
                else
                {
                    Pause();
                    Cursor.visible = true;
                }
            }
        }

        if (paused)
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }
        else
        {
            if (!showingAbility)
            {
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        pauseMain.SetActive(true);
        pauseControls.SetActive(false);

        foreach (GameObject menu in pauseSures)
        {
            menu.SetActive(false);
        }

        if (playerCanvas)
        {
            if (!playerCanvas.activeSelf)
            {
                playerCanvas.SetActive(true);
                Cursor.visible = false;
            }
        }

        paused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        if (playerCanvas)
        {
            if (playerCanvas.activeSelf)
            {
                playerCanvas.SetActive(false);
                Cursor.visible = true;
            }
        }

        paused = true;
    }

    public void Quit()
    {
        GameManager.QuitGame();
    }

    public void MainMenu()
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(1));
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .01f;
        //KillManager.dorpsKilled = 0;
        //KillManager.swarmersKilled = 0;
        //KillManager.knightsKilled = 0;
        Cursor.visible = false;
        CameraZoom.levelCleared = false;
        paused = false;
        gameOver = false;

        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(111));
    }

    /*public static void DeleteSave(bool ok, bool sure, bool yes, bool goodbye)
    {
        if (ok)
        {
            if (sure)
            {
                if (yes)
                {
                    if (goodbye)
                    {
                        string path = Application.persistentDataPath + $"/{username}/";

                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                            MainMenuManager.justReset = true;
                            SceneManager.LoadScene(1);
                        }
                    }
                }
            }
        }
    }*/
}
