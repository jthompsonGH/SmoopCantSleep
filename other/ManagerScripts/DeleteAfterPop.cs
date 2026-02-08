using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterPop : MonoBehaviour
{
    public float deletionTimer = 4f;

    private void Update()
    {
        deletionTimer -= Time.deltaTime;

        if (deletionTimer <= 0f)
        {
            DeleteThis();
        }
    }

    void DeleteThis()
    {
        Destroy(this.gameObject);
    }    
}
