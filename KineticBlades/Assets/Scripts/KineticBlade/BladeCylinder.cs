using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeCylinder : MonoBehaviour
{
    protected BoxCollider myBoxCollider;
    protected Rigidbody myRigidBody;

    protected Vector3 basePosition = new Vector3(0, 2.3f, 0f);

    public float energyPool = 0;
    protected float energyMax = .02f;

    public bool energyIsFull = false;

    public bool isDisintegrating = false;
    protected int updatesTillDeath = 65;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = basePosition;
        myBoxCollider = this.GetComponent<BoxCollider>();
        myRigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (energyIsFull)
        {
            // energy is full
        }
        if (isDisintegrating)
        {
            if (updatesTillDeath == 0)
            {
                Destroy(this.gameObject);
                Destroy(myBoxCollider.gameObject);
            } else
            {
                updatesTillDeath--;
            }
        }
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
            isDisintegrating = true;
            this.transform.DetachChildren();
        }
    }
}
