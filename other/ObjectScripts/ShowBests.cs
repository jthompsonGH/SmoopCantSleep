using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBests : MonoBehaviour
{
    public GameObject bestText;

    private void OnMouseOver()
    {
        if (!bestText.activeSelf)
        {
            bestText.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (bestText.activeSelf)
        {
            bestText.SetActive(false);
        }
    }
}
