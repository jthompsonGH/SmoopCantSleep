using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public static bool justReset;
    public GameObject playButton;
    public GameObject startButton;
    public GameObject resetSave;
    public GameObject goldText;
    public GameObject goldSmoop;
    public GameObject fireworks;
    public GameObject resetText;
    public GameManager gameManager;
    public AudioMixer mixer;
    public Slider[] volumeSliders;
    string username;
    MoneyUI moneyUI;
    
    private void Awake()
    {
        if (justReset)
        {
            resetText.SetActive(true);

            StartCoroutine(DeleteText());

            justReset = false;
        }
        
        if (SaveSystem.LoadPlayer() == null)
        {
            resetSave.SetActive(false);
        }
        
        moneyUI = GameObject.Find("MoneyTracker").GetComponent<MoneyUI>();
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("masterVolume", 5);
        float mVolume = PlayerPrefs.GetFloat("musicVolume", -5);
        float sVolume = PlayerPrefs.GetFloat("SFXVolume", -5);
        
        
        mixer.SetFloat("MasterVolume", 5);
        mixer.SetFloat("MusicVolume", mVolume);
        mixer.SetFloat("SFXVolume", sVolume);

        for (int i = 0; i < volumeSliders.Length; i++)
        {
            switch (i)
            {
                case 0:
                    volumeSliders[i].value = 5;
                    break;
                case 1:
                    volumeSliders[i].value = mVolume;
                    break;
                case 2:
                    volumeSliders[i].value = sVolume;
                    break;
            }
        }

        SaveMoney save = SaveSystem.LoadPlayer();
        
        if (save != null)
        {
            CheckCompletion();
        }
        else if (save == null)
        {
            playButton.SetActive(false);
            startButton.SetActive(true);
        }

        /*if (PlayerPrefs.GetInt("levelReached") == 3)
        {
            playButton.SetActive(false);
            startButton.SetActive(true);
        }*/
    }

    private void Update()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }

    public void PlayGame(int difficulty)
    {
        PlayerPrefs.SetInt("difficultySetting", difficulty);

        /*GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Destroy(player);
        }*/

        if (gameManager)
        {
            gameManager.MusicFade();
            StartCoroutine(gameManager.FadeToNextScene(100));
        }
        else
        {
            SceneManager.LoadScene("IntroText");
        }
    }

    public void QuitGame()
    {
        GameManager.QuitGame();
    }

    public void LevelSelect()
    {
        if (gameManager)
        {
            StartCoroutine(gameManager.FadeToNextScene(2, false));
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void DoIt(int wungaBunga)
    {
        if (wungaBunga == 1097)
        {
            DeleteSave(true, true, true, true);
        }
    }

    public void CorrectButton()
    {
        SaveMoney money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            if (money.maxLevel > 3)
            {
                startButton.SetActive(false);
            }
        }
        else if (money == null)
        {
            playButton.SetActive(false);
            startButton.SetActive(true);
        }
    }

    void CheckCompletion()
    {
        int completion = 0;

        SaveMoney money = SaveSystem.LoadPlayer();

        string path = Path.Combine(Application.persistentDataPath, "Saves", "LevelBests", "Level10.smoop");
        
        if (money.skinKeys[8] != "URD4G0DD")
        {
            if (File.Exists(path))
            {
                for (int i = 1; i < 11; i++)
                {
                    string level;

                    if (i < 10)
                    {
                        level = $"Level{i}";
                    }
                    else
                    {
                        level = "Level10";
                    }

                    SaveBests best = SaveSystem.LoadBests(level);

                    if (best != null)
                    {
                       if (best.noDeathBonus)
                        {
                            completion++;
                        }
                        if (best.noDmgBonus)
                        {
                            completion++;
                        }
                        if (best.parBonus)
                        {
                            completion++;
                        }
                        if (best.clearedBonus)
                        {
                            completion++;
                        }
                    }
                }

                if (completion >= 36)
                {
                    moneyUI.skinKeys[8] = "URD4G0DD";

                    SaveSystem.SavePlayer(moneyUI);

                    if (goldSmoop)
                    {
                        goldSmoop.SetActive(true);
                    }

                    if (goldText)
                    {
                        goldText.SetActive(true);
                    }

                    if (fireworks)
                    {
                        fireworks.SetActive(true);
                    }
                }
            }
        }
        else if (money.skinKeys[8] == "URD4G0DD")
        {
            if (goldSmoop)
            {
                goldSmoop.SetActive(true);
            }
        }
    }

    IEnumerator DeleteText()
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < 3f)
        {
            yield return null;
        }

        resetText.SetActive(false);
    }

    void DeleteSave(bool ok, bool sure, bool yes, bool goodbye)
    {
        if (ok)
        {
            if (sure)
            {
                if (yes)
                {
                    if (goodbye)
                    {
                        string path = Path.Combine(Application.persistentDataPath, "Saves");

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
    }
}
