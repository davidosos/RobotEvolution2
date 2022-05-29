using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivot : MonoBehaviour
{
    public float sens;
    public float maxDist = 4.5f;

    public GameObject cam;
    public RobotController controller;

    private void Start()
    {
        controller = transform.root.GetComponent<RobotController>();
    }

    private void Update()
    {
        if (controller == null || controller.isConscious)
        {
            cam.GetComponent<Camera>().enabled = true;
            transform.Rotate(0, Input.GetAxis("Mouse X") * sens * Time.deltaTime, 0, Space.World);
            transform.Rotate(Input.GetAxis("Mouse Y") * sens * Time.deltaTime, 0, 0, Space.Self);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

            RaycastHit hit;
            int layer = ~LayerMask.GetMask("Robot");
            if (Physics.Raycast(transform.position, -transform.forward, out hit, maxDist, layer))
            {
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, -hit.distance);
            }
            else
            {
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, -maxDist);
            }
        }
        else
        {
            cam.GetComponent<Camera>().enabled = false;
        }
    }
}
