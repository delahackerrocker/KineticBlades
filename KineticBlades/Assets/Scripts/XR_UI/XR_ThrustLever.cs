using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XR_ThrustLever : MonoBehaviour
{
    public Transform handleBase;
    public Vector3 handleBaseStartPosition;
    public OVRGrabbable grabber;
    public Vector3 grabberStartPosition;

    public Transform grabberLink;

    public Transform pointZero;
    public Transform pointOneHundred;

    public float grabbableZ = 0;

    void Start()
    {
        grabberStartPosition = grabber.transform.localPosition;
        handleBaseStartPosition = handleBase.localPosition;
    }

    void Update()
    {
        grabbableZ = grabber.transform.localPosition.z - grabberStartPosition.z;
        Vector3 newPosition;

        if (grabbableZ > pointZero.localPosition.z)
        {
            if (grabbableZ > pointOneHundred.localPosition.z)
            {
                grabbableZ = pointOneHundred.localPosition.z;
            }
            newPosition = new Vector3(handleBaseStartPosition.x, handleBaseStartPosition.y, grabbableZ);

            handleBase.localPosition = newPosition;
        }
        else
        {
            grabbableZ = 0f;

            handleBase.localPosition = handleBaseStartPosition;
        }
        
        if (grabber.isGrabbed)
        {
            // Do nothing
        } else
        {
            grabber.transform.position = grabberLink.position;
        }
    }
}