using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnlockUI : MonoBehaviour
{
    public GameObject[] subs;
    public Image healthBar;
    public Image lifeBar;
    public Image meleeBar;
    int currentSub = 0;
    CameraZoom cameraZoom;
    [SerializeField] private string unlockType;
    [SerializeField] private Text textToShowControls;
    PlayerInput input;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            input = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        if (input)
        {
            switch (unlockType)
            {
                case "melee":
                    textToShowControls.text = $"PRESS {input.actions["Melee"].GetBindingDisplayString()} TO RELEASE A DEVASTATING BLAST";
                    break;
                case "dash":
                    textToShowControls.text = $"{input.actions["Dash"].GetBindingDisplayString()} TO DASH, {input.actions["Ground Pound"].GetBindingDisplayString()} TO GROUND POUND";
                    break;
            }
        }
    }

    private void Start()
    {
        if (GameObject.Find("vCam"))
        {
            cameraZoom = GameObject.Find("vCam").GetComponent<CameraZoom>();
        }
        
        StartCoroutine(ShowSubs());

        GameObject playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");

        if (playerCanvas)
        {
            playerCanvas.SetActive(false);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cameraZoom)
            {
                cameraZoom.ClickedTheButton();
            }
        }
    }

    IEnumerator ShowSubs()
    {
        float startTime = Time.realtimeSinceStartup;

        if (currentSub < subs.Length - 1)
        {
            while (Time.realtimeSinceStartup - startTime < 1f)
            {
                yield return null;
            }
        }
        else if (currentSub >= subs.Length - 1)
        {
            while (Time.realtimeSinceStartup - startTime < 1f)
            {
                yield return null;
            }
        }

        if (currentSub < subs.Length - 1)
        {
            subs[currentSub].SetActive(true);
            currentSub++;
        }
        else if (currentSub == subs.Length - 1)
        {
            subs[currentSub].SetActive(true);
            currentSub++;
            Cursor.visible = true;
        }
        else if (currentSub == subs.Length)
        {
            this.enabled = false;
        }

        yield return StartCoroutine(ShowSubs());
    }
}
