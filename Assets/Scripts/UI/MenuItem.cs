using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    public KeyCode triggerKey;
    public KeyCode disableKey;

    private void Update()
    {
        if (disableKey != KeyCode.None)
        {
            if (Input.GetKeyDown(disableKey))
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(triggerKey))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
