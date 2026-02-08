using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveKills
{
    public int deadEnemies;
    public int totalEnemies;
    public int difficultyInt;

    public SaveKills(Tracker tracker)
    {
        this.totalEnemies = tracker.totalEnemies;
        difficultyInt = tracker.difficultySetting;
        
        deadEnemies = tracker.totalEnemiesKilled;
    }
}
