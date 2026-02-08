using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectBests : MonoBehaviour
{
    public Text enemiesText;
    public Text diffText;
    public Text timeText;
    public Text parTime;
    public GameObject[] filledBonuses;
    public string[] parTimeList;
    string difficulty;
    string username;

    public void SetText(string name)
    {
        SaveBests bests = SaveSystem.LoadBests(name);

        if (bests != null)
        {
            enemiesText.text = bests.deadEnemies.ToString() + " / " + bests.totalEnemies.ToString();
            diffText.text = $"{SavedDifficulty(bests.difficultyInt)}";

            if (bests.seconds < 10f)
            {
                timeText.text = $"{bests.minutes}:0" + bests.seconds.ToString("F3");
            }
            else if (bests.seconds >= 10f)
            {
                timeText.text = $"{bests.minutes}:" + bests.seconds.ToString("F3");
            }

            foreach (GameObject bonus in filledBonuses)
            {
                bonus.SetActive(false);
            }
            
            for (int i = 0; i < filledBonuses.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (bests.noDmgBonus)
                        {
                            filledBonuses[i].SetActive(true);
                        }
                        break;
                    case 1:
                        if (bests.noDeathBonus)
                        {
                            filledBonuses[i].SetActive(true);
                        }
                        break;
                    case 2:
                        if (bests.parBonus)
                        {
                            filledBonuses[i].SetActive(true);
                        }
                        break;
                    case 3:
                        if (bests.clearedBonus)
                        {
                            filledBonuses[i].SetActive(true);
                        }
                        break;
                }
            }

            switch (name)
            {
                case "Level1":
                    parTime.text = $"PAR: {parTimeList[0]}";
                    break;
                case "Level2":
                    parTime.text = $"PAR: {parTimeList[1]}";
                    break;
                case "Level3":
                    parTime.text = $"PAR: {parTimeList[2]}";
                    break;
                case "Level4":
                    parTime.text = $"PAR: {parTimeList[3]}";
                    break;
                case "Level5":
                    parTime.text = $"PAR: {parTimeList[4]}";
                    break;
                case "Level6":
                    parTime.text = $"PAR: {parTimeList[5]}";
                    break;
                case "Level7":
                    parTime.text = $"PAR: {parTimeList[6]}";
                    break;
                case "Level8":
                    parTime.text = $"PAR: {parTimeList[7]}";
                    break;
                case "Level9":
                    parTime.text = $"PAR: {parTimeList[8]}";
                    break;
                case "Level10":
                    parTime.text = $"PAR: {parTimeList[9]}";
                    break;
            }
        }
        if (bests == null)
        {
            this.gameObject.SetActive(false);
        }
        #region Separate Saves
        /*SaveTime time = SaveSystem.LoadTime(name);
        SaveKills kills = SaveSystem.LoadKills(name);

        if (kills != null)
        {
            enemiesText.text = kills.deadEnemies.ToString() + " / " + kills.totalEnemies.ToString();
            diffText.text = $"({SavedDifficulty(kills.difficultyInt)})";

        }
        if (time != null)
        {
            timeText.text = $"{time.minutes}:0" + time.seconds.ToString("F3");
        }
        
        if (time == null || kills == null)
        {
            this.gameObject.SetActive(false);
        }*/
        #endregion
    }

    string SavedDifficulty(int difficultyInt)
    {
        switch (difficultyInt)
        {
            case 1:
                difficulty = "CAKEWALK";
                break;
            case 2:
                difficulty = "EASY";
                break;
            case 3:
                difficulty = "NORMAL";
                break;
            case 4:
                difficulty = "HARD";
                break;
            case 5:
                difficulty = "INSANE";
                break;
        }

        return difficulty;
    }
}
