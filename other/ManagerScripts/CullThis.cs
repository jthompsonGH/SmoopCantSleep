using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullThis : MonoBehaviour
{
    public GameObject child;
    CullingManager cull;

    private void Awake()
    {
        if (GameObject.Find("CullingManager"))
        {
            if (GameObject.Find("CullingManager"))
            {
                cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
            }
        }

        if (cull)
        {
            cull.AddThis(child);
        }
    }
}
