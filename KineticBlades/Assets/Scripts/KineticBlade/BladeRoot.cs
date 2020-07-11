using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRoot : MonoBehaviour
{
    public static BladeRoot instance;
    public GameObject bladeCylinderTemplate;

    protected int increments;
    protected int incrementMax = 100;
    protected GameObject[] bladeCylinders;
    protected BladeCylinder currentBladeCylinder;
    protected Transform parentTarget;

    protected Vector3 lastPosition;
    protected Vector3 currentPosition;
    protected Quaternion lastRotation;
    protected Quaternion currentRotation;

    public float movementMagnitude = 0;
    public float rotationMagnitude = 0;

    protected float movementChangeThreshold = .007f;
    protected float rotationChangeThreshold = .007f;
    protected float rotationMagnify = 3.5f;

    public bool testDisintegration = false;

    public bool isDisintegrating = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitializeSword();
    }

    protected void InitializeSword()
    {
        isDisintegrating = false;

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
        if (isDisintegrating)
        {
            // Do Nothing
        } else
        {
            lastPosition = currentPosition;
            currentPosition = this.transform.position;

            float movementChange = Vector3.Distance(currentPosition, lastPosition);
            float absMovementChange = Mathf.Abs(movementChange);
            if (absMovementChange > movementChangeThreshold) movementMagnitude += absMovementChange;

            float rotationChange = Quaternion.Angle(lastRotation, currentRotation);
            float absRotationChange = Mathf.Abs(rotationChange);
            if (absRotationChange > rotationChangeThreshold) rotationMagnitude += (absRotationChange * rotationMagnify);

            TransferEnergy();

            // Reset the Movement Magnitude if they press OVRInput.Button.Two
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                Disintegrate();
            }

            if (testDisintegration)
            {
                testDisintegration = false;
                Disintegrate();
            }
        }
    }

    public void TransferEnergy()
    {
        if (isDisintegrating)
        {
            // Do Nothing
        }
        else
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
        if (isDisintegrating)
        {
            // Do Nothing
        }
        else
        {
            if (increments >= incrementMax)
            {
                // We've reached the maximum number of BladeCylinders
            }
            else
            {
                // create a new BladeCylinder and parent it to the parentTarget
                bladeCylinders[increments] = Instantiate(bladeCylinderTemplate);
                bladeCylinders[increments].transform.SetParent(parentTarget);

                // get a reference to the current BladeCylinder's class
                currentBladeCylinder = bladeCylinders[increments].GetComponent<BladeCylinder>();

                // make our newest BladeCylinder the parentTarget
                parentTarget = bladeCylinders[increments].transform;

                // set the newest Blade Cylinder's position
                bladeCylinders[increments].transform.localScale = Vector3.one;
                bladeCylinders[increments].transform.localRotation = Quaternion.Euler(0, 0, 0);
                bladeCylinders[increments].transform.localPosition = new Vector3(0f, 0f, 0f);

                increments++;

                TransferEnergy();
            }
        }
    }

    public void ResetMovementMagnitude()
    {
        movementMagnitude = 0;
    }

    public void Disintegrate()
    {
        for (int index = 0; index < increments; index++)
        {
            bladeCylinders[index].GetComponent<BladeCylinder>().Disintegrate();
        }
        this.transform.DetachChildren();

        isDisintegrating = true;
        Invoke("InitializeSword", .2f);
    }
}
