using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsciousnessSwitcher : MonoBehaviour
{
    public Button yes;
    public Button no;

    public TMP_Text distanceText;

    private void OnEnable()
    {
        yes.onClick.AddListener(OnYes);
        no.onClick.AddListener(OnNo);
    }

    private void Update()
    {
        foreach (RobotController o in FindObjectsOfType<RobotController>())
        {
            if (o.isConscious)
            {
                distanceText.text = Vector3.Distance(o.transform.position, GlobalDataStorage.pendingConSwitch.transform.position).ToString();
            }
        }
    }

    void OnYes()
    {
        foreach(RobotController o in FindObjectsOfType<RobotController>())
        {
            o.isConscious = false;
        }
        try
        {
            GlobalDataStorage.pendingConSwitch.isConscious = true;
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Consciousness switcher button pressed without pending consciousness switch. (Gameobject: " + transform.name + ")");
        }
        GlobalDataStorage.pendingConSwitch = null;
        gameObject.SetActive(false);
    }
    void OnNo()
    {
        GlobalDataStorage.pendingConSwitch = null;
        gameObject.SetActive(false);
    }
}
