using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CullingManager : MonoBehaviour
{
    public List<GameObject> gameObjects;
    List<GameObject> despawns;
    GameObject player;
    Transform playerPos;

    private void Awake()
    {
        despawns = new List<GameObject>();

        if (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else if (GameObject.FindGameObjectWithTag("Head"))
            {
                player = GameObject.FindGameObjectWithTag("Head");
            }
        }
    }
    private void Start()
    {
        if (player)
        {
            DoCull();
        }
    }

    private void Update()
    {
        despawns = new List<GameObject>();

        if (player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
                DoCull();
            }
            else if (GameObject.FindGameObjectWithTag("Head"))
            {
                player = GameObject.FindGameObjectWithTag("Head");
                DoCull();
            }
        }
    }

    public void AddThis(GameObject insert)
    {
        gameObjects.Add(insert);
    }

    public void RemoveThis(GameObject insert)
    {
        gameObjects.Remove(insert);
    }

    public void ClearRemaining()
    {
        foreach (GameObject useless in despawns)
        {
            if (useless.GetComponent<ExploderController>())
            {
                useless.GetComponent<ExploderController>().HarmlessExplode();
            }
            else if (useless.GetComponent<CasterController>())
            {
                useless.GetComponent<CasterController>().Die();
            }
            else if (useless.GetComponent<CasterLeftController>())
            {
                useless.GetComponent<CasterLeftController>().Die();
            }
        }
    }

    IEnumerator Cull()
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < 1f)
        {
            yield return null;
        }

        if (player)
        {
            playerPos = player.transform;

            if (gameObjects != null)
            {
                foreach (GameObject thing in gameObjects)
                {
                    if (thing != null)
                    {
                        switch (thing.tag)
                        {
                            case "Swarmer":
                                if (Vector2.Distance(playerPos.position, thing.transform.position) > 30f)
                                {
                                    SwarmerController swarmer = thing.GetComponent<SwarmerController>();
                                    ExploderController exploder = thing.GetComponent<ExploderController>();


                                    if (swarmer)
                                    {
                                        if (!swarmer.playerDetected)
                                        {
                                            if (thing.activeSelf)
                                            {
                                                thing.SetActive(false);
                                            }
                                        }
                                    }
                                    else if (exploder)
                                    {
                                        if (!exploder.playerDetected)
                                        {
                                            if (thing.activeSelf)
                                            {
                                                thing.SetActive(false);
                                            }
                                        }
                                    }
                                }
                                else if (Vector2.Distance(playerPos.position, thing.transform.position) <= 30f)
                                {
                                    if (!thing.activeSelf)
                                    {
                                        thing.SetActive(true);
                                    }
                                }
                                break;
                            default:
                                if (Vector2.Distance(playerPos.position, thing.transform.position) > 30f)
                                {
                                    if (thing.activeSelf)
                                    {
                                        thing.SetActive(false);
                                    }
                                }
                                else if (Vector2.Distance(playerPos.position, thing.transform.position) <= 30f)
                                {
                                    if (!thing.activeSelf)
                                    {
                                        thing.SetActive(true);
                                    }
                                }
                                break;
                        }

                        if (thing.GetComponent<ExploderController>() || thing.GetComponent<CasterController>() || thing.GetComponent<CasterLeftController>())
                        {
                            despawns.Add(thing);
                        }
                    }
                }
            }

            yield return StartCoroutine(Cull());
        }
    }

    void DoCull()
    {
        if (player)
        {
            playerPos = player.transform;

            if (gameObjects != null)
            {
                foreach (GameObject thing in gameObjects)
                {
                    if (thing != null)
                    {
                        switch (thing.tag)
                        {
                            case "Swarmer":
                                if (Vector2.Distance(playerPos.position, thing.transform.position) > 30f)
                                {
                                    SwarmerController swarmer = thing.GetComponent<SwarmerController>();
                                    ExploderController exploder = thing.GetComponent<ExploderController>();


                                    if (swarmer)
                                    {
                                        if (!swarmer.playerDetected)
                                        {
                                            if (thing.activeSelf)
                                            {
                                                thing.SetActive(false);
                                            }
                                        }
                                    }
                                    else if (exploder)
                                    {
                                        if (!exploder.playerDetected)
                                        {
                                            if (thing.activeSelf)
                                            {
                                                thing.SetActive(false);
                                            }
                                        }
                                    }
                                }
                                else if (Vector2.Distance(playerPos.position, thing.transform.position) <= 30f)
                                {
                                    if (!thing.activeSelf)
                                    {
                                        thing.SetActive(true);
                                    }
                                }
                                break;
                            default:
                                if (Vector2.Distance(playerPos.position, thing.transform.position) > 30f)
                                {
                                    if (thing.activeSelf)
                                    {
                                        thing.SetActive(false);
                                    }
                                }
                                else if (Vector2.Distance(playerPos.position, thing.transform.position) <= 30f)
                                {
                                    if (!thing.activeSelf)
                                    {
                                        thing.SetActive(true);
                                    }
                                }
                                break;
                        }

                        if (thing.GetComponent<ExploderController>() || thing.GetComponent<CasterController>() || thing.GetComponent<CasterLeftController>())
                        {
                            despawns.Add(thing);
                        }
                    }
                }
            }

            if (player)
            {
                StartCoroutine(Cull());
            }
        }
    }
}
