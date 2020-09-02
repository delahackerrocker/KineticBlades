using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XR_Lever : MonoBehaviour
{
    public Transform handleBase;
    public Vector3 handleBaseStartPosition;
    public OVRGrabbable grabber;
    public Vector3 grabberStartPosition;

    public Transform grabberLink;

    public Transform pointZero;
    public Transform pointOneHundred;

    public float changeInPosition = 0.0f;
    public float increment = 0.0f;
    public float normalizedValue = 0.0f;

    public bool isUsingGrabberOverride = false; // for testing without the headset, and manual overrides

    void Start()
    {
        grabberStartPosition = grabber.transform.localPosition;
        handleBaseStartPosition = handleBase.localPosition;

        increment = (pointOneHundred.localPosition.z - pointZero.localPosition.z) / 100f;
    }

    void Update()
    {
        if (grabber.isGrabbed || isUsingGrabberOverride)
        {
            changeInPosition = grabber.transform.localPosition.z - grabberStartPosition.z;
            Vector3 newPosition;

            if (changeInPosition > pointZero.localPosition.z)
            {
                if (changeInPosition > pointOneHundred.localPosition.z)
                {
                    changeInPosition = pointOneHundred.localPosition.z;
                }
                newPosition = new Vector3(handleBaseStartPosition.x, handleBaseStartPosition.y, changeInPosition);

                handleBase.localPosition = newPosition;
            }
            else
            {
                changeInPosition = 0f;

                handleBase.localPosition = handleBaseStartPosition;
            }

            normalizedValue = Mathf.RoundToInt(changeInPosition / increment);
        }
        else
        {
            grabber.transform.position = grabberLink.position;
        }
    }

    public void SetStartingPosition(float newNormalizedValue)
    {
        float newPosition = (newNormalizedValue * increment) + grabberStartPosition.z;
        grabber.transform.localPosition = new Vector3(grabber.transform.localPosition.x, grabber.transform.localPosition.y, newPosition);
        this.normalizedValue = newNormalizedValue;

        // override the grabber temporarily so the handle position updates, then turn it off 
        isUsingGrabberOverride = true;
        Invoke("DisableOverride", 1f);
    }

    void DisableOverride()
    {
        isUsingGrabberOverride = false;
    }

    protected void OnApplicationQuit() { CancelInvoke(); }
    protected void OnDestroy() { CancelInvoke(); }
}