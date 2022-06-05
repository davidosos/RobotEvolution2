using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandWormSpawner : MonoBehaviour
{
    public int length;
    public GameObject entrance;
    public GameObject[] undergroundPath;

    private void Start()
    {
        GameObject worm = Instantiate(Resources.Load<GameObject>("Procedural/Enemies/SandWorm/SandWorm"));
        worm.transform.position = transform.position;
        worm.GetComponent<SandWormConstructor>().Construct(length);
        SandWormAI ai = worm.transform.GetChild(0).GetComponent<SandWormAI>();
        ai.home = gameObject;
        ai.homeEntrance = entrance;
        ai.undergroundPath = undergroundPath;
        ai.isEntering = true;
    }
}
