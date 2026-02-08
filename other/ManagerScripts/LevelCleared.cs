using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCleared : MonoBehaviour
{

    public ParticleSystem[] particles;
    public AudioClip firework;
    public bool mainMenu = false;
    AudioSource pop;
    bool ending = false;
    float timer;
    int thisIndex;
    int lastIndex;

    private void Start()
    {
        pop = GetComponent<AudioSource>();
        pop.clip = firework;
        StartCoroutine(Fireworks());

        timer = 0f;
    }

    private void Update()
    {
        if (!mainMenu)
        {
            if (!CameraZoom.levelCleared)
            {
                if (!ending)
                {
                    EndThis();
                }
            }
        }
        else if (mainMenu)
        {
            timer += Time.deltaTime;
            
            if (timer >= 6f)
            {
                if (!ending)
                {
                    EndThis();
                }
            }
        }
    }

    void EndThis()
    {
        ending = true;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

    IEnumerator Fireworks()
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < Random.Range(1f, 2f))
        {
            yield return null;
        }

        if (!ending)
        {
            thisIndex = Random.Range(0, particles.Length - 1);

            if (thisIndex == lastIndex)
            {
                float random = Random.Range(-1f, 1f);

                if (random < 0f)
                {
                    thisIndex--;
                }
                else if (random >= 0f)
                {
                    thisIndex++;
                }
            }

            if (thisIndex < 0 || thisIndex > particles.Length - 1)
            {
                thisIndex = Random.Range(0, particles.Length - 1);
            }

            lastIndex = thisIndex;

            particles[thisIndex].Play();
            pop.Play();

            yield return StartCoroutine(Fireworks());
        }
    }
}
