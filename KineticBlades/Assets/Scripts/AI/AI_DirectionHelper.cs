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

        int healthBarsToShow = Mathf.RoundToInt(aiNPC.health / healthBars.Length);
        for (int index = 0; index < healthBarsToShow; index++)
        {
            healthBars[index].SetActive(true);
        }
    }
}
