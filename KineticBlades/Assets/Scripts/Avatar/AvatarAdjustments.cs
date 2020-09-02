using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class AvatarAdjustments : MonoBehaviour
{
    public VRIK avatarEmbodimentRig;

    public XR_Lever avatarScale;
    protected float avatarScaleRange = .14f;
    protected float avatarDefaultScale = 1f;
    protected float avatarScaleIncrement;

    public Transform headTrackerLink;

    public XR_Lever headTrackerY;
    protected float headTrackerYRange = .2f;
    protected float headTrackerYDefault;
    protected float headTrackerYIncrement;

    public XR_Lever headTrackerZ;
    protected float headTrackerZRange = .4f;
    protected float headTrackerZDefault;
    protected float headTrackerZIncrement;

    protected bool uiIsReady = false;

    void Start()
    {
        avatarDefaultScale = avatarDefaultScale - (avatarScaleRange/2);
        avatarScaleIncrement = avatarScaleRange / 100f;

        headTrackerYDefault = headTrackerLink.localPosition.y - (headTrackerYRange / 2f);
        headTrackerYIncrement = headTrackerYRange / 100f;

        headTrackerZDefault = headTrackerLink.localPosition.z - (headTrackerZRange / 2f);
        headTrackerZIncrement = headTrackerYRange / 100f;

        Invoke("SetDefaultLocations", 0.05f);
    }

    void SetDefaultLocations()
    {
        avatarScale.SetStartingPosition(68f);
        headTrackerY.SetStartingPosition(48f);
        headTrackerZ.SetStartingPosition(25f);

        uiIsReady = true;
    }

    void Update()
    {
        if (uiIsReady)
        {
            float adjustedScale = avatarDefaultScale + (avatarScale.normalizedValue * avatarScaleIncrement);
            avatarEmbodimentRig.gameObject.transform.localScale = new Vector3(adjustedScale, adjustedScale, adjustedScale);

            float adjustedHeadTrackerY = headTrackerYDefault + (headTrackerY.normalizedValue * headTrackerYIncrement);
            float adjustedHeadTrackerZ = headTrackerZDefault + (headTrackerZ.normalizedValue * headTrackerZIncrement);
            headTrackerLink.localPosition = new Vector3(headTrackerLink.localPosition.x, adjustedHeadTrackerY, adjustedHeadTrackerZ);
        }
    }
}
