using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates particles on terrain
public class SandWormDirt : MonoBehaviour
{
    private ParticleSystem particles;
    private TerrainCollider coll;

    int origRate;

    private void Start()
    {
        particles = Instantiate(Resources.Load<GameObject>("Particles/SandParticles")).GetComponent<ParticleSystem>();
        coll = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray r = new Ray(new Vector3(transform.position.x, 10000, transform.position.z), -Vector3.up);
        if(coll.Raycast(r, out hit, 10000 - transform.position.y))
        {
            particles.Play();
            particles.transform.position = hit.point;
        }
        else
        {
            particles.Stop();
        }
    }
}
