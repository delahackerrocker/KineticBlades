using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInventory : MonoBehaviour
{
    [SerializeField]
    private OVRInput.Controller m_controller = OVRInput.Controller.None;    // set in inspector

    protected int selected = 0;
    public HandItem[] handItems;

    void Start()
    {
        TurnOnItem(selected);
    }

    void Update()
    {
        // The player can use their button on their right controller to change swords
        if (OVRInput.GetDown(OVRInput.Button.Two, m_controller))
        {
            NextItem();
        }
    }

    void NextItem()
    {
        if (selected == handItems.Length) selected = 0;

        // turn on the next item in handItems while turning off the rest
        TurnOffAllItems();
        TurnOnItem(++selected);
    }

    void TurnOnItem(int selected)
    {
        handItems[selected].gameObject.SetActive(true);
        this.selected = selected;
    }

    void TurnOffAllItems()
    {
        for (int index=0; index<handItems.Length; index++)
        {
            handItems[index].gameObject.SetActive(false);
        }
    }
}
