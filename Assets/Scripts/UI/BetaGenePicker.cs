using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetaGenePicker : MonoBehaviour
{
    public Button triggerButton;
    public GameObject currentTree;
    public float uiOptionDist = 25;
    public float spaceAngle = 0.5f;

    public static GenePool current = RobotConstructor.basic;
    public static List<GenePool> options = new List<GenePool>()
    {
        new GenePool("Basic/Basic_Body", new string[] { "Base_1", "Highlight_1" }, 3, 1, "Body", new string[] {"CameraTelescope", "Tracks"}, 5, new GenePool[5]),
        new GenePool("Basic/Basic_CameraTelescope", new string[] {"Base_1", "Highlight_1"}, 1, 2,"CameraTelescope", new string[] {"Camera" }, 1, new GenePool[1]),
        new GenePool("Basic/Basic_Camera", new string[] {"Base_1", "Highlight_1", "DARK"}, 1, 1, "Camera"),
        new GenePool("Basic/Basic_Tracks", new string[] {"Base_1", "Highlight_1"}, 3, 2, "Tracks"),
        new GenePool("Agility/Agility_Tracks", new string[] {"Base_1", "Highlight_1"}, 2, 3, "Tracks", 1),
        new GenePool("Agility/Agility_Body", new string[] {"Base_1", "Highlight_1"}, 3, 3, "Body", new string[] {"CameraTelescope", "Tracks"}, 5, new GenePool[5], 1)
    };
    [HideInInspector]
    public static GenePool[] allOptions = new GenePool[]
    {
        new GenePool("Basic/Basic_Body", new string[] { "Base_1", "Highlight_1" }, 3, 1, "Body", new string[] {"CameraTelescope", "Tracks"}, 5, new GenePool[5]),
        new GenePool("Basic/Basic_CameraTelescope", new string[] {"Base_1", "Highlight_1"}, 1, 2,"CameraTelescope", new string[] {"Camera" }, 1, new GenePool[1]),
        new GenePool("Basic/Basic_Camera", new string[] {"Base_1", "Highlight_1", "DARK"}, 1, 1, "Camera"),
        new GenePool("Basic/Basic_Tracks", new string[] {"Base_1", "Highlight_1"}, 3, 2, "Tracks"),
        new GenePool("Agility/Agility_Tracks", new string[] {"Base_1", "Highlight_1"}, 2, 3, "Tracks", 1),
        new GenePool("Agility/Agility_Body", new string[] {"Base_1", "Highlight_1"}, 3, 3, "Body", new string[] {"CameraTelescope", "Tracks"}, 5, new GenePool[5], 1)
    };

    public static void AddOption(int option)
    {
        options.Add(allOptions[option]);
    }

    void Awake()
    {
        triggerButton.onClick.AddListener(CreateEgg);

        ConstructTree();
    }

    void ConstructTree()
    {
        Destroy(currentTree);
        currentTree = new GameObject("GeneTree");
        currentTree.transform.parent = transform;
        currentTree.transform.localPosition = Vector3.zero;

        RecursiveConstructor(current, transform.position, transform.position, new GenePool("NULL", null, 0, 0, "NULL"), -1, currentTree.transform);
    }

    void RecursiveConstructor(GenePool current, Vector3 where, Vector3 last, GenePool geneParent, int geneIndex, Transform parent)
    {
        //Create current node
        GameObject node = Instantiate(Resources.Load<GameObject>("UI/TreeOption"), parent);
        node.transform.position = where;
        TreeOption option = node.GetComponent<TreeOption>();
        TreeOption parentOption = parent.GetComponent<TreeOption>();

        option.editMe = current;
        option.geneIndex = geneIndex;
        option.parent = parentOption;

        if (parentOption != null)
        {
            parent.GetComponent<TreeOption>().children.Add(option);
        }
        option.UpdateLook();

        if(where == last)
        {
            //Full circle
            for(int a = 0; a < current.childCount; a++)
            {
                float angle = 2 * Mathf.PI * a / current.childGenes.Length;
                float x = Mathf.Cos(angle) * uiOptionDist;
                float y = Mathf.Sin(angle) * uiOptionDist;
                if (float.IsNaN(x) || float.IsNaN(y))
                {
                    Debug.LogError("Warning NaN in angle calculation");
                }
                RecursiveConstructor(current.childGenes[a], new Vector3(x + where.x, y + where.y), where, current, a, node.transform);
            }
        }
        else
        {
            //Calculate dead angle
            Vector3 toParent = last - where;
            float startAngle = Mathf.Asin(toParent.normalized.x) + spaceAngle;
            for (int a = 0; a < current.childCount; a++)
            {
                float angle = 2 * (Mathf.PI - spaceAngle) * a / current.childGenes.Length;
                float x = Mathf.Cos(angle + startAngle) * uiOptionDist;
                float y = Mathf.Sin(angle + startAngle) * uiOptionDist;
                if(float.IsNaN(x) || float.IsNaN(y))
                {
                    Debug.LogError("Warning NaN in angle calculation");
                }
                RecursiveConstructor(current.childGenes[a], new Vector3(x + where.x, y + where.y), where, current, a, node.transform);
            }
        }
    }

    public GenePool GetModifiedGenePool()
    {
        TreeOption start = currentTree.transform.GetChild(0).GetComponent<TreeOption>();
        GenePool startGene = start.editMe;
        startGene.childGenes = GetChildGenes(start);
        return startGene;
    }
    private GenePool[] GetChildGenes(TreeOption of)
    {
        List<GenePool> list = new List<GenePool>();
        for(int i = 0; i < of.children.Count; i++)
        {
            of.children[i].editMe.childGenes = GetChildGenes(of.children[i]);
            list.Add(of.children[i].editMe);
        }
        return list.ToArray();
    }

    void CreateEgg()
    {
        //Apply changes
        current = GetModifiedGenePool();
        //Actually create the egg
        GameObject egg = Instantiate(Resources.Load<GameObject>("Basic_Egg"));
        egg.GetComponent<Egg>().pool = current;
        //Not final
        Vector3 robPos = FindConscious().gameObject.transform.position;
        egg.transform.position = new Vector3(robPos.x, robPos.y + 10, robPos.z);
    }
    static RobotController FindConscious() 
    {
        foreach(RobotController c in FindObjectsOfType<RobotController>())
        {
            if (c.isConscious)
            {
                return c;
            }
        }
        return null;
    }
}
