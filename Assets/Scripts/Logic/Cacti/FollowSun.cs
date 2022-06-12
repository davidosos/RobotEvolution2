using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSun : MonoBehaviour
{
    public GameObject rotY;
    public GameObject rotX;

    private Light dirLight;

    private void Start()
    {
        foreach(Light t in FindObjectsOfType<Light>())
        {
            if(t.type == LightType.Directional)
            {
                dirLight = t;
            }
        }
        if(dirLight == null)
        {
            gameObject.SetActive(false);
        }
        Debug.Log(dirLight.transform.eulerAngles);
    }

    private void FixedUpdate()
    {
        rotY.transform.rotation = yRotation(dirLight.transform.rotation) * Quaternion.AngleAxis(180, Vector3.up);
        rotX.transform.localRotation = Quaternion.Inverse(xRotation(dirLight.transform.rotation));
        rotX.transform.Rotate(90, 0, 0, Space.Self);
    }
    //I have no clue about how this works, gl
    private Quaternion yRotation(Quaternion q)
    {
        float theta = Mathf.Atan2(q.y, q.w);

        // quaternion representing rotation about the y axis
        return new Quaternion(0, Mathf.Sin(theta), 0, Mathf.Cos(theta));
    }
    private Quaternion xRotation(Quaternion q)
    {
        float theta = Mathf.Atan2(q.x, q.w);

        // quaternion representing rotation about the x axis
        return new Quaternion(Mathf.Sin(theta), 0, 0, Mathf.Cos(theta));
    }
}
