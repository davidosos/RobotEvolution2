using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public float force;
    public float maxSpeed = 2.5f;
    [HideInInspector]
    public float leftMultiplier = 1;
    [HideInInspector]
    public float rightMultiplier = 1;
    [HideInInspector]
    public float multiplier = 1;
    public GameObject leftTrackForcePoint;
    public GameObject rightTrackForcePoint;

    public List<Animator> animators;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.root == transform.root)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            return;
        }
        GameObject trackPart = collision.GetContact(0).thisCollider.gameObject;
        if (trackPart.layer == 6)
        {
            if (trackPart.tag == "RightTrack")
            {
                transform.root.GetComponent<Rigidbody>().AddForceAtPosition(transform.root.forward * force * rightMultiplier * multiplier, rightTrackForcePoint.transform.position);
                
            }
            else if(trackPart.tag == "LeftTrack")
            {
                transform.root.GetComponent<Rigidbody>().AddForceAtPosition(transform.root.forward * force * leftMultiplier * multiplier, leftTrackForcePoint.transform.position);
                
            }
            Vector3 curVel = GetComponent<Rigidbody>().velocity;
            Vector3 locVel = transform.InverseTransformDirection(curVel);
            locVel.x = locVel.x * 0.05f;
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);
            GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Clamp(curVel.x, -maxSpeed, maxSpeed), curVel.y, Mathf.Clamp(curVel.z, -maxSpeed, maxSpeed));
        }
    }
    private void Update()
    {
        foreach(Animator animator in animators) 
        {
            animator.speed = Vector3.Dot(new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z), transform.forward) / maxSpeed;
        }
    }
}
