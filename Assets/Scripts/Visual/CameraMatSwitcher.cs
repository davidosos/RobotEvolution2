using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMatSwitcher : MonoBehaviour
{
    public List<Material> mats;
    public int current;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(mats[current] == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        Graphics.Blit(source, destination, mats[current]);
        Debug.Log("Hello");
    }
}
