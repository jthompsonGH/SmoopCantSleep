using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Pathfinding;
using UnityEngine;

public class SwarmerPathing : MonoBehaviour
{
    public List<Transform> introPos = new List<Transform>();
    public List<Transform> targetPos = new List<Transform>();
    public Transform currentTarget;
    public bool enablePathing;
    public bool useIntro;
    public int whichPos;
    SwarmerController thisBug;
    ExploderController thisBoom;
    AIDestinationSetter pather;
    Transform currentIntroTarget;
    float timer;
    float nextTimer;
    bool goingBack;
    int currentIntroPos;

    private void Awake()
    {
        if (!enablePathing)
        {
            this.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        goingBack = false;
        nextTimer = 0f;

        if (!enablePathing)
        {
            this.enabled = false;
        }

        pather = GetComponent<AIDestinationSetter>();

        thisBoom = GetComponent<ExploderController>();
        thisBug = GetComponent<SwarmerController>();

        whichPos = 0;
        currentIntroPos = 0;

        if (useIntro)
        {
            currentIntroTarget = introPos[currentIntroPos];
            pather.target = introPos[currentIntroPos];
        }
        if (!useIntro)
        {
            currentTarget = targetPos[whichPos];
            pather.target = targetPos[whichPos];
        }
    }

    // Update is called once per frame
    void Update()
    {
        nextTimer += Time.deltaTime;
        
        if (useIntro)
        {
            if (thisBoom)
            {
                if (!thisBoom.playerDetected)
                {
                    if ((Vector2.Distance(transform.position, currentIntroTarget.position) < 0.4) || nextTimer >= 5f)
                    {
                        if (currentIntroPos < introPos.Count - 1)
                        {
                            currentIntroPos++;

                            ChangeTarget();
                        }
                    }

                }
                /*else if (thisBoom.playerDetected)
                {
                    this.enabled = false;
                }*/
            }
            else if (thisBug)
            {
                if (!thisBug.playerDetected)
                {
                    if ((Vector2.Distance(transform.position, currentIntroTarget.position) < 0.4) || nextTimer >= 5f)
                    {
                        if (currentIntroPos < introPos.Count - 1)
                        {
                            currentIntroPos++;

                            ChangeTarget();
                        }
                    }

                }
                /*else if (thisBug.playerDetected)
                {
                    this.enabled = false;
                }*/
            }

            if (Vector2.Distance(transform.position, currentIntroTarget.position) < 0.4 && currentIntroPos >= introPos.Count - 1)
            {
                useIntro = false;

                ChangeTarget();
            }
        }
        else if (!useIntro)
        {
            if (thisBoom)
            {
                if (!thisBoom.playerDetected)
                {
                    if ((Vector2.Distance(transform.position, currentTarget.position) < 0.4) || nextTimer >= 5f)
                    {
                        if (whichPos < targetPos.Count - 1 && whichPos > 0)
                        {
                            if (!goingBack)
                            {
                                whichPos++;
                            }
                            else if (goingBack)
                            {
                                whichPos--;
                            }

                            ChangeTarget();
                        }
                    }

                }
                /*else if (thisBoom.playerDetected)
                {
                    this.enabled = false;
                }*/
            }
            else if (thisBug)
            {
                if (!thisBug.playerDetected)
                {
                    if ((Vector2.Distance(transform.position, currentTarget.position) < 0.4) || nextTimer >= 5f)
                    {
                        if (whichPos < targetPos.Count - 1 && whichPos > 0)
                        {
                            if (!goingBack)
                            {
                                whichPos++;
                            }
                            else if (goingBack)
                            {
                                whichPos--;
                            }

                            ChangeTarget();
                        }
                    }

                }
                /*else if (thisBug.playerDetected)
                {
                    this.enabled = false;
                }*/
            }

            if (Vector2.Distance(transform.position, currentTarget.position) < 0.4 && whichPos >= targetPos.Count - 1)
            {
                timer += Time.deltaTime;

                if (timer >= 1f)
                {
                    timer = 0f;

                    goingBack = true;

                    whichPos--;

                    ChangeTarget();
                }
            }
            else if (Vector2.Distance(transform.position, currentTarget.position) < 0.4 && whichPos == 0)
            {
                timer += Time.deltaTime;

                if (timer >= 1f)
                {
                    timer = 0f;

                    goingBack = false;

                    whichPos++;

                    ChangeTarget();
                }
            }
        }
    }

    void ChangeTarget()
    {
        nextTimer = 0f;
        
        if (useIntro)
        {
            currentIntroTarget = introPos[currentIntroPos];
            pather.target = introPos[currentIntroPos];
        }
        else if (!useIntro)
        {
            currentTarget = targetPos[whichPos];
            pather.target = targetPos[whichPos];
        }
    }

    public void SetSavedTarget()
    {
        PlayerDied();
    }

    void PlayerDied()
    {
        if (targetPos.Count < 2)
        {
            this.enabled = false;
        }

        whichPos = 0;
        currentTarget = targetPos[whichPos];
        pather.target = targetPos[whichPos];
    }
}
