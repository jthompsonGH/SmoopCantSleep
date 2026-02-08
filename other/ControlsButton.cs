using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsButton : MonoBehaviour
{
    public GameObject controls;
    public GameObject player;
    public GameObject controlText;
    bool inRange;
    PlayerInput input;
    Text enableText;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Controls"))
        {
            input = GameObject.FindGameObjectWithTag("Controls").GetComponent<PlayerInput>();
        }

        if (controlText)
        {
            enableText = controlText.GetComponent<Text>();
        }
    }

    private void Start()
    {
        if (input)
        {
            enableText.text = $"{input.actions["Interact"].GetBindingDisplayString()} to view controls";
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player)
        {
            if (Vector3.Distance(player.transform.position, controlText.transform.position) < 2f)
            {
                if (!controlText.activeSelf)
                {
                    controlText.SetActive(true);
                    inRange = true;
                }
            }
            else
            {
                if (controlText.activeSelf)
                {
                    controlText.SetActive(false);
                    inRange = false;
                }
            }

            if (input.actions["Interact"].triggered)
            {
                Interacted();
            }

        }
    }

    void Interacted()
    {
        if (inRange && controlText.activeSelf)
        {
            if (!controls.activeSelf)
            {
                controls.SetActive(true);
            }
            else if (controls.activeSelf)
            {
                controls.SetActive(false);
            }
        }
    }
}
