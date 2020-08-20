using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        // magnitude math goes here
        // -- 

        // then we do the thing that will go for some time
        StartCoroutine(VibrateForSeconds(duration, frequency, amplitude, controller));
    }

    IEnumerator VibrateForSeconds(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);

        // we're going to keep doing the thing above until duration has run it's time
        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0f, 0f, controller);
    }
}