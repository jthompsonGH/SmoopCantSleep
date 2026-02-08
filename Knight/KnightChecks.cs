using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightChecks : MonoBehaviour
{
    public KnightController brain;
    public LayerMask layerMask;
    public Rigidbody2D rb;
    public bool movable;

    private void Update()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 1f, layerMask);
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, -transform.right, .4f, layerMask);

        if (!groundCheck || wallCheck)
        {
            brain.Flip();
            brain.canMove = true;
            brain.playerDetected = false;
        }
    }
}
