using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuickButton : MonoBehaviour
{
    public string buttonType;
    public bool gameOverScreen;
    public bool isTipButton;
    public bool abilityResume = false;
    UnityEngine.UI.Button thisButton;
    KeyCode key;
    KeyCode key2;

    private void Start()
    {
        thisButton = GetComponent<UnityEngine.UI.Button>();


        switch (buttonType)
        {
            case "resume":
                key = KeyCode.E;
                break;
            case "continue":
                key = KeyCode.C;
                if (gameOverScreen)
                {
                    key2 = KeyCode.E;
                }
                break;
            case "restart":
                key = KeyCode.R;
                if (gameOverScreen)
                {
                    key2 = KeyCode.E;
                }
                break;
            case "mainmenu":
                key = KeyCode.M;
                break;
            case "quit":
                key = KeyCode.Q;
                break;
            case "yesmenu":
                key = KeyCode.Y;
                break;
            case "nomenu":
                key = KeyCode.N;
                break;
            case "rightarrow":
                key = KeyCode.RightArrow;
                break;
            case "leftarrow":
                key = KeyCode.LeftArrow;
                break;
            case "backbutton":
                key = KeyCode.Escape;
                break;
            case "fix":
                key = KeyCode.Return;
                break;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            StartCoroutine(ClickedThis());
        }
        
        if (abilityResume)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(ClickedThis());
            }
        }
    }

    IEnumerator ClickedThis()
    {
        float startTime = Time.realtimeSinceStartup;

        thisButton.Select();

        while (Time.realtimeSinceStartup - startTime < .1f)
        {
            yield return null;
        }

        BaseEventData data = new BaseEventData(EventSystem.current);
        ExecuteEvents.Execute(thisButton.gameObject, data, ExecuteEvents.submitHandler);

        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}
