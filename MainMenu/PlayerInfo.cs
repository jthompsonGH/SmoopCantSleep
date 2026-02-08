using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Text playerName;
    //public AvatarRawImage avatar;
    public RawImage rawImage;
    string username;

    private void Awake()
    {
        playerName.text = username;
    }
}
