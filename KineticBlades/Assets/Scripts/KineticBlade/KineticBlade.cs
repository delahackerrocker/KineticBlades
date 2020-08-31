using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBlade : HandItem
{
    public bool iAmTeamOne = false;

    public BladeRoot bladeRoot;

    public void SetTeam(bool isTeamOne)
    {
        iAmTeamOne = isTeamOne;
    }
    public void Disintegrate()
    {
        if (bladeRoot != null)
        {
            bladeRoot.Disintegrate();
        } else if (GetComponent<BladeRoot>() != null)
        {
            GetComponent<BladeRoot>().Disintegrate();
        }
        Destroy(this.gameObject);
    }
}