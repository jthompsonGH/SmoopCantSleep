using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicObject : MonoBehaviour
{

    MusicManager music;
    
    private void Awake()
    {
        music = GetComponent<MusicManager>();
        
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (music.mainMenu)
            {
                music.mainMenu = false;
                music.levelSelect = true;
            }
            else if (!music.mainMenu)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    music.mainMenu = true;
                    music.levelSelect = false;
                }
            }
        }
    }
}
