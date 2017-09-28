using UnityEngine;
using System.Collections;

public class AIAnimationAdapter : MonoBehaviour {

	public float angleResponseTime = 0.6f;
	public float deadZone = 5f;

	private Transform opponent;	
	private AISightAndHearing aiSight;
	private UnityEngine.AI.NavMeshAgent nav;	
	private Movement_Handler mov;								

	void Awake(){

		aiSight = GetComponent<AISightAndHearing>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		mov = GetComponent<Movement_Handler>();
	//	nav.updateRotation = false;
	//	nav.updatePosition = false;
		deadZone *= Mathf.Deg2Rad;

	}

	void Update(){
	
		if(mov.isAlive)
		NavAnimSetup();

	}
	
	public void Setup(float speed, float angle)
	{

		float angularSpeed = angle * angleResponseTime;

		bool run = speed > 1 ? true : false;

		mov.VerticalMovements (speed, run);
		mov.rotate (angularSpeed);
	}

	void OnAnimatorMove()
	{

		Animator a = GetComponent<Animator> ();
		if (Time.timeScale != 0) {
			nav.velocity = a.deltaPosition / Time.deltaTime;
			transform.rotation = a.rootRotation;
		}

	}

	void NavAnimSetup ()
	{
		float speed;
		float angle;
		
		if(aiSight.opponentInSight)
		{
			speed = 0f;
			opponent = aiSight.Target;
			
			angle = FindAngle(transform.forward, opponent.position - transform.position, transform.up);
		}
		else
		{
			speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
			
			angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);
			
			if(Mathf.Abs(angle) < deadZone)
			{
				transform.LookAt(transform.position + nav.desiredVelocity);
				angle = 0f;
			}
		}
		
		Setup(speed, angle);
	}
	
	
	float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		if(toVector == Vector3.zero)
			return 0f;
		
		float angle = Vector3.Angle(fromVector, toVector);
		
		Vector3 normal = Vector3.Cross(fromVector, toVector);
		
		angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
		
		angle *= Mathf.Deg2Rad;
		
		return angle;
	}
}
