using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseBroken : MonoBehaviour
{
    public CinemachineImpulseSource impulse;
    public GameObject gemDrop;
    public Transform gemPoint;
    float deletionTimer;

    private void Start()
    {
        Instantiate(gemDrop, gemPoint.position, Quaternion.identity);
        
        impulse.GenerateImpulse();
    }

    private void Update()
    {
        deletionTimer += Time.deltaTime;

        if (deletionTimer >= 5f)
        {
            Destroy(this.gameObject);
        }
    }
}
