using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTracker : MonoBehaviour
{
    public int numberOfLives;
    public int startingLives = 3;

    private void Awake()
    {
        numberOfLives = startingLives;
    }

    public void DecreaseCount()
    {
        numberOfLives -= 1;
    }
}
