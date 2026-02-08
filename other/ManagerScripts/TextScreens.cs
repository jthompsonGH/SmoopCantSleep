using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextScreens : MonoBehaviour
{
    public int levelNumber;
    public float waitTime;
    public GameObject pressAny;
    public GameObject skipText;
    public GameObject[] texts;
    public GameManager manager;
    int currentIndex = 1;
    float timer;
    string username;

    private void Awake()
    {
        SaveMoney money = SaveSystem.LoadPlayer();
        
        if (money.maxLevel > levelNumber)
        {
            SceneManager.LoadScene(levelNumber);
        }
    }

    private void Start()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (skipText.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(levelNumber);
        }
        
        if (timer >= waitTime && currentIndex < texts.Length)
        {
            ShowNext();
        }
        else if (timer >= waitTime && currentIndex == texts.Length)
        {
            if (!pressAny.activeSelf)
            {
                PressAny();
            }
        }

        if (pressAny.activeSelf && Input.anyKeyDown)
        {
            PlayNextLevel(levelNumber);
        }
    }

    void ShowNext()
    {
        timer = 0f;

        texts[currentIndex].SetActive(true);

        currentIndex++;
    }

    void PressAny()
    {
        pressAny.SetActive(true);
    }

    void PlayNextLevel(int levelNumber)
    {
        manager.MusicFade();
        StartCoroutine(manager.FadeToNextScene(levelNumber));
    }
}
