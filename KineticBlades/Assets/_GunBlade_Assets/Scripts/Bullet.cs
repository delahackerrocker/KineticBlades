using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Start is called before the first frame update

    //bullet speed
    protected float speed = 10f;
    //rigidbody of the bullet

    Rigidbody rb;
   
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        BabyIWasBornToDie();
    }

    protected float secondsTillDestruction = 10f;
    void BabyIWasBornToDie()
    {
        Destroy(gameObject, secondsTillDestruction);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
