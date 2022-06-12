using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public float force;
    public float maxSpeed = 2.5f;
    public float multiplier = 0;

    private Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();   
    }
    private void OnCollisionStay(Collision collision)
    {
        GameObject trackPart = collision.GetContact(0).thisCollider.gameObject;
        float frontSpeed = Vector3.Dot(rigid.velocity, transform.forward);
        if (trackPart.transform.CompareTag("TrackPart") && frontSpeed <= maxSpeed && frontSpeed >= -maxSpeed)
        {
            rigid.AddForce(transform.root.forward * force * multiplier);
            Vector3 curVel = GetComponent<Rigidbody>().velocity;
            Vector3 locVel = transform.InverseTransformDirection(curVel);
            locVel.x = locVel.x * 0.05f;
            rigid.velocity = transform.TransformDirection(locVel);
        }
    }
}
