using UnityEngine;
using System.Collections;

public class Movement_Handler : MonoBehaviour {

	private enum Stances
	{
		Stand,
		Crouch,
		Prone
	};

	private Animation_Handler anim;
	private float speed = 1;
	private float WalkSpeed = 1f;
	private float CrouchWalkSpeed = 0.8f;
	private float CrawlSpeed = 0.5f;
	private Stances currStance;
	private CharacterController charController;
	private float gravity = 100f;
	private float FootStepNoiseRadius = 10f;
	private Globals globs;

	public float StandColliderYPos = 0.84f;
	public float CrouchColliderYPos = 0.6f;
	public float ProneColliderYPos = 0.2f;
	public float ProneColliderHeight = 0f;
	public float NormalColliderHeight = 1.58f;
	public float CrouchColliderHeight = 1.05f;
	public bool isAlive = true;

	void Awake(){

		globs = GameObject.FindGameObjectWithTag("GameController").GetComponent<Globals>();
		anim = new Animation_Handler (gameObject.GetComponent<Animator> ());
		currStance = Stances.Stand;
		charController = GetComponent<CharacterController> ();

	}

	public void move(float x, float z){

		Vector3 movePosition = new Vector3();
		movePosition = transform.forward * z;
		movePosition += transform.right * x;

		movePosition.y -= gravity * Time.deltaTime;
		charController.Move (movePosition * speed * Time.deltaTime);

	}

	public float VerticalMovements(float movSpeed, bool run){

		if (movSpeed > 0) {
			if (run && speed < 8 && currStance == Stances.Stand) {
				speed *= 8;
				MakeNoise(FootStepNoiseRadius *2);
			} else if (speed > 1 && currStance == Stances.Stand){
				speed = WalkSpeed;
				MakeNoise(FootStepNoiseRadius);

			}
		} else if (movSpeed < 0)
			speed = WalkSpeed;
		anim.MoveStraight (movSpeed * Mathf.Ceil(speed));
		return movSpeed;
		
	}

	public float HorizontalMovements(float movSpeed){
		
		anim.MoveSideWays (movSpeed * Mathf.Ceil(speed));

		if( movSpeed != 0)
		MakeNoise (FootStepNoiseRadius);

		return movSpeed;

	}

	public void Stance(float stance){
	
		if(stance != 0)
			anim.ChangeStance (stance);

		if ((stance > 0f && currStance == Stances.Prone)||(stance < 0f && currStance == Stances.Stand)) {
			currStance = Stances.Crouch;
			speed = CrouchWalkSpeed;
			charController.height = CrouchColliderHeight;
			charController.center = new Vector3 (0f, CrouchColliderYPos, 0f);
			FootStepNoiseRadius = 0f;
		} else if (stance > 0f && currStance == Stances.Crouch) {
			currStance = Stances.Stand;
			speed = WalkSpeed;
			charController.height = NormalColliderHeight;
			charController.center = new Vector3 (0f, StandColliderYPos, 0f);
			FootStepNoiseRadius = 10f;
		} else if (stance < 0f && currStance == Stances.Crouch) {
			currStance = Stances.Prone;
			speed = CrawlSpeed;
			charController.height = ProneColliderHeight;
			charController.center = new Vector3 (0f, ProneColliderYPos, 0f);
			FootStepNoiseRadius = 5f;
		} 

		print (currStance.ToString ());

	}

	private float currxVal = 0;
	public void rotate(float direction){

		currxVal = Mathf.Lerp (currxVal, direction, 0.2f);
		anim.Turn (currxVal);
		transform.Rotate (new Vector3 (0f, direction /* Time.deltaTime*/, 0f));

	}

	public void Die(){

		globs.DeadSoldiers.Add(this.transform);
		anim.Die ();
		isAlive = false;

	}

	public void MakeNoise(float radius){
		Collider[] otherObjects = Physics.OverlapSphere(transform.position , radius);
		foreach(Collider c in otherObjects){

			if((c.tag == "Enemy" || c.tag == "Player") & c!= GetComponent<Collider>() & !c.isTrigger){
				Vector3 currPos = transform.position;
				currPos.y = 0;
				AISightAndHearing AIP = c.GetComponent<AISightAndHearing>();
				if(AIP){
					if(gameObject.tag == AIP.opponent)
						AIP.HeardSomething(currPos);
				}
			}
		}
	}
}