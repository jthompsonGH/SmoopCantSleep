using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmOnSmoop : MonoBehaviour
{
    public GameObject bullet;
    public GameObject muzzleFlash;
    public Transform flashPoint;

    private void Start()
    {
        Instantiate(muzzleFlash, flashPoint.position, flashPoint.rotation);
        Instantiate(bullet, flashPoint.position, flashPoint.rotation);
    }
}
