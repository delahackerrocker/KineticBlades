using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XR_Joystick : MonoBehaviour
{
    public GameObject rotationBase;
    public GameObject grabber;
    public GameObject stick;

    protected Vector3 originalRotation = new Vector3(90f, -90f, 90f);
    protected float maxGrabberDrag = 4f;
    protected float maxRotationAngle = 40f;

    void Start()
    {

    }


    void Update()
    {
        Vector3 newRotation = new Vector3(0f, 0f, 0f);
        float grabbableZ = grabber.transform.localPosition.z;
        float rotationRatioX = maxRotationAngle / maxGrabberDrag;
        float totalChangeRotationX = 0;
        if (grabbableZ > 0f)
        {
            if (grabbableZ > maxGrabberDrag) grabbableZ = maxGrabberDrag;
            totalChangeRotationX = rotationRatioX * grabbableZ;
        }
        else
        {
            if (grabbableZ < -maxGrabberDrag) grabbableZ = -maxGrabberDrag;
            totalChangeRotationX = rotationRatioX * grabbableZ;
        }

        float grabbableX = grabber.transform.localPosition.x;
        float rotationRatioZ = maxRotationAngle / maxGrabberDrag;
        float totalChangeRotationZ = 0;
        if (grabbableX > 0f)
        {
            if (grabbableX > maxGrabberDrag) grabbableX = maxGrabberDrag;
            totalChangeRotationZ = rotationRatioZ * grabbableX;
        }
        else
        {
            if (grabbableX < -maxGrabberDrag) grabbableX = -maxGrabberDrag;
            totalChangeRotationZ = rotationRatioZ * grabbableX;
        }

        rotationBase.transform.localEulerAngles = newRotation;
        rotationBase.transform.Rotate(totalChangeRotationX, 0.0f, -totalChangeRotationZ, Space.Self);
    }
}