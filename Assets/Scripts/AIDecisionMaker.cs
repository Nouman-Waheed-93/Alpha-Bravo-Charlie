using UnityEngine;
using System.Collections;

public class AIDecisionMaker : MonoBehaviour {

	private Vector3 destination;
	private int currWayPointIndex;
	private AISightAndHearing aiPerception;
	private UnityEngine.AI.NavMeshAgent nav;
	private float patrolSpeed = 1f;
	public float patrolWaitTime = 0.5f;	
	private float patrolTimer;
	private Weapon_Handler wep;
	public Weapon currWep;
	private bool shouldPatrol = true;
	private Movement_Handler movHdlr;
	private Transform leader;

	public bool fireAtWill;
	public bool holdPosition;
	public bool follow;
	public Transform[] wayPoints;

	void Awake(){

		movHdlr = GetComponent<Movement_Handler> ();
		aiPerception = GetComponent<AISightAndHearing> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		wep = GetComponent<Weapon_Handler> ();
		currWep = wep.GetCurrWeapon ();
		fireAtWill = true;
		holdPosition = true;
		follow = false;
		currWep = wep.GetCurrWeapon ();

	}

	void Update(){

		if (movHdlr.isAlive) {
			if (aiPerception.opponentInSight) {

				if (fireAtWill || gameObject.tag == 						"Enemy") {

					StartCoroutine(Shoot ());

				}
			} 
			else if (gameObject.tag == "Enemy" & 						shouldPatrol) {
	
				holdPosition = false;
				Patrol ();

			}
			else if(follow){

				destination = leader.position;
				if(Vector3.Distance(leader.position,transform.position) > 10)
					SetNavSpeed(patrolSpeed * 2);
				else
					SetNavSpeed(patrolSpeed);

			}

			MeasureDistance();
			if (destination != new Vector3 (0, 0, 0))
				nav.destination = destination;
	
		}
	}

	IEnumerator Shoot(){

		currWep = wep.GetCurrWeapon ();
		if(nav)
			if(nav.isActiveAndEnabled)
				nav.Stop ();
		if(aiPerception.Target){
			Vector3 opponentPos = aiPerception.Target.position - new Vector3(0, aiPerception.Target.position.y, 0);
			transform.LookAt(opponentPos);
			currWep.transform.LookAt ((aiPerception.Target.position) + aiPerception.Target.up * 0.5f);
			RaycastHit hit;
			if(Physics.Raycast(currWep.transform.position, currWep.transform.forward, out hit, currWep.weaponRange)){

				if(hit.transform.tag == aiPerception.Target.tag)
					if(aiPerception.Target.GetComponent<Movement_Handler>().isAlive)
						wep.useWeapon (true);

			}

		}

		yield return new WaitForSeconds(1);
		wep.useWeapon(false);
		if (movHdlr.isAlive)
			if(nav)
				if(nav.isActiveAndEnabled)
					nav.Resume();

	}

	public void GotoPosition(Vector3 position){

		if(nav)
			if(nav.isActiveAndEnabled)
				nav.Resume ();
		if(position != new Vector3(-1,-1,-1)){
			if(fireAtWill)
				SetNavSpeed(2);
			else
				SetNavSpeed(1);
			holdPosition = false;
			shouldPatrol = false;
			follow = false;
			destination = position;
	}
	}

	public void HoldPosition(){

		follow = false;
		holdPosition = true;
		if(nav)
			if(nav.isActiveAndEnabled)
				nav.Stop ();

	}

	public void FireAtWill(){

		fireAtWill = true;

	}

	public void HoldFire(){

		fireAtWill = false;

	}

	public void Follow(Transform leader){
	
		SetNavSpeed (patrolSpeed);
		follow = true;
		holdPosition = false;
		if (nav)
		if (nav.isActiveAndEnabled)
			nav.Resume ();
		this.leader = leader;

	}

	void Patrol(){

		if(wayPoints.Length > 0)
		if (wayPoints[currWayPointIndex]) {
			SetNavSpeed (patrolSpeed);
			//MeasureDistance();
			destination = wayPoints [currWayPointIndex].position;
		}

	}

	void MeasureDistance(){

		if(nav.remainingDistance <= nav.stoppingDistance)
		{
			
			patrolTimer += Time.deltaTime;			
			
			if(patrolTimer >= patrolWaitTime)
			{

				shouldPatrol = true;
				if(currWayPointIndex == wayPoints.Length - 1)
					currWayPointIndex = 0;
				else
					currWayPointIndex++;	
				
				patrolTimer = 0;
			}

		}
		else	
			patrolTimer = 0;

		}

	private void SetNavSpeed(float speed){

		if (nav)
			nav.speed = speed;

	}

}
