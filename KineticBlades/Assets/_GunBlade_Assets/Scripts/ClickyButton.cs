using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ClickyButton : MonoBehaviour
{
    public float MinLocalY = 0.25f;
    public float MaxLocalY = 0.55f;
  
    public bool isBeingTouched = false;
    public bool isClicked = false;

    public bool soundPlayed = false;

    public Material greenMat;

    public float smooth = 0.1f;

    protected Vector3 defaultPosition;

    public bool nowTesting = false;

    void Start()
    {
        Reset();
    }

    private void OnApplicationQuit() { CancelInvoke(); }
    private void OnDestroy() { CancelInvoke(); }

    protected void Reset()
    {
        // Start with button up top / popped up
        defaultPosition = transform.localPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);

        GetComponent<MeshRenderer>().material = greenMat;
    }

    private void Update()
    {
        Vector3 buttonDownPosition = new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z);
        Vector3 buttonUpPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
        if (!isClicked)
        {
            if (!isBeingTouched && (transform.localPosition.y > MaxLocalY  || transform.localPosition.y < MaxLocalY))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, buttonUpPosition, Time.deltaTime * smooth);
            }

            if (transform.localPosition.y < MinLocalY)
            {
                isClicked = true;               
                transform.localPosition = buttonDownPosition;
                OnButtonDown();
            }
        }
      
    }


    void OnButtonDown()
    {
        GetComponent<MeshRenderer>().material = greenMat;
        GetComponent<Collider>().isTrigger = true;

        ////Playing Sound
        AudioManager.instance.buttonClickSound.gameObject.transform.position = transform.position;
        AudioManager.instance.buttonClickSound.Play();

        Invoke("Reset", .1f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isClicked)
        {
            ////Playing Sound

            if (!soundPlayed)
            {
                AudioManager.instance.buttonClickSound.gameObject.transform.position = transform.position;
                AudioManager.instance.buttonClickSound.Play();

                VibrationManager.instance.VibrateController(.1f, .1f, .1f, OVRInput.Controller.LTouch);
                VibrationManager.instance.VibrateController(.1f, .1f, .1f, OVRInput.Controller.RTouch);

                soundPlayed = true;
            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = false;
            transform.localPosition = defaultPosition;
        }
    }
}