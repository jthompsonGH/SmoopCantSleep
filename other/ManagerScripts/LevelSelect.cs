using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LevelSelect : MonoBehaviour
{
    public Button[] levelButtons;
    public Toggle[] diffToggles;
    GameManager manager;
    int setDifficulty;
    int levelReached;
    string username;

    private void Awake()
    {
        //float totalMinutes = 0f;
        //float totalSeconds = 0f;
        //float totalTime = 0f;
        //int difTotal = 0;
        
        setDifficulty = PlayerPrefs.GetInt("difficultySetting", 3);
        diffToggles[setDifficulty - 1].isOn = true;

        if (GameObject.Find("GameManager"))
        {
            manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        SaveMoney save = SaveSystem.LoadPlayer();
    }

    private void Start()
    {
        SaveMoney money = SaveSystem.LoadPlayer();
        
        if (money != null)
        {
            levelReached = money.maxLevel;
        }
        else if (money == null)
        {
            levelReached = 3;
        }
        
        for (int i = 0; i < levelButtons.Length; i++)
        {

            if (i + 3 > levelReached)
            {
                levelButtons[i].interactable = false;
                levelButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void MainMenu()
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(1, false));
    }

    public void Select(int levelIndex)
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(levelIndex));
    }

    public void IntroText()
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(100));
    }

    public void Credits()
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(104));
    }

    public void Epilogue()
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(105));
    }
    #region Difficulties
    public void SetVEasy(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("difficultySetting", 1);
        }
    }

    public void SetEasy(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("difficultySetting", 2);
        }
    }

    public void SetNormal(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("difficultySetting", 3);
        }
    }

    public void SetHard(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("difficultySetting", 4);
        }
    }

    public void SetVHard(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("difficultySetting", 5);
        }
    }
    #endregion
}
