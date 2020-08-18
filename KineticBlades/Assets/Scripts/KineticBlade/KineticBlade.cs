using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBlade : HandItem
{
    public bool iAmTeamOne = false;

    public void SetTeam(bool isTeamOne)
    {
        iAmTeamOne = isTeamOne;
    }
    public void Disintegrate()
    {
        //GetComponent<BladeRoot>().Disintegrate(0);
    }
}