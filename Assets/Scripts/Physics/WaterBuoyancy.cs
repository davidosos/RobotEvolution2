using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBuoyancy : MonoBehaviour
{
    public float volume;
    public WaterPhysicsEmmiter water;
    public float waterDrag;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if(rigid == null)
        {
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (water != null) 
        {
            float height = Mathf.Pow(volume, 1 / 3);
            float deltaY = water.transform.position.y - transform.position.y;
            float effectiveVolume = volume * Mathf.Clamp(height * deltaY + 0.5f, 0, 1);

            //Add buoyancy force
            rigid.AddForce(Vector3.up * effectiveVolume * 1000 * 9.81f * 0.05f);

            if (effectiveVolume != 0)
            {
                //0.8 is the drag coefficent for a cube
                rigid.AddForce((0.5f * rigid.velocity.magnitude * rigid.velocity.magnitude * 1000 * 0.8f * height * height * 0.05f) * -rigid.velocity.normalized);
                rigid.angularVelocity = rigid.angularVelocity * 0.5f;

                if (rigid.velocity.magnitude < water.streamSpeed)
                {
                    //Add stream force
                    rigid.AddForce(water.force * 0.05f * Mathf.Clamp(Mathf.Abs(water.streamSpeed - rigid.velocity.magnitude), 0, 1));
                }
            }
            
        }
    }
}
