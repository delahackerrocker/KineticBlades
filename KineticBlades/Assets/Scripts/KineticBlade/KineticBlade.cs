using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBlade : HandItem
{
    public void Disintegrate()
    {
        GetComponent<BladeRoot>().Disintegrate(0);
    }
}