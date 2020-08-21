using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AI_NPC : MonoBehaviour
{
    public AI_Team myTeam;

    public bool iAmTeamOne = false;

    // they AI get's targets around the real target from this
    public AI_TargetingStack aiTargetingStack;
    // this helper is part of how we track our actual heading vs direction
    protected AI_DirectionHelper aiDirectionHelper;

    // these are the two prefabs we could use for aiDirectionHelper
    public AI_DirectionHelper aiDirectionHelper_TeamOne;
    public AI_DirectionHelper aiDirectionHelper_TeamTwo;

    [HideInInspector] public Transform lookTarget;

    protected AI_Target aiTarget;

    protected NavMeshAgent navMeshAgent;
    public Animator animator;

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

    [HideInInspector] protected int health = 250;
    [HideInInspector] public int healthTwo = 250;
    [HideInInspector] protected int maxHealth = 250;
    [HideInInspector] public int maxHealthTwo = 250;

    public GameObject[] skins;

    protected bool engagingTarget = false;

    void Start()
    {
        aiTargetingStack = Instantiate(aiTargetingStack, this.transform.position, Quaternion.identity);
        aiTargetingStack.aiNPC = this;

        if (myTeam.isTeamOne)
        {
            aiDirectionHelper = Instantiate(aiDirectionHelper_TeamOne, this.transform.position, Quaternion.identity);
        } else
        {
            aiDirectionHelper = Instantiate(aiDirectionHelper_TeamTwo, this.transform.position, Quaternion.identity);
        }

        aiDirectionHelper.aiNPC = this;
        aiDirectionHelper.aiTransform = this.transform;
        aiDirectionHelper.animationTransform = animator.transform;

        navMeshAgent = GetComponent<NavMeshAgent>();

        if (rightHandKineticBlade != null) rightHandKineticBlade.GetComponent<KineticBlade>().iAmTeamOne = iAmTeamOne;

        SetRigidBodyState(true);
        SetRigidColliderState(false);

        movementVector = Vector3.zero;

        ChangeSkin();
    }
    private void OnApplicationQuit() { CancelInvoke(); }
    private void OnDestroy() { CancelInvoke(); }

    public void ChangeSkin()
    {
        for (int index = 0; index < skins.Length - 1; index++)
        {
            skins[index].SetActive(false);
        }
        skins[Random.Range(0,skins.Length-1)].SetActive(true);
    }

    void Update()
    {
        if (this.healthTwo <= 0) killMe = true;

        if (killMe)
        {
            Die();
        }
        else
        {
            
            if (engagingTarget == false)
            {
                if (lookTarget != null)
                {
                    float distanceToStartTargeting = Vector3.Distance(lookTarget.position, transform.position);
                    if (distanceToStartTargeting < 10)
                    {
                        NewTargetStackTarget();
                    }
                } else
                {
                    myTeam.MyTargetDied(this);
                }
            }
            if (targetAssigned)
            {
                if (lookTarget.GetComponent<AI_NPC>() == null)
                {
                    // we need to get a new target
                    myTeam.MyTargetDied(this);
                }

                float distanceToTarget = Vector3.Distance(lookTarget.position, transform.position);

                // are we close enough to lock on the look?
                if (distanceToTarget < 10)
                {
                    if (engagingTarget == false)
                    {
                        NewTargetStackTarget();
                    }
                    lockOnToggle = true;
                } else
                {
                    lockOnToggle = false;
                }

                // if we are close enough...
                if (lockOnToggle)
                {
                    var direction = new Vector3(lookTarget.position.x, transform.position.y, lookTarget.position.z) - transform.position;
                    var rotation = Quaternion.LookRotation(direction);

                    transform.rotation = rotation;
                }

                animator.SetFloat("Forward", Mathf.Abs(movementVector.z));
                animator.SetFloat("Strafe", -movementVector.x);

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
                            //Debug.Log("Attack");

                            attackVariation = Random.Range(1, 4);

                            animator.SetInteger("AttackVariation", attackVariation);
                            animator.SetTrigger("Attack");

                            animator.SetFloat("Defensiveness", 2f);

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

        GetComponent<CapsuleCollider>().enabled = true;

        int shouldStayOrGo = Random.Range(0, 6);

        if (shouldStayOrGo > 2)
        {
            aiTarget = aiTargetingStack.GetRandomTarget();
            if (aiTarget != null && navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.destination = aiTarget.transform.position;
                targetAssigned = true;
                engagingTarget = true;
            }
        }

        Invoke("NewTargetStackTarget", 1f);
    }

	void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Bullet")
		{
            myTeam.MyTargetDied(this);
            BulletDamage();
        }
		else if (other.tag == "KineticBlade")
		{
            myTeam.MyTargetDied(this);
            Damage();
		}
	}

    void Damage()
    {
        this.healthTwo = this.healthTwo - 2;
    }

    void BulletDamage()
    {
        this.healthTwo = this.healthTwo - 35;
    }
    
    void Die()
	{
        Destroy(this.GetComponent<Collider>());
        Destroy(aiDirectionHelper);
        Destroy(aiTargetingStack);

        animator.enabled = false;
		navMeshAgent.enabled = false;

		SetRigidBodyState(false);
		SetRigidColliderState(true);

        if (rightHandKineticBlade != null)
        {
            rightHandKineticBlade.Disintegrate();
            Destroy(rightHandKineticBlade.gameObject);
        }

        CancelInvoke();
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

        GetComponent<Rigidbody>().isKinematic = !state;
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