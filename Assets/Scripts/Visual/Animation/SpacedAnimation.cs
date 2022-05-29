using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacedAnimation : MonoBehaviour
{
    public GameObject target;
    public int desiredCount;

    public List<VisualWheel> visWheels;

    public List<Animator> animators;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = transform.root.GetComponent<Rigidbody>();
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
                animators.Add(anim);
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (VisualWheel wh in visWheels)
        {
            float fromSpeed = Vector3.Dot(new Vector3(rigid.velocity.x, 0, rigid.velocity.z), transform.root.forward) * wh.bearing / 2.5f;
            float fromAngular = rigid.angularVelocity.y * -wh.hObject.transform.localPosition.x * wh.bearing / 2.5f;
            wh.hObject.transform.Rotate(fromSpeed + fromAngular, 0, 0);
        }
        foreach (Animator animator in animators)
        {
            float fromAngular = rigid.angularVelocity.y * -animator.gameObject.transform.localPosition.x / 2.5f;
            float fromSpeed = Vector3.Dot(new Vector3(rigid.velocity.x, 0, rigid.velocity.z), transform.forward) / 2.5f;
            float total = fromSpeed + fromAngular;
            animator.SetFloat("Multiplier", total);
        }
    }
}
[System.Serializable]
public struct VisualWheel
{
    public GameObject hObject;
    public float bearing;

    public VisualWheel(GameObject hObject, float bearing)
    {
        this.hObject = hObject;
        this.bearing = bearing;
    }
}
