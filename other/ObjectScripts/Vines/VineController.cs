using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject cullObject = GameObject.Find("CullingManager");
        
        if (cullObject)
        {
            CullingManager cull = cullObject.GetComponent<CullingManager>();
            
            if (cull)
            {
                cull.AddThis(this.gameObject);
            }
        }
    }
}
