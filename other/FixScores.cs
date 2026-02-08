using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixScores : MonoBehaviour
{
    public GameObject[] leaderboards;
    GameObject[] scoreObjects;

    int minutes;
    float seconds;

    private void Update()
    {
        scoreObjects = GameObject.FindGameObjectsWithTag("Score");

        foreach (GameObject score in scoreObjects)
        {
            Text scoreText = score.GetComponent<Text>();

            if (!scoreText.text.Contains(":"))
            {
                float thisSeconds = float.Parse(scoreText.text, System.Globalization.NumberStyles.Float);

                scoreText.text = TimeFormat(thisSeconds);
            }
        }
    }
    /*IEnumerator ScoreText()
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < 1f)
        {
            scoreObjects = GameObject.FindGameObjectsWithTag("Score");

            foreach (GameObject score in scoreObjects)
            {
                Text scoreText = score.GetComponent<Text>();

                if (!scoreText.text.Contains(":"))
                {
                    float thisSeconds = float.Parse(scoreText.text, System.Globalization.NumberStyles.Float);

                    scoreText.text = TimeFormat(thisSeconds);
                }
            }

            yield return null;
        }
    }*/

    string TimeFormat(float time)
    {
        string answer;
        
        minutes = 0;
        seconds = 0;
        
        minutes = Mathf.FloorToInt(time / 60000f);

        if (minutes > 0)
        {
            float thisTime = time;
            thisTime -= (minutes * 60000);

            seconds = thisTime / 1000f;
        }
        else if (minutes == 0)
        {
            seconds = time / 1000f;
        }

        if (seconds >= 10)
        {
            answer = $"{minutes}:" + seconds.ToString("F3");
            return answer;
        }
        else if (seconds < 10)
        {
            answer = $"{minutes}:0" + seconds.ToString("F3");
            return answer;
        }

        return null;
    }

    public void ShowLeaderboard(int boardIndex)
    {
        string boardName = leaderboards[boardIndex].name;

        foreach (GameObject leader in leaderboards)
        {
            if (leader.name != boardName)
            {
                leader.SetActive(false);
            }
            else if (leader.name == boardName)
            {
                leader.SetActive(true);
            }
        }
    }
}
