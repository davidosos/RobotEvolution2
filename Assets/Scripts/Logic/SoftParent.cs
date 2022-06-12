using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftParent : MonoBehaviour
{
    public GameObject with;
    
    void FixedUpdate()
    {
        transform.position = with.transform.position;
    }
}
