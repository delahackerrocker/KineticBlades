using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFighter : MonoBehaviour
{
    public XR_Joystick attitudeJoy;
    public Transform gameWorld;

    void Update()
    {
        //this.transform.Rotate(attitudeJoy.totalChangeRotationX/100, 0.0f, -attitudeJoy.totalChangeRotationZ/100, Space.Self);
        //this.transform.Translate(attitudeJoy.totalChangeRotationZ / 100, 0.0f, -attitudeJoy.totalChangeRotationX / 100, Space.Self);
        gameWorld.transform.Rotate(attitudeJoy.totalChangeRotationX / 100, 0.0f, -attitudeJoy.totalChangeRotationZ / 100, Space.Self);
    }
}
