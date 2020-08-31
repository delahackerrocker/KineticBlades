using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DirectionHelper_Ranged : MonoBehaviour
{
    public AI_NPC_RANGED aiNPC;
    public Transform aiTransform;
    public Transform animationTransform;

    public Vector3 previousPosition;
    public Vector3 currentPosition;
    public Vector3 movementVector;

    public GameObject[] healthBars;

    void Start()
    {
        currentPosition = previousPosition = this.transform.position;
        previousRelativeHealth = aiNPC.maxHealthTwo;
    }


    void Update()
    {
        if (aiNPC == null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            this.transform.position = new Vector3(aiTransform.position.x, 0, aiTransform.position.z);
            this.transform.rotation = animationTransform.rotation;

            previousPosition = currentPosition;
            currentPosition = this.transform.position;

            movementVector = currentPosition - previousPosition;
            movementVector *= 100;

            aiNPC.movementVector = movementVector;

            UpdateHealthBars();
        }
    }

    public Material healthBarGood;
    public Material healthBarBad;
    protected int previousRelativeHealth;
    void UpdateHealthBars()
    {
        int ratio = Mathf.FloorToInt(aiNPC.maxHealthTwo / healthBars.Length);
        int relativeHealth = Mathf.FloorToInt(aiNPC.healthTwo / ratio);

        if (previousRelativeHealth == relativeHealth)
        {
            // Do Nothing
        }
        else
        {
            for (int index = 0; index < healthBars.Length; index++)
            {
                healthBars[index].GetComponent<MeshRenderer>().material = healthBarBad;
            }

            for (int index = 0; index < relativeHealth; index++)
            {
                healthBars[index].GetComponent<MeshRenderer>().material = healthBarGood;
            }

            previousRelativeHealth = relativeHealth;
        }
    }
}
