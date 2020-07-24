using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class Slicer : MonoBehaviour
{
    public Material materialAfterSlice;
    public LayerMask sliceMask;

    public bool isTouched;

    private void Start()
    {
        VibrationManager instance = VibrationManager.instance;
    }

    private void Update()
    {     
        if (isTouched == true)
        {
            isTouched = false;
            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);
            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {
              
                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialAfterSlice);
                Debug.Log(materialAfterSlice);
                GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialAfterSlice);
                GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialAfterSlice);

             
                upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                lowerHullGameobject.transform.position = objectToBeSliced.transform.position;
               

                MakeItPhysical(upperHullGameobject, objectToBeSliced.gameObject.GetComponent<Rigidbody>().velocity);
                MakeItPhysical(lowerHullGameobject, objectToBeSliced.gameObject.GetComponent<Rigidbody>().velocity);

                Destroy(objectToBeSliced.gameObject);
            }
        }

    }
    private void MakeItPhysical(GameObject obj, Vector3 _velocity)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().velocity = -_velocity;

        float randomNumberX = Random.Range(0f, 4f) - 2f;
        float randomNumberY = Random.Range(0f, 4f) - 2f;
        float randomNumberZ = Random.Range(0f, 4f) - 2f;

        obj.GetComponent<Rigidbody>().AddForce(3*new Vector3(randomNumberX,randomNumberY,randomNumberZ),ForceMode.VelocityChange);       
        obj.AddComponent<DestroyAfterSeconds>();
    }


    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // Shake the sword side controller
        VibrationManager.instance.VibrateController(.2f, .4f, .4f, OVRInput.Controller.RTouch);

        // slice the provided object using the transforms of this object
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }

}
