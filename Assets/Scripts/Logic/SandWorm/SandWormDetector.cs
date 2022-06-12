using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormDetector : MonoBehaviour
{
    public SandWormAI ai;

    GameObject lastTarget;
    GameObject target;

    Rigidbody r;
    private void Start()
    {
        r = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //Actually detect
        GameObject prey = GetClosestPrey(ai.visibleRadius);
        if(ai.target == null && prey != null)
        {
            ai.target = prey;
        }
        //Align with movement axis
        //Help:
        //https://math.stackexchange.com/questions/1905533/find-perpendicular-distance-from-point-to-line-in-3d
        if (target != ai.currentlyFollowing)
        {
            lastTarget = target;
            target = ai.currentlyFollowing;
        }
        if (lastTarget != null)
        {
            Vector3 dirToTarget = (target.transform.position - lastTarget.transform.position).normalized;
            Vector3 dirToPos = transform.position - lastTarget.transform.position;

            //Potentional error: Unity uses left handed coordinate system
            float dist = Vector3.Dot(dirToTarget, dirToPos);

            Vector3 to = lastTarget.transform.position + dist * dirToTarget;
            Vector3 force = ((to - transform.position).normalized * Time.deltaTime * ai.force * Mathf.Clamp(dist, 0, 1)) * 10;
            r.AddForce(force);
        }
    }

    //Returns null if no object is found
    GameObject GetClosestPrey(float radius)
    {
        GameObject closest = null;
        float lastDist = float.MaxValue;
        foreach (Collider coll in Physics.OverlapSphere(transform.position, radius))
        {
            Vector3 pos = coll.transform.position;
            if ((coll.transform.root.tag == "Prey" || coll.transform.root.tag == "Robot") && !ai.ignored.Contains(coll.transform.root.gameObject))
            {
                float flatDist = new Vector3(pos.x - transform.position.x, 0, pos.z - transform.position.z).magnitude;
                if (flatDist < 10)
                {
                    if (closest == null)
                    {
                        closest = coll.transform.root.gameObject;
                    }
                    else
                    {
                        float dist = Vector3.Distance(coll.transform.root.position, transform.position);
                        if (dist < lastDist)
                        {
                            closest = coll.transform.root.gameObject;
                        }
                    }
                }
            }
        }
        return closest;
    }
}
