using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogEmmiter : MonoBehaviour
{
    public string fogName;
    private void OnTriggerEnter(Collider other)
    {
        FogSwitcher sw = other.GetComponent<FogSwitcher>();
        if (sw != null && other.GetComponent<Camera>().enabled)
        {
            sw.OnSetFog(fogName);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        FogSwitcher sw = other.GetComponent<FogSwitcher>();
        if (sw != null && other.GetComponent<Camera>().enabled)
        {
            sw.ResetFog();
        }
    }
}
