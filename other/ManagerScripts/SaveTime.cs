using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveTime
{
    public float seconds;
    public float minutes;

    public SaveTime(Tracker tracker)
    {
        seconds = tracker.timer;
        this.minutes = tracker.minutes;
    }
}