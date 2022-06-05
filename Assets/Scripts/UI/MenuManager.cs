using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuItem[] items;
    [HideInInspector]
    public MenuItem currentItem;
    public static bool isUIOpen = false;
    public static bool isTyping = false;

    public static MenuManager _instance;

    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        if (!isTyping)
        {
            foreach (MenuItem mItem in items)
            {
                if (Input.GetKeyDown(mItem.triggerKey))
                {
                    mItem.gameObject.SetActive(true);
                    currentItem = mItem;
                    DisableInactive();
                }
            }
        }
        if (currentItem != null)
        {
            isUIOpen = currentItem.gameObject.activeSelf;
        }
        else
        {
            isUIOpen = false;
        }
    }

    public void ActivateMenu(int menu)
    {
        currentItem = items[menu];
        currentItem.gameObject.SetActive(true);
        DisableInactive();
    }

    public void DisableInactive()
    {
        foreach(MenuItem item in items)
        {
            if(item != currentItem)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
