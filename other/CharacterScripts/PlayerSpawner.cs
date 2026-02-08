using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    CinemachineVirtualCamera vCam;
    string username;
    int chosenSkin;

    private void Awake()
    {
        if (GameObject.Find("vCam"))
        {
            vCam = GameObject.Find("vCam").GetComponent<CinemachineVirtualCamera>();
        }
        
        if (vCam)
        {
            vCam.m_Follow = this.gameObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveMoney money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            chosenSkin = money.equippedSkin;
        }
        else
        {
            chosenSkin = 0;
        }

        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            if (money != null)
            {
                switch (chosenSkin)
                {
                    case 0:
                        SpawnDefault();
                        break;
                    case 1:
                        if (money.skinKeys[chosenSkin] == "e1BRA098")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 2:
                        if (money.skinKeys[chosenSkin] == "M2BRM199")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 3:
                        if (money.skinKeys[chosenSkin] == "Y32KB213")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 4:
                        if (money.skinKeys[chosenSkin] == "CMN1013A")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 5:
                        if (money.skinKeys[chosenSkin] == "BRK4411L")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 6:
                        if (money.skinKeys[chosenSkin] == "D01NK3RZ")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 7:
                        if (money.skinKeys[chosenSkin] == "SM009FLY")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                    case 8:
                        if (money.skinKeys[chosenSkin] == "URD4G0DD")
                        {
                            SpawnSkin(chosenSkin);
                        }
                        else
                        {
                            SpawnDefault();
                        }
                        break;
                }
            }
            else if (money == null)
            {
                SpawnDefault();
            }
        }
    }

    void SpawnDefault()
    {
        Instantiate(prefabs[0], transform.position, Quaternion.identity);
    }

    void SpawnSkin(int skin)
    {
        Instantiate(prefabs[skin], transform.position, Quaternion.identity);
    }

    public void ChangedSkins(int skin)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player)
        {
            Destroy(player);

            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y + .2f);

            Instantiate(prefabs[skin], playerPos, Quaternion.identity);
        }
        else
        {
            Instantiate(prefabs[skin], transform.position, Quaternion.identity);
        }
    }
}
