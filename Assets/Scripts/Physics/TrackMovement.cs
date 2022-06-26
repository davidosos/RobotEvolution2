using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public float force;
    public float maxPartForce;
    public float splitForce;
    public float maxSpeed = 2.5f;
    public float multiplier = 0;

    private Rigidbody rigid;
    private List<GameObject> touching;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        touching = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (touching.Count != 0)
        {
            splitForce = Mathf.Clamp(force / touching.Count, 0, maxPartForce);
        }
        else
        {
            splitForce = maxPartForce;
        }
        Debug.Log(touching.Count);
        touching.Clear();
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject trackPart = collision.GetContact(0).thisCollider.gameObject;
        float frontSpeed = Vector3.Dot(rigid.velocity, transform.forward);

        Vector3 curVel = GetComponent<Rigidbody>().velocity;
        Vector3 locVel = transform.InverseTransformDirection(curVel);
        locVel.x = locVel.x * 0.05f;

        Vector3 forceVec = trackPart.transform.up * splitForce * multiplier * 0.05f;
        if (trackPart.transform.CompareTag("TrackPart"))
        {
            //Add to part list to split force between parts
            if (!touching.Contains(trackPart))
            {
                touching.Add(trackPart);
            }

            if (frontSpeed <= maxSpeed && frontSpeed >= -maxSpeed)
            {
                rigid.AddForce(forceVec);
            }
            if(frontSpeed >= maxSpeed + 2 && frontSpeed <= -maxSpeed - 2 && multiplier != 0)
            {
                locVel.z = locVel.z * 0.5f;
            }
        }
        rigid.velocity = transform.TransformDirection(locVel);
    }
}
