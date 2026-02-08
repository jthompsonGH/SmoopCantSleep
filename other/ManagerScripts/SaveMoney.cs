using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveMoney
{
    public int gems;
    public int maxLevel;
    public int equippedSkin;
    public string[] skinKeys;
    public bool doubleJump;
    public bool dash;
    public bool melee;
    public bool gameFinished;

    public SaveMoney(MoneyUI money)
    {
        this.gems = money.currentGems;
        this.skinKeys = money.skinKeys;

        this.doubleJump = money.doubleJump;
        this.dash = money.dash;
        this.melee = money.melee;

        this.maxLevel = money.maxLevel;
        this.equippedSkin = money.equippedSkin;
        this.gameFinished = money.gameFinished;
    }
}
