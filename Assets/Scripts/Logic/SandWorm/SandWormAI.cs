using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormAI : MonoBehaviour
{
    public GameObject target;
    public GameObject currentlyFollowing { get; private set; }
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

    public List<GameObject> ignored;
    private GameObject head;

    public GameObject held;
    public Rigidbody heldRigid;

    private void Start()
    {
        head = gameObject;
        ignored = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !MenuManager.isTyping)
        {
            ignored = new List<GameObject>();
        }
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
                        heldRigid = null;
                        ignored.Add(held);
                        held = null;
                    }
                }
            }
        }
    }

    void Follow(GameObject o)
    {
        currentlyFollowing = o;
        head.transform.rotation = Quaternion.Lerp(head.transform.rotation, XLookRotation(o.transform.position - head.transform.position), 0.1f);

        float dist = Vector3.Distance(head.transform.position, o.transform.position);
        head.GetComponent<Rigidbody>().AddForce(head.transform.right * force * Mathf.Clamp(dist * distModifier, 0, maxDistMultiplier));
        head.GetComponent<Rigidbody>().AddForce(transform.forward * Mathf.Sin(dist * distModifier) * 500);
        Debug.DrawLine(head.transform.position, head.transform.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody == heldRigid)
        {
            Physics.IgnoreCollision(collision.collider, collision.GetContact(0).thisCollider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject == target)
        {
            Rigidbody r = other.transform.root.GetComponent<Rigidbody>();
            Transform o = other.transform.root;
            if (r != null)
            {
                heldRigid = r;
                r.isKinematic = true;
            }
            o.parent = head.transform;
            o.localPosition = new Vector3(10.8f, 0, 0);
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
