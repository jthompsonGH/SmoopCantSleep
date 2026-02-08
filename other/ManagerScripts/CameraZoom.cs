using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public static bool levelCleared;
    public static bool abilityUnlock;
    public PauseMenu pauseMenu;
    public GameObject clearedText;
    public GameObject abilityUI;
    public AbilityUp abilityPickup;
    public float zoomSpeed;
    public float zoomTarget;
    string ability;
    float baseSpeed;
    float target;
    bool zoomedIn = true;
    CinemachineVirtualCamera vCam;
    PlayerInput controls;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            controls = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        levelCleared = false;

        if (SceneManager.GetActiveScene().buildIndex != 3)
        {
            Bars.brokeOut = true;
        }

        baseSpeed = zoomSpeed;

        vCam = GetComponent<CinemachineVirtualCamera>();

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            vCam.m_Lens.OrthographicSize = 3;
            target = 3;
        }
        else
        {
            vCam.m_Lens.OrthographicSize = 6;
            target = 6;
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") || GameObject.FindGameObjectWithTag("Head"))
        {
            if (!PauseMenu.paused)
            {
                if (vCam)
                {
                    if (!levelCleared)
                    {
                        if (!Bars.brokeOut)
                        {
                            target = 3;
                        }
                    }

                    /*if (vCam.m_Lens.OrthographicSize > target)
                    {
                        ZoomIn();
                    }
                    else if (vCam.m_Lens.OrthographicSize < target)
                    {
                        ZoomOut();
                    }*/

                    if (levelCleared)
                    {
                        if (!clearedText.activeSelf)
                        {
                            clearedText.SetActive(true);
                        }

                        if (Time.timeScale > .3f)
                        {
                            Time.timeScale -= Time.unscaledDeltaTime;
                            Time.fixedDeltaTime = Time.timeScale * .01f;
                        }
                        else if (Time.timeScale < .3f)
                        {
                            Time.timeScale = .3f;
                            Time.fixedDeltaTime = Time.timeScale * .01f;
                        }
                    }
                    else if (!levelCleared && !abilityUnlock)
                    {
                        if (Time.timeScale < 1f)
                        {
                            Time.timeScale += Time.unscaledDeltaTime;
                            Time.fixedDeltaTime = Time.timeScale * .01f;
                        }
                        else if (Time.timeScale > 1f)
                        {
                            Time.timeScale = 1f;
                            Time.fixedDeltaTime = Time.timeScale * .01f;
                        }
                    }

                    if (abilityUnlock)
                    {
                        if (Time.timeScale >= Time.unscaledDeltaTime / 2f)
                        {
                            Time.timeScale -= Time.unscaledDeltaTime / 2f;
                        }
                        else if (Time.timeScale < Time.unscaledDeltaTime / 2f)
                        {
                            Time.timeScale = 0f;
                        }

                        Time.fixedDeltaTime = Time.timeScale * .01f;
                    }

                    if (abilityUnlock && Time.timeScale <= 0f)
                    {
                        if (!abilityUI.activeSelf)
                        {
                            PauseMenu.showingAbility = true;
                            abilityUI.SetActive(true);
                        }
                    }

                    if (controls.actions["Zoom"].triggered)
                    {
                        ToggleZoom();
                    }
                    
                    DoZoom();
                }
            }
        }
    }

    /*void ZoomIn()
    {
        vCam.m_Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
    }

    void ZoomOut()
    {
        vCam.m_Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
    }*/

    void ToggleZoom()
    {
        Debug.LogError("zoomToggled");
        
        if (zoomedIn)
        {
            zoomedIn = false;
        }
        else if (!zoomedIn)
        {
            zoomedIn = true;
        }
    }
    
    void DoZoom()
    {
        if (!levelCleared)
        {
            if (zoomedIn)
            {
                target = 6;
            }
            else if (!zoomedIn)
            {
                target = zoomTarget;
            }
        }
        else if (levelCleared)
        {
            target = 3.5f;
        }

        if (vCam.m_Lens.OrthographicSize > target)
        {
            //ZoomIn();
            vCam.m_Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
        }
        else if (vCam.m_Lens.OrthographicSize < target)
        {
            //ZoomOut();
            vCam.m_Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
        }

        if (Mathf.Abs(target - vCam.m_Lens.OrthographicSize) < .1f)
        {
            vCam.m_Lens.OrthographicSize = target;
        }
    }
    

    /*if (vCam.m_Lens.OrthographicSize > target)
    {
        //ZoomIn();
        vCam.m_Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
    }
    else if (vCam.m_Lens.OrthographicSize < target)
    {
        //ZoomOut();
        vCam.m_Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
    }*/

    public IEnumerator ClearedCam()
    {
        levelCleared = true;

        float startTime = Time.realtimeSinceStartup;

        zoomSpeed -= 2f;
        target = 3.5f;

        while (Time.realtimeSinceStartup - startTime < 6f)
        {
            yield return null;
        }

        zoomSpeed = baseSpeed;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .01f;

        levelCleared = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().SlowMoGone();
    }

    public void UnlockedAnAbility(string unlocked)
    {
        ability = unlocked;

        abilityUnlock = true;
    }

    public void ClearUI(string toUnlock)
    {
        abilityUnlock = false;
        PauseMenu.showingAbility = false;

        abilityUI.SetActive(false);

        GameObject playerCanvas = pauseMenu.playerCanvas;

        if (playerCanvas)
        {
            playerCanvas.SetActive(true);
        }

        abilityPickup.PickedUp(toUnlock);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .01f;
    }

    public void ClickedTheButton()
    {
        ClearUI(ability);
    }

    public void ChangeTarget()
    {
        target = 6;
        zoomedIn = true;
    }
}
