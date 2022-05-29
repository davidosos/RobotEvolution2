using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotConstructor : MonoBehaviour
{
    /*
     * This class constructs the new robot from passed down gene struct
     */

    public GenePool genePool = new GenePool("NULL", null, 0, 0, "NULL");

    public static GenePool basic = new GenePool("Basic/Basic_Body", new string[] { "Base_1", "Highlight_1" }, 3, 1, "Body", new string[] {"CameraTelescope", "Tracks"}, 2, new GenePool[]
                                    {new GenePool("Basic/Basic_CameraTelescope", new string[] {"Base_1", "Highlight_1"}, 1, 2,"CameraTelescope", new string[]{"Camera"}, 1, new GenePool[]
                                        {new GenePool("Basic/Basic_Camera", new string[] {"Base_1", "Highlight_1", "DARK"}, 1, 1, "Camera")}),
                                    new GenePool("Basic/Basic_Tracks", new string[] {"Base_1", "Highlight_1"}, 3, 2, "Tracks")});

    void Start()
    {
        if(genePool.prefab == "NULL")
        {
            //Generate basic robot
            genePool = basic;
            ConstructRobot(genePool, Vector3.zero, Quaternion.identity, transform);
        }
    }

    public void ConstructRobot(GenePool pool, Vector3 position, Quaternion rotation,Transform parent)
    {
        if(pool.prefab == "NULL")
        {
            ConstructRobot(basic, position, rotation, parent);
            return;
        }
        GameObject newPart = Instantiate((GameObject)Resources.Load("Parts/" + pool.prefab), parent);
        newPart.transform.localPosition = position;
        newPart.transform.localRotation = rotation;
        RobotPartInfo rInfo = newPart.GetComponent<RobotPartInfo>();
        if(rInfo == null)
        {
            rInfo = newPart.AddComponent<RobotPartInfo>();
        }
        //Generate other parts
        for(int i = 0; i < pool.childGenes.Length; i++)
        {
            ConstructRobot(pool.childGenes[i], rInfo.partConnections[i], rInfo.partConnectionRots[i], newPart.transform);
        }
        //Add colour
        /*for(int i = 0; i < pool.materials.Length; i++)
        {
            newPart.GetComponent<MeshRenderer>().materials[i] = (Material)Resources.Load("Materials/" + pool.materials[i]);
        }*/
        //Fill in game info
        rInfo.resistance = pool.resistance;
        rInfo.areodynamicity = pool.areoDyn;

        ApplyStatusChange(pool.statChange);
    }
    void ApplyStatusChange(int statusChange)
    {
        if(statusChange == 0)
        {
            return;
        }
        switch (statusChange)
        {
            case 1:
                TrackMovement tr = transform.GetComponent<TrackMovement>();
                tr.maxSpeed += 5;
                transform.GetComponent<RobotController>().rotSpeed += 1;
                break;
            default:
                Debug.LogError("No such status as: " + statusChange);
            break;
        }
    }
}
[System.Serializable]
public struct GenePool
{
    public string prefab;
    public string[] materials;
    public int resistance;
    public int areoDyn;
    /*
     * All changes a gene can have on the robot, with indexes
     * 0 - None
     * 1 - Increase speed to 5
     */
    public int statChange;
    public GenePool[] childGenes;
    public int childCount;
    //The group that this gene is a part of
    public string geneGroup;
    //The group that connected genes need to be a part of on specified slots
    public string[] requiredGeneGroups;

    public GenePool(string prefab, string[] materials, int resistance, int areoDyn, string geneGroup, string[] requiredGeneGroups, int childCount, GenePool[] childGenes, int statChange = 0)
    {
        this.prefab = prefab;
        this.materials = materials;
        this.resistance = resistance;
        this.areoDyn = areoDyn;
        this.childGenes = childGenes;
        this.requiredGeneGroups = requiredGeneGroups;
        this.geneGroup = geneGroup;
        this.childCount = childCount;
        this.statChange = statChange;
    }
    public GenePool(string prefab, string[] materials, int resistance, int areoDyn, string geneGroup, int statChange = 0)
    {
        this.prefab = prefab;
        this.materials = materials;
        this.resistance = resistance;
        this.areoDyn = areoDyn;
        this.childGenes = new GenePool[0];
        this.childCount = 0;
        this.requiredGeneGroups = new string[0];
        this.geneGroup = geneGroup;
        this.statChange = statChange;
    }
}
