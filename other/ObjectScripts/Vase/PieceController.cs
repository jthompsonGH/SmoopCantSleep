using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public bool isCoffinPiece = false;
    public float X1;
    public float X2;
    public float Y1;
    public float Y2;
    float velocityX;
    float velocityY;

    private void Awake()
    {
        velocityX = Random.Range(X1, X2 + 1f);
        velocityY = Random.Range(Y1, Y2 + 1f);

        if (isCoffinPiece)
        {
            if (Mathf.Abs(velocityX) < 3)
            {
                if (velocityX < 0f)
                {
                    velocityX -= 3f;
                }
                else if (velocityX > 0f)
                {
                    velocityX += 3f;
                }
            }
        }
        else if (!isCoffinPiece)
        {
            if (Mathf.Abs(velocityX) < 1)
            {
                if (velocityX < 0f)
                {
                    velocityX--;
                }
                else if (velocityX > 0f)
                {
                    velocityX++;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(0.5f, 1.5f));
    }
}
