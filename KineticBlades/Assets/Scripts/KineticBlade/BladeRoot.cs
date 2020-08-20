using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRoot : MonoBehaviour
{
    public static BladeRoot instance;
    public GameObject bladeCylinderTemplate;

    protected int increments;
    public int incrementMax = 50;
    protected GameObject[] bladeCylinders;
    protected BladeCylinder currentBladeCylinder = null;
    protected Transform parentTarget;

    protected Vector3 lastPosition;
    protected Vector3 currentPosition;
    protected Quaternion lastRotation;
    protected Quaternion currentRotation;

    public float movementMagnitude = 0;
    public float rotationMagnitude = 0;

    protected float movementChangeThreshold = .01f;
    protected float movementMagnify = .15f;
    protected float rotationChangeThreshold = .01f;
    protected float rotationMagnify = 2.5f;

    public bool testDisintegration = false;

    public bool justBroke = false;

    public bool iAmTeamOne = false;
    public bool iAmPlayer = false;

    public void SetTeam(bool isTeamOne)
    {
        iAmTeamOne = isTeamOne;
    }

    void Start()
    {
        instance = this;
        InitializeSword();
    }

    protected void InitializeSword()
    {
        justBroke = false;

        increments = 0;

        lastPosition = this.transform.position;
        currentPosition = this.transform.position;

        lastRotation = this.transform.rotation;
        currentRotation = this.transform.rotation;

        parentTarget = this.transform;

        this.transform.localScale = new Vector3(0.03217896f, 0.004119071f, 0.03217896f);

        bladeCylinders = new GameObject[incrementMax];

        movementMagnitude = 0;
        rotationMagnitude = 0;
        CreateBladeCylinder();
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = this.transform.position;

        float movementChange = Vector3.Distance(currentPosition, lastPosition);
        float absMovementChange = Mathf.Abs(movementChange);
        if (absMovementChange > movementChangeThreshold) movementMagnitude += (absMovementChange/movementMagnify);

        float rotationChange = Quaternion.Angle(lastRotation, currentRotation);
        float absRotationChange = Mathf.Abs(rotationChange);
        if (absRotationChange > rotationChangeThreshold) rotationMagnitude += (absRotationChange * rotationMagnify);

        TransferEnergy();
    }

    public void TransferEnergy()
    {
        if (currentBladeCylinder != null)
        { 
            if (currentBladeCylinder.energyIsFull)
            {
                // the energy of this BladeCylinder is full make a new one
                CreateBladeCylinder();
            }
            else
            {
                // add our movement magnitude as energy to our current BladeCylinder
                movementMagnitude = currentBladeCylinder.AddEnergy(movementMagnitude);
                rotationMagnitude = currentBladeCylinder.AddEnergy(rotationMagnitude);

                if ((movementMagnitude > 0) || (rotationMagnitude > 0)) TransferEnergy();
            }
        }
    }

    public void CreateBladeCylinder()
    {
        if (increments == incrementMax)
        {
            // We've reached the maximum number of BladeCylinders
        }
        else
        {
            // create a new BladeCylinder and parent it to the parentTarget
            bladeCylinders[increments] = Instantiate(bladeCylinderTemplate);
            bladeCylinders[increments].transform.SetParent(parentTarget);

            // get a reference to the current BladeCylinder's class
            if (increments > 0 && !justBroke) currentBladeCylinder.nextCylinder = bladeCylinders[increments].GetComponent<BladeCylinder>();
            currentBladeCylinder = bladeCylinders[increments].GetComponent<BladeCylinder>();
            currentBladeCylinder.myBladeRoot = this;
            currentBladeCylinder.incrementID = increments;
            currentBladeCylinder.SetTeam(iAmTeamOne);

            justBroke = false;

            // make our newest BladeCylinder the parentTarget
            parentTarget = bladeCylinders[increments].transform;

            // set the newest Blade Cylinder's position
            bladeCylinders[increments].transform.localScale = Vector3.one;
            bladeCylinders[increments].transform.localRotation = Quaternion.Euler(0, 0, 0);
            bladeCylinders[increments].transform.localPosition = new Vector3(0f, 0f, 0f);

            increments++;

            //TransferEnergy();
        }
    }

    public void ResetMovementMagnitude()
    {
        movementMagnitude = 0;
    }

    public void Disintegrate()
    {
        if (bladeCylinders[1] != null) bladeCylinders[1].GetComponent<BladeCylinder>().Disintegrate();
        currentBladeCylinder = bladeCylinders[0].GetComponent<BladeCylinder>();

        currentBladeCylinder.transform.DetachChildren();

        increments = 1;

        parentTarget = currentBladeCylinder.transform;

        movementMagnitude = 0;
        rotationMagnitude = 0;

        if (iAmPlayer) AudioManager.instance.swordShatter.Play();
    }
}