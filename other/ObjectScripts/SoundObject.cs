using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public List<AudioSource> sources;
    public List<AudioClip> clips;

    private void Awake()
    {
        foreach (AudioSource source in sources)
        {
            source.playOnAwake = false;
            source.Stop();
        }
    }
    public void PlaySound(int which)
    {
        if (sources[which].gameObject.GetComponent<SoundSelector>())
        {
            sources[which].gameObject.GetComponent<SoundSelector>().PlayThis();
        }
        else
        {
            sources[which].clip = clips[which];
            sources[which].Play();
        }
    }
}
