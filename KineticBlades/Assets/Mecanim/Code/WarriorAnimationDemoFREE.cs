using UnityEngine;
using System.Collections;

using UnityEngine.AI;

public class WarriorAnimationDemoFREE : MonoBehaviour 
{
	public float patrolTime = 15; // time in seconds to wait before seeking a new patrol destination
	public float aggroRange = 10; // distance in scene units below which the NPC will increase speed and seek the player
	public Transform[] waypoints; // collection of waypoints which define a patrol area

	int index; // the current waypoint index in the waypoints array
	float speed, agentSpeed; // current agent speed and NavMeshAgent component speed
	Transform player; // reference to the player object transform

	protected Animator animator;
	protected NavMeshAgent navMeshAgent;

	public bool killMe = false;
	
	void Update()
	{
		Vector3 movementVelocity = navMeshAgent.velocity;

		//Apply inputs to animator
		//animator.SetFloat("Input X", movementVelocity.z);
		//animator.SetFloat("Input Z", -(movementVelocity.x));

		if (movementVelocity.x != 0 || movementVelocity.z != 0 )  //if there is some input
		{
			//set that character is moving
			animator.SetBool("Moving", true);
		}
		else
		{
			//character is not moving
			animator.SetBool("Moving", false);
		}
		/*
		if (Input.GetButtonDown("Fire1"))
		{
			animator.SetTrigger("Attack1Trigger");
			if (warrior == Warrior.Brute)
				StartCoroutine (COStunPause(1.2f));
			else if (warrior == Warrior.Sorceress)
				StartCoroutine (COStunPause(1.2f));
			else
				StartCoroutine (COStunPause(.6f));
		}

		//update character position and facing
		UpdateMovement();
		*/

		if (killMe)
		{
			Die();
		}
	}

	public IEnumerator COStunPause(float pauseTime)
	{
		yield return new WaitForSeconds(pauseTime);
	}

	void Hit()
	{
	}

	void FootR()
	{
	}

	void FootL()
	{
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bullet")
		{
			Die();
		} else if (other.tag == "Sword")
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

	void Awake()
	{
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		if (navMeshAgent != null) { agentSpeed = navMeshAgent.speed; }
		player = GameObject.FindGameObjectWithTag("Player").transform;
		index = Random.Range(0, waypoints.Length);

		InvokeRepeating("Tick", 0, 0.5f);

		if (waypoints.Length > 0)
		{
			InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
		}
	}

	void Patrol()
	{
		index = index == waypoints.Length - 1 ? 0 : index + 1;
	}

	void Tick()
	{
		navMeshAgent.destination = waypoints[index].position;
		navMeshAgent.speed = agentSpeed;

		if (player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
		{
			navMeshAgent.destination = player.transform.position;
			navMeshAgent.speed = agentSpeed;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
	}

	private void Start()
	{
		SetRigidBodyState(true);
		SetRigidColliderState(false);

		navMeshAgent = GetComponent<NavMeshAgent>();
		//navMeshAgent.destination = player.position;

		animator = GetComponent<Animator>();
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