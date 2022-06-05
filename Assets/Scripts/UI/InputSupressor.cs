using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputSupressor : MonoBehaviour
{
    private TMP_InputField field;

    private void Start()
    {
        field = GetComponent<TMP_InputField>();
        if(field != null)
        {
            field.onSelect.AddListener(StartTyping);
            field.onDeselect.AddListener(EndTyping);
            field.onEndEdit.AddListener(EndTyping);
        }
    }
    void StartTyping(string inp)
    {
        MenuManager.isTyping = true;
    }
    void EndTyping(string inp)
    {
        MenuManager.isTyping = false;
    }
}
