using UnityEngine;
using System.Collections;

public class AISightAndHearing : MonoBehaviour {

	public float fieldOfViewAngle = 110f;	
	public bool opponentInSight;	
	public Vector3 personalLastSighting = new Vector3(0f, 0f, 0f);
	public Transform Target;

	private Movement_Handler trgtMvHdlr;
	public string opponent;
	private bool opponentSpy; //to check if the spy is opponent
	private SphereCollider col;
	private AIDecisionMaker decMkr;
	private Spy spy;
	private Globals globs;

	void Awake(){

		globs = GameObject.FindGameObjectWithTag("GameController").GetComponent<Globals>();
		col = GetComponent<SphereCollider> ();
		decMkr = GetComponent<AIDecisionMaker> ();

		if (gameObject.tag == "Player" || gameObject.tag == "Spy") {
			opponent = "Enemy";
			opponentSpy = false;
		} else if (gameObject.tag == "Enemy") {
			opponent = "Player";
			opponentSpy = true;
		}

	}

	void Update(){

		if(trgtMvHdlr){
			if(!trgtMvHdlr.isAlive)
			EmptyTarget();
		}
	
	}


	void OnTriggerStay(Collider other){

		if (other.tag == opponent | (other.tag == "Spy" & opponentSpy)) {
		

			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
				{
					// ... and if the raycast hits the player...
					if(!Target & (hit.collider.tag == opponent | (hit.collider.tag == "Spy" & opponentSpy)) & !globs.DeadSoldiers.Contains(other.transform)){

						if(hit.collider.tag == "Spy"){

						spy = other.GetComponent<Spy>();
							if(spy)
						spy.canBeSeen = true;

						}

						if(hit.collider.tag !="Spy" || spy.coverBlown){

						Target = other.transform;
						trgtMvHdlr = other.GetComponent<Movement_Handler>();
						opponentInSight = true;

						}
					}
				
				}
				else if(other.transform == Target)
					EmptyTarget();
			}
			else if(other.transform == Target){

				EmptyTarget();

			}

		}
	}


	void OnTriggerExit(Collider other){
	
		if (other.transform == Target)
			EmptyTarget();

		if (other.tag == "Spy") {
		
			other.GetComponent<Spy>().canBeSeen = false;

		}

	}

	private void EmptyTarget(){

		Target = null;
		trgtMvHdlr = null;
		opponentInSight = false;

	}

	public void HeardSomething(Vector3 soundPosition){

		if (!decMkr.holdPosition)
		decMkr.GotoPosition (soundPosition);

	}
	
}

