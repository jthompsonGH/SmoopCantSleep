using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject firstScreen;
    public GameObject secondScreen;
    public GameObject selectText;
    public GameObject skipText;
    public GameObject pressAny;
    public GameManager manager;
    public float speed;
    public GameObject[] texts1;
    public GameObject[] texts2;
    public MoneyUI moneyUI;
    public bool isIntro = true;
    public bool credits = false;
    public bool epilogue = false;
    bool isSecond;
    int maxLevel;
    int nextText;
    float timer;
    string username;
    SaveMoney money;

    private void Awake()
    {
        money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            maxLevel = money.maxLevel;
        }
        else if (money == null)
        {
            maxLevel = 3;
        }

        timer = 0f;
        nextText = 1;
        isSecond = false;

        if (isIntro)
        {
            if (maxLevel != 3)
            {
                selectText.SetActive(true);
                skipText.SetActive(false);
            }
        }
        else if (!isIntro && !credits)
        {
            if (money.gameFinished == true)
            {
                skipText.SetActive(true);
            }
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isIntro)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && skipText.activeSelf)
            {
                if (manager)
                {
                    manager.MusicFade();
                    StartCoroutine(manager.FadeToNextScene(3));
                }
                else
                {
                    SceneManager.LoadScene(3);
                }
            }
        }
        else if (!isIntro && !credits)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && skipText.activeSelf)
            {
                if (manager)
                {
                    manager.MusicFade();
                    StartCoroutine(manager.FadeToNextScene(15));
                }
                else
                {
                    SceneManager.LoadScene(15);
                }
            }
        }
        else if (!isIntro && credits)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && skipText.activeSelf)
            {
                if (manager)
                {
                    manager.MusicFade();
                    StartCoroutine(manager.FadeToNextScene(1));
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
        
        if (selectText != null)
        {
            if (selectText.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Cursor.visible = true;

                    if (manager)
                    {
                        manager.MusicFade();
                        StartCoroutine(manager.FadeToNextScene(2));
                    }
                    else
                    {
                        SceneManager.LoadScene(2);
                    }
                }
            }
        }

        if (!isSecond && nextText != 4)
        {
            if (timer >= speed)
            {
                timer = 0f;
                SetNext(nextText, false);
            }
        }
        else if (isSecond && nextText != 4)
        {
            if (timer >= speed - 2)
            {
                timer = 0f;
                SetNext(nextText, true);
            }
        }

        if (!isSecond)
        {
            if (timer >= speed && nextText == 4)
            {
                pressAny.SetActive(true);
            }
        }
        else if (isSecond)
        {
            if (timer >= speed - 2 && nextText == 4)
            {
                pressAny.SetActive(true);
            }
        }

        if (pressAny != null)
        {
            if (pressAny.activeSelf)
            {
                if (!pressAny.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("FadeOut"))
                {
                    if (!isSecond)
                    {
                        if (Input.anyKeyDown)
                        {
                            pressAny.GetComponent<Animator>().SetTrigger("FadeOut");

                            foreach (GameObject text in texts1)
                            {
                                text.GetComponent<Animator>().SetTrigger("FadeOut");
                            }

                            Invoke("ChangeScreens", 1.5f);
                        }
                    }
                    else if (isSecond)
                    {
                        if (Input.anyKeyDown)
                        {
                            pressAny.GetComponent<Animator>().SetTrigger("FadeOut");
                            manager.MusicFade();

                            if (epilogue)
                            {
                                manager.MusicFade();
                                StartCoroutine(manager.FadeToNextScene(15));
                            }
                            else
                            {
                                if (maxLevel == 3)
                                {
                                    manager.MusicFade();
                                    StartCoroutine(manager.FadeToNextScene(3));
                                }
                                else if (maxLevel != 3)
                                {
                                    Cursor.visible = true;

                                    manager.MusicFade();
                                    StartCoroutine(manager.FadeToNextScene(2));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void SetNext(int next, bool second)
    {
        if (!second)
        {
            texts1[next].SetActive(true);
        }
        else if (second)
        {
            texts2[next].SetActive(true);
        }
        nextText++;
    }

    void ChangeScreens()
    {
        pressAny.SetActive(false);

        isSecond = true;

        timer = 0;
        
        firstScreen.SetActive(false);
        secondScreen.SetActive(true);

        nextText = 1;
    }
}
