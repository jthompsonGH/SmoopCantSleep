using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightDim : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D _Light;
    public float dimSpeed;
    public float waitTime;
    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            _Light.intensity -= Time.deltaTime * dimSpeed;

            if (_Light.intensity <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
