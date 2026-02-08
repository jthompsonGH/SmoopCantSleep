using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject deathPop;
    public Material redFlash;
    Material matDefault;
    
    // Start is called before the first frame update
    void Start()
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        
        matDefault = sprite.material;
    }

    // Update is called once per frame
    public void HurtFlash()
    {
        sprite.material = redFlash;
        Invoke("ReturnToNormal", .07f);
        Invoke("SecondFlash", .2f);

    }

    void ReturnToNormal()
    {
        sprite.material = matDefault;
    }

    public void DeathPop()
    {
        Instantiate(deathPop, transform.position, transform.rotation);
    }

    void SecondFlash()
    {
        sprite.material = redFlash;
        Invoke("ReturnToNormal", .07f);
    }

}
