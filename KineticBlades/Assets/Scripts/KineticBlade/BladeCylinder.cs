using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeCylinder : MonoBehaviour
{
    public BladeRoot myBladeRoot;
    public int incrementID = -1;

    protected BoxCollider myBoxCollider;
    protected Rigidbody myRigidBody;

    protected Vector3 basePosition = new Vector3(0, 2.3f, 0f);

    public float energyPool = 0;
    protected float energyMax = .02f;

    public bool energyIsFull = false;

    public bool isDisintegrating = false;
    protected int updatesTillDeath = 3;

    public BladeCylinder nextCylinder = null;

    void Awake()
    {
        myBoxCollider = this.GetComponent<BoxCollider>();
        myRigidBody = this.GetComponent<Rigidbody>();
    }
    void Start()
    {
        this.transform.localPosition = basePosition;
    }

    public float AddEnergy(float newEnergy)
    {
        float leftOverEnergy = 0;
        if (isDisintegrating)
        {
            // already disintegrating
        }
        else
        {
            energyPool += newEnergy;

            if (energyPool > energyMax)
            {
                energyIsFull = true;
                leftOverEnergy = energyPool - energyMax;
                energyPool = energyMax;
            }
        }
        return leftOverEnergy;
    }

    public void Disintegrate()
    {
        if (isDisintegrating)
        {
            // already disintegrating
        } else
        {
            myBoxCollider.isTrigger = false;
            myRigidBody.isKinematic = false;
            myRigidBody.useGravity = true;
            isDisintegrating = true;
            if (nextCylinder != null) 
            {
                nextCylinder.Disintegrate();
                this.transform.DetachChildren();
                Destroy(this.gameObject);
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KineticBlade")
        {
            //myBladeRoot.Disintegrate(incrementID);
        } else if (other.tag == "Strikable")
        {
            myBladeRoot.Disintegrate(incrementID);
        }
        else if (other.tag == "NPCBlade")
        {
            myBladeRoot.Disintegrate(incrementID);
        }
    }
}
