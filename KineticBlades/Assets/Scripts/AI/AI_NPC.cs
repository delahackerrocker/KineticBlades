using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AI_NPC : MonoBehaviour
{
    // they AI get's targets around the real target from this
    public AI_TargetingStack aiTargetingStack;
    // this helper is part of how we track our actual heading vs direction
    public AI_DirectionHelper aiDirectionHelper;

    [HideInInspector] public Transform lookTarget;

    protected AI_Target aiTarget;

    protected NavMeshAgent navMeshAgent;
    public Animator animator;

    public KineticBlade leftHandKineticBlade;
    public KineticBlade rightHandKineticBlade;

    protected int attackStateHash = Animator.StringToHash("Attack.Attack");
    protected int attackVariation = 0;

    protected bool shouldAttackNow = false;
    protected int attackDesire = 0;
    protected int desireThreshold = 10;
    protected float decisionTimer = 1f;
    protected float strikingDistance = 2f;

    [HideInInspector] public bool lockOnToggle = true;
    [HideInInspector] public bool targetAssigned = false;

    [HideInInspector] public Vector3 movementVector;

    [HideInInspector] public bool killMe = false;

    protected int health = 10;

    void Start()
    {
        aiTargetingStack = Instantiate(aiTargetingStack, this.transform.position, Quaternion.identity);
        aiTargetingStack.aiNPC = this;

        aiDirectionHelper.aiNPC = this;
        aiDirectionHelper.aiTransform = this.transform;
        aiDirectionHelper.animationTransform = animator.transform;

        aiDirectionHelper = Instantiate(aiDirectionHelper, this.transform.position, Quaternion.identity);

        navMeshAgent = GetComponent<NavMeshAgent>();

        SetRigidBodyState(true);
        SetRigidColliderState(false);

        movementVector = Vector3.zero;
    }

    private void OnApplicationQuit()
    {
        CancelInvoke();
    }


    void Update()
    {
        if (killMe)
        {
            // I'm dead
        }
        else
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

                            attackVariation = Random.Range(1, 4);

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
    }

    public void NewTargetStackTarget()
    {
        CancelInvoke();

        int shouldStayOrGo = Random.Range(0, 10);

        if (shouldStayOrGo > 2)
        {
            aiTarget = aiTargetingStack.GetRandomTarget();
            navMeshAgent.destination = aiTarget.transform.position;

            targetAssigned = true;
        }

        Invoke("NewTargetStackTarget", 1f);
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bullet")
		{
            Damage();
		}
		else if (other.tag == "Sword")
		{
            Damage();
		}
		else if (other.tag == "KineticBlade")
		{
            if (leftHandKineticBlade != null)
            {
                bool itsNotMine = false;

                if (other.gameObject == leftHandKineticBlade.gameObject)
                {
                    // can't be hurt by your own blade
                } else
                {
                    itsNotMine = true;
                }

                if (other.gameObject == rightHandKineticBlade.gameObject)
                {
                    // can't be hurt by your own blade
                }
                else
                {
                    itsNotMine = true;
                }

                if (itsNotMine) Damage();
            }
		}
	}

    void Damage()
    {
        health--;
        if (health <= 0)
        {
            //Die();
        }
    }

    void Die()
	{
        Debug.Log("DEATH to " + this.name);

        killMe = true;

        Destroy(this.GetComponent<Collider>());

        animator.enabled = false;
		navMeshAgent.enabled = false;

		CancelInvoke();

		SetRigidBodyState(false);
		SetRigidColliderState(true);

        if (leftHandKineticBlade != null) leftHandKineticBlade.Disintegrate();
        if (rightHandKineticBlade != null) rightHandKineticBlade.Disintegrate();

        Destroy(gameObject, 10f);
        Destroy(this);
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