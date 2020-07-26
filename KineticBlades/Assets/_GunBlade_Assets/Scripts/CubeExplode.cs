using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplode : MonoBehaviour
{

    public GameObject shatteredObject;
    public GameObject mainCube;
    // Start is called before the first frame update
    void Start()
    {
        mainCube.SetActive(true);
        shatteredObject.SetActive(false);
    }

    public void IsShot()
    {
        Destroy(mainCube);
        shatteredObject.SetActive(true);
        var shatterAnimation = shatteredObject.GetComponent<Animation>().Play();

        MakeItPhysical(this.gameObject, this.gameObject.GetComponent<Rigidbody>().velocity);

        Destroy(shatteredObject,2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            IsShot();
        }
    }

    private void MakeItPhysical(GameObject obj, Vector3 _velocity)
    {
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().velocity = _velocity / 8;
        obj.GetComponent<Rigidbody>().useGravity = true;

        float randomNumberX = Random.Range(0f, .2f) - .1f;
        float randomNumberY = Random.Range(0f, .2f) - .1f;
        float randomNumberZ = Random.Range(0f, .2f) - .1f;

        obj.GetComponent<Rigidbody>().AddForce(3 * new Vector3(randomNumberX, randomNumberY, randomNumberZ), ForceMode.Impulse);
        obj.AddComponent<DestroyAfterSeconds>();
    }
}
