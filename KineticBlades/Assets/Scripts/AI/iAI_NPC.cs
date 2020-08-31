using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class iAI_NPC : MonoBehaviour
{
    public AI_Team myTeam;

    public bool iAmTeamOne = false;

    // they AI get's targets around the real target from this
    public GameObject aiTargetingStack;

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

    protected void OnApplicationQuit() { CancelInvoke(); }
    protected void OnDestroy() { CancelInvoke(); }

    // Declared virtual so it can be overridden.
    public virtual void ChangeSkin()
    {
        // change the skin on the model within the AI_NPC
    }

    public virtual void NewTargetStackTarget()
    {
        // get a new target for this AI_NPC
    }
}
