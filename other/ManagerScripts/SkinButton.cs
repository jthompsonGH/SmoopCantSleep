using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    public GameObject equipButton;
    public GameObject buyButton;
    public Text priceText;
    public MoneyUI moneyUI;
    public AudioSource sound;
    public AudioClip[] clips;
    public string key;
    public int skinNumber;
    public int cost;
    public int achiIndex;
    SaveMoney money;
    string username;

    private void Awake()
    {
        money = SaveSystem.LoadPlayer();
    }

    private void Start()
    {
        if (priceText)
        {
            priceText.text = cost.ToString();
        }
    }

    private void Update()
    {
        if (moneyUI.skinKeys[skinNumber] == key)
        {
            equipButton.SetActive(true);

            if (buyButton)
            {
                buyButton.SetActive(false);
            }
            if (priceText)
            {
                priceText.gameObject.SetActive(false);
            }
        }

        if (moneyUI.equippedSkin == skinNumber && equipButton.activeSelf)
        {
            Text equipText = equipButton.transform.GetChild(0).gameObject.GetComponent<Text>();
            equipText.text = "EQUIPPED";

            equipButton.GetComponent<Button>().interactable = false;
        }
        else if (moneyUI.equippedSkin != skinNumber && equipButton.activeSelf)
        {
            Text equipText = equipButton.transform.GetChild(0).gameObject.GetComponent<Text>();
            equipText.text = "EQUIP";

            equipButton.GetComponent<Button>().interactable = true;
        }
    }

    public void BuySkin()
    {
        if (moneyUI.currentGems >= cost)
        {
            moneyUI.LoseGems(cost);
            moneyUI.skinKeys[skinNumber] = key;


            buyButton.SetActive(false);
            priceText.gameObject.SetActive(false);
            equipButton.SetActive(true);

            if (sound)
            {
                sound.clip = clips[0];
                sound.Play();
            }

            EquipSkin();
        }
        else
        {
            moneyUI.LoseGems(0);

            if (sound)
            {
                sound.clip = clips[1];
                sound.Play();
            }
        }
    }

    public void EquipSkin()
    {
        moneyUI.equippedSkin = skinNumber;
        moneyUI.SaveGems();

        if (GameObject.Find("PlayerSpawner"))
        {
            GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>().ChangedSkins(skinNumber);
        }
    }
}
