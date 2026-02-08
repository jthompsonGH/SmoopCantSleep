using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUppable : MonoBehaviour
{
    public GameObject pop;
    public GameObject lifePickup;
    public string pickupType;
    public float floatForce = .1f;
    public float floatSpeed = 3f;
    PlayerController controller;
    PlayerMovement movement;
    GameObject player;
    Transform playerPos;
    CullingManager cull;
    Vector2 floatY;
    RespawnTracker respawner;
    DamageUI damage;
    int healAmount;
    bool added = false;
    float originalY;

    private void Awake()
    {
        cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();

        if (cull)
        {
            cull.AddThis(this.gameObject);
        }

        switch (PlayerPrefs.GetInt("difficultySetting"))
        {
            case 1:
                healAmount = 5;
                break;
            case 2:
                healAmount = 4;
                break;
            case 3:
                healAmount = 3;
                break;
            case 4:
                healAmount = 2;
                break;
        }
    }

    private void Start()
    {
        respawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnTracker>();

        if (GameObject.Find("BallUp"))
        {
            damage = GameObject.Find("BallUp").GetComponent<DamageUI>();
        }

        switch (pickupType)
        {
            case "health":
                if (PlayerPrefs.GetInt("difficultySetting") == 5)
                {
                    Instantiate(lifePickup, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
                break;
        }

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
            controller = player.GetComponent<PlayerController>();
            movement = player.GetComponent<PlayerMovement>();

            if (Vector2.Distance(playerPos.position, transform.position) <= 0.5f)
            {
                if (!added)
                {
                    switch (pickupType)
                    {
                        case "damage":
                            DamageUp();
                            break;
                        case "life":
                            LifeUp();
                            break;
                        case "health":
                            HealthUp();
                            break;
                        case "jump":
                            if (GameManager.doubleJumpUnlocked)
                            {
                                JumpUp();
                            }
                            break;
                    }
                }

            }
        }
    }

    void Despawn()
    {
        Instantiate(pop, transform.position, Quaternion.identity);
        
        if (cull)
        {
            cull.RemoveThis(this.gameObject);

        }
        
        Destroy(this.gameObject);
    }

    void DamageUp()
    {
        damage.PickedUp();
        added = true;
        Despawn();
    }

    void LifeUp()
    {
        if (respawner.numberOfLives < respawner.startingLives)
        {
            controller.AddLife();
            added = true;
            Despawn();
        }
    }

    void HealthUp()
    {
        if (controller.health < controller.maxHealth)
        {
            controller.Heals(healAmount);
            added = true;
            Despawn();
        }
    }

    void JumpUp()
    {
        movement.multiJump = 0;
        added = true;
        Despawn();
    }
}
