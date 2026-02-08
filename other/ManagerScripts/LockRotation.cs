using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Quaternion xRotation;
    private void Start()
    {
        xRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = xRotation;
    }
}
