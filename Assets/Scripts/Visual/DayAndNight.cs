using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Range(0,1)]
    public float time;
    public bool manual = false;
    public float timeRate;

    [Header("Sun")]
    public Light sun;
    public Material sunSkybox;
    public Gradient sunColor;
    public AnimationCurve sunIntesity;

    [Header("Moon")]
    public Light moon;
    public Material moonSkybox;
    public Gradient moonColor;
    public AnimationCurve moonIntesity;
    public GameObject stars;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        time += timeRate;
        if(time > 1)
        {
            time = 0;
        }
        transform.rotation = Quaternion.Euler(time * 360, 0, 0);

        sun.color = sunColor.Evaluate(time);
        sun.intensity = sunIntesity.Evaluate(time);

        moon.color = moonColor.Evaluate(time);
        moon.intensity = moonIntesity.Evaluate(time);
        //Switch skybox
        if(time > 0 && time < 0.5f)
        {
            //Day
            RenderSettings.skybox = sunSkybox;
            RenderSettings.sun = sun;
            RenderSettings.ambientIntensity = 1;
            stars.SetActive(false);
        }
        else
        {
            //Night
            RenderSettings.skybox = moonSkybox;
            RenderSettings.sun = moon;
            RenderSettings.ambientIntensity = 0;
            stars.SetActive(true);
        }
    }
}
