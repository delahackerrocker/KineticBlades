using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
   
    protected float fireRate = 0.05f;
    public GameObject bulletPrefab;

    float elapsedTime;

    public Transform nozzleTransform;

 
    public Animator gunAnimator;

    public OVRInput.Button shootingButton;

    public GameObject slicerGameObject;
    

    // Update is called once per frame
    void Update()
    {
        //elapsed time
        elapsedTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(shootingButton, OVRInput.Controller.LTouch))
        {
            if (elapsedTime > fireRate)
            {
                Shoot();
                
                elapsedTime = 0;
            }
        }

    }

    private void Shoot()
    {
        //Play sound
        AudioManager.instance.gunSound.gameObject.transform.position = nozzleTransform.position;
        AudioManager.instance.gunSound.Play();

        //Play animation
        gunAnimator.SetTrigger("Fire");

        //Shake Left Controller
        VibrationManager.instance.VibrateController(.05f, .3f, .3f, OVRInput.Controller.LTouch);

        //Create the bullet
        GameObject bulletGameobject = Instantiate(bulletPrefab, nozzleTransform.position, Quaternion.Euler(0, 0, 0));
        bulletGameobject.transform.forward = nozzleTransform.forward;

        Physics.IgnoreCollision(bulletGameobject.GetComponent<Collider>(), slicerGameObject.GetComponent<Collider>());
    }

}