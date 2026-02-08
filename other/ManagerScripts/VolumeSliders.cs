using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    public AudioMixer master;
    public Slider music;
    public Slider sfx;
    
    private void Start()
    {
        float mVolume = PlayerPrefs.GetFloat("musicVolume", -5);
        float sVolume = PlayerPrefs.GetFloat("SFXVolume", -5);


        master.SetFloat("MusicVolume", mVolume);
        master.SetFloat("SFXVolume", sVolume);

        music.value = mVolume;
        sfx.value = sVolume;
    }


   public void SetMusicVolume(float volume)
    {
        master.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);

        if (volume == -50f)
        {
            master.SetFloat("MusicVolume", -80f);
        }
        else if (volume > -50f)
        {
            master.SetFloat("MusicVolume", volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        master.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        if (volume == -50f)
        {
            master.SetFloat("SFXVolume", -80f);
        }
        else if (volume > -50f)
        {
            master.SetFloat("SFXVolume", volume);
        }
    }
}
