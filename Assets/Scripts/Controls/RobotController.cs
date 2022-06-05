using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float speedMult = 1;
    public float rotSpeed = 1;
    public TrackMovement tracks;
    [HideInInspector]
    public Rigidbody rigid;

    public float horizontal = 0;
    public float vertical = 0;

    public bool isConscious;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if(GameObject.FindGameObjectsWithTag("Robot").Length > 1)
        {
            //Prompt consciousness change
            MenuManager._instance.ActivateMenu(1);
            GlobalDataStorage.pendingConSwitch = this;
            isConscious = false;
        }
        else
        {
            isConscious = true;
        }
    }

    private void Update()
    {
        if (MenuManager.isUIOpen)
        {
            return;
        }
        //horizontal = 0;
        vertical = 0;
        if (Input.GetKey(KeyCode.A) && horizontal <= 1)
        {
            horizontal += 0.2f;
        }
        if (Input.GetKey(KeyCode.D) && horizontal >= -1)
        {
            horizontal -= 0.2f;
        }
        if(horizontal > 0)
        {
            horizontal -= 0.1f;
        }
        if (horizontal < 0)
        {
            horizontal += 0.1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            vertical += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vertical -= 1;
        }
    }

    private void FixedUpdate()
    {
        if (isConscious)
        {
            Vector3 localAngVel = transform.InverseTransformDirection(rigid.angularVelocity);
            rigid.angularVelocity = transform.TransformDirection(new Vector3(localAngVel.x, -horizontal * rotSpeed / 2, localAngVel.z));

            tracks.multiplier = vertical * speedMult;
        }
        else
        {
            tracks.multiplier = 0;
        }
    }
}
