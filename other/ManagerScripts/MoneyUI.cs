using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text gemText;
    public GameObject purse;
    public Material redFlash;
    public int currentGems;
    public int maxLevel;
    public int equippedSkin;
    public string[] skinKeys;
    public bool doubleJump = false;
    public bool dash = false;
    public bool melee = false;
    public bool gameFinished;
    Material purseMat;
    string username;
    //public int gemsPickedUp = 0;
    //int startGems;

    private void Awake()
    {
        skinKeys = new string[9];

        SaveMoney money = SaveSystem.LoadPlayer();

        if (money != null)
        {
            for (int i = 0; i < skinKeys.Length; i++)
            {
                skinKeys[i] = money.skinKeys[i];
            }

            gemText.text = money.gems.ToString();
            currentGems = money.gems;

            this.doubleJump = money.doubleJump;
            this.dash = money.dash;
            this.melee = money.melee;

            this.maxLevel = money.maxLevel;
            this.equippedSkin = money.equippedSkin;
            this.gameFinished = money.gameFinished;

        }
        else
        {
            skinKeys[0] = "000Smoop";
            gemText.text = "0";
            currentGems = 0;
        }

        purseMat = purse.GetComponent<Image>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gemText.text = currentGems.ToString();
    }

    public void AddGems(int gemCount)
    {
        currentGems += gemCount;
        purse.GetComponent<Animator>().SetTrigger("gemsGrabbed");
    }

    public void LoseGems(int gemCount)
    {
        currentGems -= gemCount;

        if (currentGems < 0)
        {
            currentGems = 0;
        }

        purse.GetComponent<Animator>().SetTrigger("gemsGrabbed");
        Flashing();
        
    }

    public void SaveGems()
    {
        SaveSystem.SavePlayer(this);
    }

    void Flashing()
    {
        purse.GetComponent<Image>().material = redFlash;
        Invoke("Regular", .2f);
    }

    void Regular()
    {
        purse.GetComponent<Image>().material = purseMat;
    }

}
