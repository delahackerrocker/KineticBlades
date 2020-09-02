using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ValueDisplay : MonoBehaviour
{
    public XR_Lever xrLever;
    public TextMeshProUGUI tmpDisplay;

    void Update()
    {
        tmpDisplay.text = ""+xrLever.normalizedValue;
    }
}