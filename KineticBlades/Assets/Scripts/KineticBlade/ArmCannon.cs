using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmCannon : MonoBehaviour
{
    public int ammo = 0;
    protected int ammoMax = 6;
    public TextMeshProUGUI ammoDisplay;

    protected float fireRate = 0.05f;
    public GameObject bulletPrefab;

    protected float elapsedTime;

    public Transform nozzleTransform;

    protected OVRInput.Button shootingButton;

    private void Start()
    {
        InvokeRepeating("AddAmmo", 1f, 1f);
    }

    private void OnApplicationQuit() { CancelInvoke(); }
    private void OnDestroy() { CancelInvoke(); }

    void Update()
    {
        //elapsed time
        elapsedTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            if (elapsedTime > fireRate)
            {
                if (ammo > 0)
                {
                    Shoot();

                    elapsedTime = 0;
                }
            }
        }

    }

    private void Shoot()
    {
        if (ammo > 0)
        {
            AudioController.Play("gunSound", nozzleTransform, 1);

            //Shake Left Controller
            VibrationManager.instance.VibrateController(.05f, .3f, .3f, OVRInput.Controller.LTouch);

            //Create the bullet
            GameObject bulletGameobject = Instantiate(bulletPrefab, nozzleTransform.position, Quaternion.Euler(0, 0, 0));
            bulletGameobject.transform.forward = nozzleTransform.forward;

            ammoDisplay.text = "" + --ammo;
        }
    }

    public void AddAmmo()
    {
        if (++ammo > ammoMax) ammo = ammoMax;
        ammoDisplay.text = ""+ammo;
    }

}