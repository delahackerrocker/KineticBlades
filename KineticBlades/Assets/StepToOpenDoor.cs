using UnityEngine;

public class StepToOpenDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Transform doorToOpen;
    public Vector3 doorClosedPosition;
    public Vector3 doorOpenedPosition;


    void Start()
    {
        
    }

    void Update()
    {
        if (isOpen)
        {
            doorToOpen.localPosition = doorOpenedPosition;
        } else
        {
            doorToOpen.localPosition = doorClosedPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Door should open for: " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOpen = false;
        }
    }
}
