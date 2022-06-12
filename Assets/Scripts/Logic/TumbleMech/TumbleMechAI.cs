using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleMechAI : MonoBehaviour
{
    public GameObject[] rocks;
    public Animator drill;
    public Animator[] legs;
    public Collider[] disOnMove;
    public bool isDeployed;
    public float maxSpeed;

    public float moveForce;
    public float drillDist;
    public float drillTime;

    public GameObject ball;
    public float ballRadius;

    private int target = -1;
    private Rigidbody rigid;
    private bool limitVel = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.identity;
        rigid.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if(target == -1)
        {
            target = Random.Range(0, rocks.Length);
        }
        if (limitVel)
        {
            rigid.velocity = new Vector3(rigid.velocity.x / 2, rigid.velocity.y, rigid.velocity.z / 2);
        }
        ball.transform.Rotate(rigid.velocity.z / ballRadius, 0, -rigid.velocity.x / ballRadius, Space.World);
        foreach(Animator a in legs)
        {
            a.SetBool("IsExtended", isDeployed);
        }
        foreach (Collider c in disOnMove)
        {
            c.enabled = isDeployed;
        }
        Vector3 dirToRock = rocks[target].transform.position - transform.position;
        if (rigid.velocity.magnitude < maxSpeed)
        {
            rigid.AddForce(dirToRock.normalized * moveForce);
        }

        if(dirToRock.magnitude <= drillDist)
        {
            isDeployed = true;
            rigid.freezeRotation = false;
            limitVel = true;
            StartCoroutine("disengageDrilling");
        }
    }
    IEnumerator disengageDrilling()
    {
        yield return new WaitForSeconds(drillTime);
        isDeployed = false;
        target = Random.Range(0, rocks.Length);
        transform.rotation = Quaternion.identity;
        rigid.freezeRotation = true;
        limitVel = false;
    }
}
