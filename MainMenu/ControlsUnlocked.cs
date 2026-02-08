using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUnlocked : MonoBehaviour
{
    public string unlockType;
    public List<GameObject> objects;
    string username;

    private void Awake()
    {
        SaveMoney money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            switch (unlockType)
            {
                case "dash":
                    if (!money.dash)
                    {
                        foreach (GameObject thing in objects)
                        {
                            thing.SetActive(false);
                        }
                    }
                    break;
                case "melee":
                    if (!money.melee)
                    {
                        foreach (GameObject thing in objects)
                        {
                            thing.SetActive(false);
                        }
                    }
                    break;
            }
        }
        else if (money == null)
        {
            foreach (GameObject thing in objects)
            {
                thing.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
