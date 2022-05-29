using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeOption : MonoBehaviour
{
    public GenePool editMe;
    public TreeOption parent;
    public List<TreeOption> children;
    public int geneIndex = -1;
    public Button button;
    public Text text;

    private GameObject selector;

    private void Start()
    {
        button.onClick.AddListener(Select);
    }
    void Select()
    {
        Destroy(selector);
        selector = Instantiate(Resources.Load<GameObject>("UI/SelectorNewPart"), transform);
        TMP_Dropdown drop = selector.transform.GetChild(0).GetComponent<TMP_Dropdown>();
        foreach (GenePool gene in BetaGenePicker.options)
        {
            if (parent != null)
            {
                if (gene.geneGroup == parent.editMe.requiredGeneGroups[geneIndex])
                {
                    drop.options.Add(new TMP_Dropdown.OptionData(gene.prefab));
                }
            }
            else
            {
                if (gene.geneGroup == "Body")
                {
                    drop.options.Add(new TMP_Dropdown.OptionData(gene.prefab));
                }
            }
        }
        drop.onValueChanged.AddListener(OnNewGeneSelect);
    }
    void OnNewGeneSelect(int option)
    {
        int index = 0;
        foreach (GenePool gene in BetaGenePicker.options)
        {
            if (parent != null)
            {
                if (gene.geneGroup == parent.editMe.requiredGeneGroups[geneIndex])
                {
                    if (index == option)
                    {
                        editMe = gene;
                        UpdateLook();
                        break;
                    }
                    index++;
                }
            }
            else
            {
                if (gene.geneGroup == "Body")
                {
                    if (index == option)
                    {
                        editMe = gene;
                        UpdateLook();
                        break;
                    }
                    index++;
                }
            }
        }
    }
    public void UpdateLook()
    {
        text.text = editMe.prefab;
    }
}
