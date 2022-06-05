using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormAI : MonoBehaviour
{
    public GameObject target;
    public GameObject[] undergroundPath;
    private int underIndex = 0;
    public GameObject homeEntrance;
    public bool isEntering = false;
    public GameObject home;
    public float force;
    public float maxDistMultiplier;
    public float distModifier;
    public float visibleRadius;
    public float wayPointAcceptDist = 2f;

    private List<GameObject> ignored;
    private GameObject head;

    [HideInInspector]
    public GameObject held;

    private void Start()
    {
        head = gameObject;
        ignored = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        target = GetClosestPrey(visibleRadius);
        if (target != null)
        {
            Follow(target);
        }
        else
        {
            if (held == null)
            {
                if (!isEntering)
                {
                    Follow(undergroundPath[underIndex]);
                    if (Vector3.Distance(undergroundPath[underIndex].transform.position, transform.position) < wayPointAcceptDist)
                    {
                        underIndex++;
                        if (underIndex >= undergroundPath.Length)
                        {
                            underIndex = 0;
                        }
                    }
                }
                else
                {
                    Follow(homeEntrance);
                    if (Vector3.Distance(homeEntrance.transform.position, transform.position) < wayPointAcceptDist)
                    {
                        isEntering = false;
                    }
                }
            }
            else 
            {
                if (!isEntering)
                {
                    Follow(homeEntrance);
                    if (Vector3.Distance(homeEntrance.transform.position, transform.position) < wayPointAcceptDist)
                    {
                        isEntering = true;
                    }
                } else if (isEntering)
                {
                    Follow(home);
                    if (Vector3.Distance(home.transform.position, transform.position) < wayPointAcceptDist)
                    {
                        held.transform.parent = null;
                        if (held.GetComponent<Rigidbody>())
                        {
                            held.GetComponent<Rigidbody>().isKinematic = false;
                        }
                        ignored.Add(held);
                        held = null;
                    }
                }
            }
        }
    }

    void Follow(GameObject o)
    {
        head.transform.rotation = Quaternion.Lerp(head.transform.rotation, XLookRotation(o.transform.position - head.transform.position), 0.1f);

        float dist = Vector3.Distance(head.transform.position, o.transform.position);
        head.GetComponent<Rigidbody>().AddForce(head.transform.right * force * Mathf.Clamp(dist * distModifier, 0, maxDistMultiplier));
        head.GetComponent<Rigidbody>().AddForce(transform.forward * Mathf.Sin(dist * distModifier) * 500);
        Debug.DrawLine(head.transform.position, head.transform.right);
    }

    //Returns null if no object is found
    GameObject GetClosestPrey(float radius)
    {
        GameObject closest = null;
        float lastDist = float.MaxValue;
        foreach(Collider coll in Physics.OverlapSphere(transform.position, radius))
        {
            Vector3 pos = coll.transform.position;
            if ((coll.transform.root.tag == "Prey" || coll.transform.root.tag == "Robot") && !ignored.Contains(coll.transform.root.gameObject))
            {
                if (new Vector3(pos.x - transform.position.x, 0, pos.z - transform.position.z).magnitude < 5 && pos.y > transform.position.y)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject == target)
        {
            Rigidbody r = other.transform.root.GetComponent<Rigidbody>();
            other.transform.root.parent = head.transform;
            other.transform.root.localPosition = new Vector3(10.8f, 0, 0);
            if (r != null)
            {
                r.isKinematic = true;
            }
            held = target;
            target = null;
        }
    }

    Quaternion XLookRotation(Vector3 right)
    {
        Quaternion rightToForward = Quaternion.Euler(0f, -90f, 0f);
        Quaternion forwardToTarget = Quaternion.LookRotation(right, Vector3.up);

        return forwardToTarget * rightToForward;
    }
}
