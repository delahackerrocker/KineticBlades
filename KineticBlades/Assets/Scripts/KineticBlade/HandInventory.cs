using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInventory : MonoBehaviour
{
    [SerializeField]
    public OVRInput.Touch controllerInput = OVRInput.Touch.None;    // set in inspector

    protected int selected = 0;
    public HandItem[] handItems;

    void Start()
    {
        TurnOffAllItems();
        TurnOnItem(selected);
    }

    void Update()
    {
        // The player can use their button on their right controller to change swords
        if (OVRInput.GetUp(controllerInput))
        {
            NextItem();
        }
    }

    void NextItem()
    {
        if (selected == handItems.Length) selected = 0;

        // turn on the next item in handItems while turning off the rest
        TurnOffAllItems();
        TurnOnItem(selected);

        selected++;
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
