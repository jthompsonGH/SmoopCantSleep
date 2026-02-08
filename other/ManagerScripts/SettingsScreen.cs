using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    public AudioMixer master;
    public Dropdown resolutionDropdown;
    public Toggle fullScreen;
    public Toggle fancyLighting;
    Resolution[] resolutions;
    List<int> middleMan = new List<int>();
    private void Start()
    {
        if (!Screen.fullScreen)
        {
            fullScreen.isOn = false;
        }

        if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
        {
            fancyLighting.isOn = false;
        }
        
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resString = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            if (!resString.Contains(resolutions[i].width + "x" + resolutions[i].height))
            {
                resString.Add(option);
                middleMan.Add(i);

                int matchCheck = CheckMiddleMan(i);
                int resolutionIndex = middleMan[matchCheck];

                if (resolutions[resolutionIndex].width == Screen.width && resolutions[resolutionIndex].height == Screen.height)
                {
                    currentResolutionIndex = matchCheck;
                }
            }
        }

        resolutionDropdown.AddOptions(resString);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        master.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("masterVolume", volume);

        if (volume == -50f)
        {
            master.SetFloat("MasterVolume", -80f);
        }
        else if (volume > -50f)
        {
            master.SetFloat("MasterVolume", volume);
        }
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

    public void IsFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void LightingOff(bool lighting)
    {
        int previous = PlayerPrefs.GetInt("fancyLighting", 10);
        int now = 11;
        
        if (lighting)
        {
            PlayerPrefs.SetInt("fancyLighting", 1);
            now = 1;
        }
        else if (!lighting)
        {
            PlayerPrefs.SetInt("fancyLighting", 0);
            now = 0;
        }

        if (previous != now)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().MusicFade();
            StartCoroutine(GameObject.Find("GameManager").GetComponent<GameManager>().FadeToNextScene(111));
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        int index = middleMan[resolutionIndex];
        
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    int CheckMiddleMan(int check)
    {
        return middleMan.IndexOf(check);
    }
}
