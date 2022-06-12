using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormConstructor : MonoBehaviour
{
    public GameObject start;
    public GameObject mid;
    public GameObject end;


    //Length is the count of mid joints
    public Rigidbody Construct(int length)
    {
        GameObject head = Instantiate(start, transform);
        SandWormAI ai = head.GetComponent<SandWormAI>();

        SandWormPart lastPart = head.GetComponent<SandWormPart>();

        for (int i = 0; i < length; i++)
        {
            GameObject midObject = Instantiate(mid, transform);
            midObject.transform.localPosition = new Vector3(-i * 10.9f - 2.8f, 0, 0);

            SandWormPart s = midObject.GetComponent<SandWormPart>();
            lastPart.joint_f.connectedBody = s.joint_m.GetComponent<Rigidbody>();

            SandWormDetector detector = midObject.GetComponent<SandWormDetector>();
            if(detector != null && ai != null)
            {
                detector.ai = ai;
            }

            lastPart = s;
        }
        GameObject tail = Instantiate(end, transform);
        tail.transform.localPosition = new Vector3(-length * 10.9f - 2.8f, 0, 0);
        lastPart.joint_f.connectedBody = tail.GetComponent<SandWormPart>().joint_m.GetComponent<Rigidbody>();

        return head.GetComponent<Rigidbody>();
    }
}
