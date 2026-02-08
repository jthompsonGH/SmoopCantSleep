using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AbilityUp : MonoBehaviour
{
    public GameObject pop;
    public GameObject barrier;
    public GameObject vCam;
    public string ability;
    public float floatForce = .1f;
    public float floatSpeed = 3f;
    GameObject player;
    Transform playerPos;
    CullingManager cull;
    Vector2 floatY;
    bool added = false;
    float originalY;
    string username;

    private void Awake()
    {
        cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();

        if (cull)
        {
            cull.AddThis(this.gameObject);
        }

        SaveMoney money = SaveSystem.LoadPlayer();
        
        switch (ability)
        {
            case "dash":
                if (money.dash)
                {
                    Destroy(barrier);
                    Destroy(this.gameObject);
                }
                break;
            case "doubleJump":
                if (money.doubleJump)
                {
                    Destroy(barrier);
                    Destroy(this.gameObject);
                }
                break;
            case "melee":
                if (money.melee)
                {
                    Destroy(barrier);
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time * floatSpeed) * floatForce);
        transform.position = floatY;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            playerPos = player.transform;

            if (Vector2.Distance(playerPos.position, transform.position) <= 0.5f)
            {
                if (!added)
                {
                    added = true;

                    switch (ability)
                    {
                        case "dash":
                            vCam.GetComponent<CameraZoom>().UnlockedAnAbility(ability);
                            break;
                        case "doubleJump":
                            vCam.GetComponent<CameraZoom>().UnlockedAnAbility(ability);
                            break;
                        case "melee":
                            vCam.GetComponent<CameraZoom>().UnlockedAnAbility(ability);
                            break;
                    }

                    Despawn();
                }
            }
        }
    }

    void Despawn()
    {
        if (pop)
        {
            Instantiate(pop, transform.position, Quaternion.identity);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(transform.GetChild(0).gameObject);
        }

        if (cull)
        {
            cull.RemoveThis(this.gameObject);

        }

        CameraZoom.abilityUnlock = true;

        Destroy(barrier);
    }

    public void PickedUp(string input)
    {
        switch (input)
        {
            case "dash":
                GameManager.dashUnlocked = true;
                GameObject.Find("MoneyTracker").GetComponent<MoneyUI>().dash = true;
                DashMove dash = player.GetComponent<DashMove>();
                dash.enabled = true;
                dash.SetUIActive();
                break;
            case "doubleJump":
                GameManager.doubleJumpUnlocked = true;
                GameObject.Find("MoneyTracker").GetComponent<MoneyUI>().doubleJump = true;
                break;
            case "melee":
                GameManager.meleeUnlocked = true;
                player.GetComponent<PlayerController>().LifeBarFix();
                player.GetComponent<PlayerController>().meleeReady = 1f;
                GameObject.Find("MoneyTracker").GetComponent<MoneyUI>().melee = true;
                break;
        }

        Destroy(this.gameObject);
    }
}
