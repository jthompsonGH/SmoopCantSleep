using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject pieces;
    public bool isDestructible = true;

    public void Shatter()
    {
        if (isDestructible)
        {
            GetComponent<EdgeCollider2D>().enabled = false;
            Instantiate(pieces, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
