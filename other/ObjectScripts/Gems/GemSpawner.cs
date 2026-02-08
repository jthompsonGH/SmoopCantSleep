using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    public GameObject[] gems;
    float despawnTimer;
    int difficultyOdds;
    int blueOdds;
    int greenOdds;
    int redOdds = 100;

    private void Awake()
    {
        difficultyOdds = PlayerPrefs.GetInt("difficultySetting", 3);

        switch (difficultyOdds)
        {
            case 1:
                blueOdds = 90;
                greenOdds = 99;
                break;
            case 2:
                blueOdds = 80;
                greenOdds = 95;
                break;
            case 3:
                blueOdds = 60;
                greenOdds = 90;
                break;
            case 4:
                blueOdds = 40;
                greenOdds = 80;
                break;
            case 5:
                blueOdds = 20;
                greenOdds = 70;
                break;
        }
    }

    private void Start()
    {
        int gemDropped = Random.Range(1, 101);

        if (gemDropped <= blueOdds)
        {
            Instantiate(gems[0], transform.position, Quaternion.identity);
        }
        else if (gemDropped > blueOdds && gemDropped <= greenOdds)
        {
            Instantiate(gems[1], transform.position, Quaternion.identity);
        }
        else if (gemDropped > greenOdds && gemDropped <= redOdds)
        {
            Instantiate(gems[2], transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        despawnTimer += Time.deltaTime;

        if (despawnTimer >= .5f)
        {
            Destroy(this.gameObject);
        }
    }
}
