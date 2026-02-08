using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButtons : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();
    int levels;
    string username;

    private void Awake()
    {
        SaveMoney money = SaveSystem.LoadPlayer();

        levels = money.maxLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (levels < 12)
        {
            foreach (GameObject wunga in buttons)
            {
                wunga.SetActive(false);
            }
        }
    }
}
