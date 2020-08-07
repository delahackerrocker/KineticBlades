using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AI_NPC : MonoBehaviour
{
    // they AI get's targets around the real target from this
    public AI_TargetingStack aiTargetingStack;

    protected AI_Target aiTarget;
    public Transform lookTarget;

    protected NavMeshAgent navMeshAgent;
    public Animator animator;

    protected int attackStateHash = Animator.StringToHash("Attack.Attack");
    protected int attackVariation = 0;

    protected bool shouldAttackNow = false;
    protected int attackDesire = 0;
    protected int desireThreshold = 40;
    protected float decisionTimer = 1f;
    protected float strikingDistance = 2f;

    [HideInInspector] public bool lockOnToggle = true;
    [HideInInspector] public bool targetAssigned = false;

    [HideInInspector] public Vector3 movementVector;

    public bool killMe = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        SetRigidBodyState(true);
        SetRigidColliderState(false);

        movementVector = Vector3.zero;

        Invoke("AssignNewTarget", .1f);
        InvokeRepeating("AssignNewTarget", decisionTimer, decisionTimer);
    }

    private void OnApplicationQuit()
    {
        CancelInvoke();
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


            animator.SetFloat("Forward", Mathf.Abs(movementVector.z));
            animator.SetFloat("Strafe", movementVector.x);

            float distanceToEnemy = Vector3.Distance(lookTarget.position, transform.position);

            if (distanceToEnemy < 4)
            {
                attackDesire++;

                if (distanceToEnemy < strikingDistance)
                {
                    attackDesire++;

                    if (attackDesire >= desireThreshold)
                    {
                        shouldAttackNow = true;
                        attackDesire = Random.Range(0, desireThreshold);
                    }

                    if (shouldAttackNow)
                    {
                        Debug.Log("Attack");

                        if (attackVariation == 0)
                        {
                            attackVariation = 1;
                        }
                        else if (attackVariation == 1)
                        {
                            attackVariation = 2;
                        }
                        else if (attackVariation == 2)
                        {
                            attackVariation = 3;
                        }
                        else if (attackVariation == 3)
                        {
                            attackVariation = 4;
                        }
                        else if (attackVariation == 4)
                        {
                            attackVariation = 1;
                        }
                        animator.SetInteger("AttackVariation", attackVariation);
                        animator.SetTrigger("Attack");

                        float newDefensiveness = Random.Range(0f, 2f);
                        animator.SetFloat("Defensiveness", newDefensiveness);

                        shouldAttackNow = false;
                    }
                }
            }
        }
    }

    protected void AssignNewTarget()
    {
        int shouldStayOrGo = Random.Range(0, 10);

        if (shouldStayOrGo > 2)
        {
            if (lookTarget == null)
                lookTarget = GameObject.FindGameObjectWithTag("Player").transform;

            aiTarget = aiTargetingStack.GetRandomTarget();
            navMeshAgent.destination = aiTarget.transform.position;

            targetAssigned = true;
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bullet")
		{
			Die();
		}
		else if (other.tag == "Sword")
		{
			Die();
		}
		else if (other.tag == "KineticBlade")
		{
			Die();
		}
	}

	void Die()
	{
		GetComponent<Animator>().enabled = false;
		GetComponent<NavMeshAgent>().enabled = false;

		CancelInvoke();

		SetRigidBodyState(false);
		SetRigidColliderState(true);

		Destroy(gameObject, 10f);
	}

    void SetRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        //GetComponent<Rigidbody>().isKinematic = !state;
    }
    void SetRigidColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }
}