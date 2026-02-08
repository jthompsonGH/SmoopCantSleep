using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static bool doubleJumpUnlocked;
    public static bool meleeUnlocked;
    public static bool dashUnlocked;
    public static bool playerHit;
    public static bool playerDied;
    public GameObject playerCanvas;
    public GameObject[] trackerObjects;
    public GameObject overScreen;
    public Animator transition;
    public AudioMixer mixer;
    public AudioClip clip;
    public float transitionTime;
    public float restartDelay;
    AudioSource source;
    MoneyUI moneyUI;
    string username;
    int targetRate = 120;
    bool gameOver = false;

    public static void QuitGame()
    {
        //Steamworks.SteamAPI.Shutdown();
        Application.Quit();
    }
    
    private void Awake()
    {
        if (GameObject.Find("MoneyTracker"))
        {
            moneyUI = GameObject.Find("MoneyTracker").GetComponent<MoneyUI>();
        }
        
        SaveMoney money = SaveSystem.LoadPlayer();

        int lights = PlayerPrefs.GetInt("fancyLighting", 1);

        if (lights == 0)
        {
            if (GameObject.Find("Global Lights"))
            {
                GameObject.Find("Global Lights").GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().intensity = 0.8f;
            }
        }

        source = GetComponent<AudioSource>();
        if (source)
        {
            source.clip = clip;
        }

        playerHit = false;
        playerDied = false;

        PauseMenu.paused = false;
        PauseMenu.gameOver = false;
        PauseMenu.showingAbility = false;

        if (money != null)
        {
            doubleJumpUnlocked = money.doubleJump;
            meleeUnlocked = money.melee;
            dashUnlocked = money.dash;
        }
        else if (money == null)
        {
            doubleJumpUnlocked = false;
            meleeUnlocked = false;
            dashUnlocked = false;
        }

        DamageUI.poweredUP = false;

        //float volume = PlayerPrefs.GetFloat("masterVolume");
        float mVolume = PlayerPrefs.GetFloat("musicVolume");
        float sVolume = PlayerPrefs.GetFloat("SFXVolume");
        
        mixer.SetFloat("MasterVolume", 5);
        mixer.SetFloat("MusicVolume", mVolume);
        mixer.SetFloat("SFXVolume", sVolume);
    }

    private void Start()
    {
        KillManager.dorpsKilled = 0;
        KillManager.knightsKilled = 0;
        KillManager.swarmersKilled = 0;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetRate;
    }

    public void GameEnded()
    {
        if (!gameOver)
        {
            gameOver = true;
            Invoke("Restart", restartDelay);
        }
    }

    public void MusicFade()
    {
        GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
        MusicManager music;

        if (musicObject)
        {
            music = musicObject.GetComponent<MusicManager>();

            if (music)
            {
                StartCoroutine(music.FadeToNext());
            }
        }
    }

    void Restart()
    {
        foreach (GameObject thing in trackerObjects)
        {
            thing.SetActive(false);
        }

        PauseMenu.paused = true;
        PauseMenu.gameOver = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        overScreen.SetActive(true);
    }

    public void NextLevel(int level)
    {
        MusicFade();
        StartCoroutine(FadeToNextScene(level));

        PauseMenu.paused = false;
        PauseMenu.gameOver = false;
    }

    public void LevelWon()
    {
        SaveMoney save = SaveSystem.LoadPlayer();

        if (save != null)
        {
            if (save.maxLevel <= SceneManager.GetActiveScene().buildIndex)
            {
                moneyUI.maxLevel = SceneManager.GetActiveScene().buildIndex + 1;

                SaveSystem.SavePlayer(moneyUI);
            }
        }
        else if (save == null)
        {
            moneyUI.maxLevel = SceneManager.GetActiveScene().buildIndex + 1;

            SaveSystem.SavePlayer(moneyUI);
        }

        string currentLevel = SceneManager.GetActiveScene().name;
    }

    public IEnumerator FadeToNextScene(int levelIndex, bool deleteMusic = true)
    {
        CameraZoom.levelCleared = false;
        
        transition.SetTrigger("play");

        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < transitionTime)
        {
            yield return null;
        }

        if (deleteMusic)
        {
            if (GameObject.Find("MusicObject"))
            {
                Destroy(GameObject.Find("MusicObject"));
            }

            if (GameObject.FindGameObjectWithTag("Player"))
            {
                Destroy(GameObject.FindGameObjectWithTag("Player"));
            }
        }
        
        PauseMenu.paused = false;
        PauseMenu.gameOver = false;
        
        Time.timeScale = 1f;
        
        switch (levelIndex)
        {
            case 100:
                SceneManager.LoadScene("IntroText");
                break;
            case 101:
                SceneManager.LoadScene("LoadingText1");
                break;
            case 102:
                SceneManager.LoadScene("LoadingText2");
                break;
            case 103:
                SceneManager.LoadScene("LoadingText3");
                break;
            case 104:
                SceneManager.LoadScene("Credits");
                break;
            case 105:
                SceneManager.LoadScene("EpilogueText");
                break;
            case 111:
                string thisScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(thisScene);
                break;
            default:
                SceneManager.LoadScene(levelIndex);
                break;
        }
    }

    public void CreditButton()
    {
        StartCoroutine(FadeToNextScene(1));
    }

    public void MenuSelect()
    {
        source.Play();
    }
}
