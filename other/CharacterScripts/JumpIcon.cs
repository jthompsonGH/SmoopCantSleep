using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpIcon : MonoBehaviour
{
    public Image jumpIcon;
    public Image jumpUnfilled;
    bool isEnabled;
    public PlayerMovement playerMove;


    private void Update()
    {
        if (GameManager.doubleJumpUnlocked)
        {
            if (!isEnabled)
            {
                isEnabled = true;
                jumpIcon.enabled = true;
                jumpUnfilled.enabled = true;
            }
            
            if (isEnabled)
            {
                if (playerMove.multiJump == 0f)
                {
                    if (jumpIcon.fillAmount < 1f)
                    {
                        jumpIcon.fillAmount += Time.deltaTime * 9;
                    }
                    else if (jumpIcon.fillAmount > 1f)
                    {
                        jumpIcon.fillAmount = 1f;
                    }
                }
                else
                {
                    jumpIcon.fillAmount = 0f;
                }
            }
        }
        else
        {
            if (!jumpIcon.enabled == false)
            {
                isEnabled = false;
                jumpIcon.enabled = false;
                jumpUnfilled.enabled = false;
            }
        }
    }
}
