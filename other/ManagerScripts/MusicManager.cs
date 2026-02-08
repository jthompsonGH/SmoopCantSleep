using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource lvlComplete;
    public AudioClip music;
    public AudioClip lvlCompleteMusic;


    public bool coffin;
    public bool mainMenu = false;
    public bool levelSelect = false;
   

     private void Awake()
    {
        backgroundMusic.clip = music;

        if (lvlComplete)
        {
            lvlComplete.clip = lvlCompleteMusic;
        }
    }

    private void Start()
    {
        if (!coffin)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void LevelComplete()
    {
        lvlComplete.Play();

        StartCoroutine(FastFade());
    }

    public void Coffin()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FastFade()
    {
        backgroundMusic.volume = 0f;

        yield return new WaitForSecondsRealtime(5);

        StartCoroutine(FadeBack());
    }

    IEnumerator FadeBack()
    {
        while (backgroundMusic.volume < 1f)
        {
            backgroundMusic.volume += Time.unscaledDeltaTime;

            yield return null;
        }

        backgroundMusic.volume = 1f;
    }

    IEnumerator FadeIn()
    {
        backgroundMusic.Play();
        
        while (backgroundMusic.volume < 1f)
        {
            backgroundMusic.volume += Time.unscaledDeltaTime;

            yield return null;
        }

        backgroundMusic.volume = 1f;
    }

    public IEnumerator FadeToNext()
    {
        while (backgroundMusic.volume > 0f)
        {
            if (!mainMenu)
            {
                backgroundMusic.volume -= Time.unscaledDeltaTime;

                if (lvlComplete)
                {
                    lvlComplete.volume = 0f;
                }
            }

            yield return null;
        }
        
        if (!mainMenu)
        {
            backgroundMusic.volume = 0f;

            if (lvlComplete)
            {
                lvlComplete.volume = 0f;
            }
        }

    }
}
