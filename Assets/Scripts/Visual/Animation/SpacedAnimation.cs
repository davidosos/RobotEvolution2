using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacedAnimation : MonoBehaviour
{
    public GameObject target;
    public int desiredCount;

    public List<>

    private void Start()
    {
        if (!target.GetComponent<Animator>())
        {
            Debug.LogError("Animator not found on target GameObject: " + target.name);
            this.enabled = false;
        }
        for(int i = 0; i < desiredCount; i++)
        {
            if(i != 0)
            {
                GameObject t = Instantiate(target, transform);
                Animator anim = t.GetComponent<Animator>();
                anim.Play("TrackMove", 0, (float)i / (float)desiredCount);
                transform.root.GetComponent<TrackMovement>().animators.Add(anim);
            }
        }
    }
}
public struct VisualWheel
{
    public GameObject hObject;
    public float speed;

    public VisualWheel(GameObject hObject, float speed)
    {
        this.hObject = hObject;
        this.speed = speed;
    }
}
