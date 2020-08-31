using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFighter : MonoBehaviour
{
    public XR_ThrustLever velocityThrust;
    public XR_Joystick attitudeJoy;
    public Transform gameWorld;

    protected float rateOfChange = 32;
    void Update()
    {
        gameWorld.transform.Translate(0.0f, 0.0f, -velocityThrust.grabbableZ / rateOfChange, Space.Self);
        gameWorld.transform.Rotate(attitudeJoy.totalChangeRotationX / rateOfChange, 0.0f, -attitudeJoy.totalChangeRotationZ / rateOfChange, Space.Self);
    }
}
