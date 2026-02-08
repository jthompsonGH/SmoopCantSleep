using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontChangeXScale : MonoBehaviour
{
    Vector3 theScale;

    // Start is called before the first frame update
    void Start()
    {
        theScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        theScale.x = 1;
    }
}
