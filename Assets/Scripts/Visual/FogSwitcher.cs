using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSwitcher : MonoBehaviour
{
    public FogPreset[] presets;
    public int active;
    private void Start()
    {
        presets[0] = new FogPreset(RenderSettings.fogDensity, RenderSettings.fogMode, RenderSettings.fogColor, "Default");
    }
#if UNITY_EDITOR
    private void Update()
    {
        //UpdateFog();
    }
#endif
    public void OnSetFog(string fogName)
    {
        for(int i = 0; i < presets.Length; i++)
        {
            if(presets[i].name == fogName)
            {
                active = i;
                return;
            }
        }
        UpdateFog();
    }
    public void ResetFog()
    {
        active = 0;
        UpdateFog();
    }

    void UpdateFog()
    {
        RenderSettings.fogDensity = presets[active].density;
        RenderSettings.fogMode = presets[active].fogMode;
        RenderSettings.fogColor = presets[active].color;
    }
}
[System.Serializable]
public struct FogPreset
{
    public float density;
    public FogMode fogMode;
    public Color color;
    public string name;

    public FogPreset(float density, FogMode fogMode, Color color, string name)
    {
        this.density = density;
        this.fogMode = fogMode;
        this.color = color;
        this.name = name;
    }
}
