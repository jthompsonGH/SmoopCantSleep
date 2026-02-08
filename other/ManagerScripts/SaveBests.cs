using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveBests
{
    public int deadEnemies;
    public int totalEnemies;
    public int difficultyInt;
    public float seconds;
    public float minutes;
    public bool parBonus;
    public bool clearedBonus;
    public bool noDmgBonus;
    public bool noDeathBonus;

    public SaveBests(Tracker tracker)
    {
        this.totalEnemies = tracker.totalEnemies;
        difficultyInt = tracker.difficultySetting;

        deadEnemies = tracker.totalEnemiesKilled;

        seconds = tracker.timer;
        this.minutes = tracker.minutes;

        this.parBonus = tracker.parBonus;
        this.clearedBonus = tracker.clearedBonus;
        this.noDmgBonus = tracker.noDmgBonus;
        this.noDeathBonus = tracker.noDeathBonus;
    }
}