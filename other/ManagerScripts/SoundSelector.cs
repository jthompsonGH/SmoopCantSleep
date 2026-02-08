using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSelector : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource source;
    int soundPlayed;
    
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        
        if (source.playOnAwake)
        {
            PlayThis();
        }
    }

    public void PlayThis()
    {
        soundPlayed = Random.Range(0, audioClips.Count);

        source.clip = audioClips[soundPlayed];

        source.Play();
    }
}
