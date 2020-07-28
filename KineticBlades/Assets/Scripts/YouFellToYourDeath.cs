using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouFellToYourDeath : MonoBehaviour
{
    public GameObject ovrPlayerController;
    public GameObject ovrSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ovrPlayerController.transform.position = ovrSpawner.transform.position;
        ovrPlayerController.transform.rotation = ovrSpawner.transform.rotation;
    }

    private void OnTriggerExit(Collider other)
    {
        ovrPlayerController.transform.position = ovrSpawner.transform.position;
        ovrPlayerController.transform.rotation = ovrSpawner.transform.rotation;
    }
}
