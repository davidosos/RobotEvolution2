using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates particles on terrain
public class SandWormDirt : MonoBehaviour
{
    private GameObject particles;

    private void Start()
    {
        particles = Instantiate(Resources.Load<GameObject>("Particles/SandParticles"));
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.up, out hit, float.MaxValue, LayerMask.NameToLayer("Terrain")))
        {
            particles.SetActive(true);
            particles.transform.position = hit.point;
        }
        else
        {
            particles.SetActive(false);
        }
    }
}
