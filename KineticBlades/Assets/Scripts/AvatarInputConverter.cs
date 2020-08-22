using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{
    protected bool handTrackingIsInstant = true;

    // Avatar Transforms
    public Transform mainAvatarTransform;
    public Transform avatarHead;
    public Transform avatarBody;

    public Transform avatarLeftHand;
    public Transform avatarRightHand;

    // Ovulus Transforms
    public Transform oculusHead;
    public Transform oculusLeftHand;
    public Transform oculusRightHand;

    public Vector3 positionOffset;

    protected float lerpSmoothing = 0.05f;

    public int currentlySelectedSword = 0;
    public GameObject[] swords;

    // Start is called before the first frame update
    void Start()
    {
        // Set all of the avatar game objects to active
        // just in case they were hidden for adjustments
        //avatarHead.gameObject.SetActive(true);
        //avatarBody.gameObject.SetActive(true);
        //avatarLeftHand.gameObject.SetActive(true);
        //avatarRightHand.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // place the entire body where the center eye anchor is on OVRPlayerController
        //mainAvatarTransform.position = Vector3.Lerp(mainAvatarTransform.position, oculusHead.position + positionOffset, lerpSmoothing);
        // The heads rotation will be updated to match CenterEyeAnchor
        //avatarHead.rotation = Quaternion.Lerp(avatarHead.rotation, oculusHead.rotation, lerpSmoothing);

        // Rotate the body just off the Y of the head
        //Vector3 avatarHeadRotY = new Vector3(0, avatarHead.rotation.eulerAngles.y, 0);
        //avatarBody.rotation = Quaternion.Lerp(avatarBody.rotation, Quaternion.Euler(avatarHeadRotY), (lerpSmoothing*2));

        avatarLeftHand.position = oculusLeftHand.position;
        avatarRightHand.position = oculusRightHand.position;
        // ...and then the rotation of hand anchors
        avatarLeftHand.rotation = oculusLeftHand.rotation;
        avatarRightHand.rotation = oculusRightHand.rotation;
        
        // The player can use their button on their right controller to change swords
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            ///ShowNextSword();
        }
    }

    protected void HideAllSwords()
    {
        for ( int index = 0; index < swords.Length; index++ )
        {
            swords[index].SetActive(false);
        }
    }

    protected void ShowSword(int swordToShow)
    {
        HideAllSwords();
        swords[currentlySelectedSword].SetActive(true); 
    }

    protected void ShowNextSword()
    {
        currentlySelectedSword++;
        if (currentlySelectedSword == swords.Length) currentlySelectedSword = 0;
        ShowSword(currentlySelectedSword);
    }
}
