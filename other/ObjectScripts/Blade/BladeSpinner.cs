using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpinner : MonoBehaviour
{
    public bool canSpin;
    public bool canSlice;
    public float spinSpeed;
    Vector2 rbposition;

    private void Awake()
    {
        GameObject cullingMan = GameObject.Find("CullingManager");

        if (cullingMan)
        {
            CullingManager cull = cullingMan.GetComponent<CullingManager>();

            if (cull)
            {
                cull.AddThis(this.gameObject);
            }
        }
    }

    private void Start()
    {
        rbposition = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {
        if (canSpin)
        {
            Spin();
        }
    }

    void Spin()
    {
        transform.Rotate(new Vector3(0f, 0f, -spinSpeed) * Time.deltaTime, Space.World);
    }

    public void NotWork()
    {
        canSpin = false;
        canSlice = false;
    }
}
