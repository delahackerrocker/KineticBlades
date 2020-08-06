using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_NPC : MonoBehaviour
{
    public AI_TargetingStack aiTargetingStack;
    protected AI_Target aiTarget;
    protected Transform lookTarget;

    protected NavMeshAgent navMeshAgent;

    public bool lockOnToggle = true;
    public bool targetAssigned = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        Invoke("AssignNewTarget", .1f);
        InvokeRepeating("AssignNewTarget", 2f, 2f);
    }


    void Update()
    {
        if (targetAssigned)
        {
            if (lockOnToggle)
            {
                var direction = new Vector3(lookTarget.position.x, transform.position.y, lookTarget.position.z) - transform.position;
                var rotation = Quaternion.LookRotation(direction);

                //rotation.eulerAngles = new Vector3(Mathf.Clamp(rotation.eulerAngles.x, -30, 30), rotation.eulerAngles.y, rotation.eulerAngles.z);

                transform.rotation = rotation;
            }
        }
    }

    protected void AssignNewTarget()
    {
        if (lookTarget == null)
            lookTarget = GameObject.FindGameObjectWithTag("Player").transform;

        aiTarget = aiTargetingStack.GetRandomTarget();
        navMeshAgent.destination = aiTarget.transform.position;

        targetAssigned = true;
    }
}
