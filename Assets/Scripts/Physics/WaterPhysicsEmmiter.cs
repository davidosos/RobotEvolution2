using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysicsEmmiter : MonoBehaviour
{
    public Vector3 force;
    public float streamSpeed;
    private void OnTriggerStay(Collider other)
    {
        WaterBuoyancy b = other.transform.root.GetComponent<WaterBuoyancy>();
        if(b != null)
        {
            b.water = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        WaterBuoyancy b = other.transform.root.GetComponent<WaterBuoyancy>();
        if (b != null)
        {
            b.water = null;
        }
    }
}
