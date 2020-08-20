using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DirectionHelper : MonoBehaviour
{
    public AI_NPC aiNPC;
    public Transform aiTransform;
    public Transform animationTransform;

    public Vector3 previousPosition;
    public Vector3 currentPosition;
    public Vector3 movementVector;

    public GameObject[] healthBars;

    void Start()
    {
        currentPosition = previousPosition = this.transform.position;
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

    void UpdateHealthBars()
    {
        for (int index = 0; index < healthBars.Length; index++)
        {
            healthBars[index].SetActive(false);
        }

        int ratio = Mathf.FloorToInt(aiNPC.maxHealthTwo / healthBars.Length);
        int relativeHealth = Mathf.FloorToInt(aiNPC.healthTwo / ratio);
        Debug.Log("---> [UpdateHealthBars] " + this.name);
        Debug.Log("---> [UpdateHealthBars] :: health:" + aiNPC.healthTwo);
        Debug.Log("---> [UpdateHealthBars] :: maxHealth:" + aiNPC.maxHealthTwo);
        Debug.Log("---> [UpdateHealthBars] :: ratio:" + ratio);
        Debug.Log("---> [UpdateHealthBars] :: relativeHealth:" + relativeHealth);
        for (int index = 0; index < relativeHealth; index++)
        {
            Debug.Log("---> [UpdateHealthBars] :: index:" + index);
            healthBars[index].SetActive(true);
        }
    }
}
