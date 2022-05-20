using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float speedMult = 1;
    public TrackMovement tracks;
    public Rigidbody rigid;

    private float horizontal = 0;
    private float vertical = 0;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = 0;
        vertical = 0;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal -= 1;
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
        transform.Rotate(transform.up, horizontal);

        tracks.multiplier = vertical * speedMult;
    }
}
