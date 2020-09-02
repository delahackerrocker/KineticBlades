using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFighter : MonoBehaviour
{
    public XR_ThrustLever velocityThrust;
    public XR_Joystick attitudeJoy;
    public Transform gameWorld;
    public Transform playersShip;

    protected float translationDivisor = 8;
    protected float rotationDivisor = 32;
    void Update()
    {
        gameWorld.transform.Translate(0.0f, 0.0f, -velocityThrust.grabbableZ / translationDivisor, Space.World);
        
        gameWorld.transform.RotateAround(this.transform.position, Vector3.right, -attitudeJoy.totalChangeRotationX / rotationDivisor);
        gameWorld.transform.RotateAround(this.transform.position, Vector3.forward, attitudeJoy.totalChangeRotationZ / rotationDivisor);
    }
}
