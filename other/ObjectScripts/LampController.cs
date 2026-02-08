using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public class LampController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D _Light;
    public float minTime;
    public float maxTime;
    public float changeStrength = 1f;
    CullingManager cull;
    float Timer;
    float intense;
    float intensityChange;

    private void Awake()
    {
        if (GameObject.Find("CullingManager"))
        {
            cull = GameObject.Find("CullingManager").GetComponent<CullingManager>();
        }

        intense = _Light.intensity;
        
        if (cull)
        {
            cull.AddThis(this.gameObject);
        }

        if (PlayerPrefs.GetInt("fancyLighting", 1) == 0)
        {
            _Light.enabled = false;
            this.enabled = false;
        }
    }

    private void Start()
    {
        Timer = Random.Range(minTime, maxTime);
        intensityChange = Random.Range(0.85f, 0.9f) * changeStrength;
    }

    private void Update()
    {
        Flicker();
    }

    void Flicker()
    {
        if (Timer > 0f)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0f)
        {
            SoftLightChange();
        }
    }

    void IntensityReset()
    {
        _Light.intensity = intense;
        Timer = Random.Range(minTime, maxTime);
    }

    void SoftLightChange()
    {
       _Light.intensity = intense - (Mathf.Abs(intensityChange - 1) / 3) * changeStrength;

        Invoke("HardLightChange", .3f);
    }

    void HardLightChange()
    {
        _Light.intensity = intense * intensityChange * changeStrength;

        Invoke("IntensityReset", .2f);
    }
}
