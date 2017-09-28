using UnityEngine;
using System.Collections;

public class ArmIK : MonoBehaviour
{
	public Transform UpperArm, Forearm, Hand;
	public Transform Target, ElbowTarget;
	public bool IsActive;
	public float transition = 0.95f;
	public bool Optimize = false;
	
	private Quaternion upperArmStartRotation, forearmStartRotation, handStartRotation;
	private Vector3 targetRelativeStartPosition, elbowTargetRelativeStartPosition;
	
	//helper GOs that are reused every frame
	private GameObject upperArmAxisCorrection, forearmAxisCorrection, handAxisCorrection;
	
	//hold last positions so recalculation is only done if needed
	private Vector3 lastUpperArmPosition, lastTargetPosition, lastElbowTargetPosition;
	
	void Start()
	{
		upperArmStartRotation = UpperArm.rotation;
		forearmStartRotation = Forearm.rotation;
		handStartRotation = Hand.rotation;
		elbowTargetRelativeStartPosition = ElbowTarget.position - UpperArm.position;
		
		//create helper GOs
		upperArmAxisCorrection = new GameObject("upperArmAxisCorrection");
		forearmAxisCorrection = new GameObject("forearmAxisCorrection");
		handAxisCorrection = new GameObject("handAxisCorrection");
		
		//set helper hierarchy
		upperArmAxisCorrection.transform.parent = transform;
		forearmAxisCorrection.transform.parent = upperArmAxisCorrection.transform;
		handAxisCorrection.transform.parent = forearmAxisCorrection.transform;
		
		//guarantee first-frame update
		lastUpperArmPosition = UpperArm.position + 5*Vector3.up;
	}
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	void LateUpdate ()
	{
		if (!IsActive)
			return;
		
		SolveIK();
	}
	
	void SolveIK()
	{
		if(Target == null)
		{
			targetRelativeStartPosition = Vector3.zero;
			return;
		}
		
		if(targetRelativeStartPosition == Vector3.zero && Target != null)
		{
			targetRelativeStartPosition = Target.position - UpperArm.position;
		}
		
		if(Optimize	&&
		   lastUpperArmPosition == UpperArm.position &&
		   lastTargetPosition == Target.position &&
		   lastElbowTargetPosition == ElbowTarget.position)
		{			
			return;
		}
		
		lastUpperArmPosition = UpperArm.position;
		lastTargetPosition = Target.position;
		lastElbowTargetPosition = ElbowTarget.position;
		
		//Calculate ikAngle variable.
		float upperArmLength = Vector3.Distance(UpperArm.position, Forearm.position);
		float forearmLength = Vector3.Distance(Forearm.position, Hand.position);
		float armLength = upperArmLength + forearmLength;
		float hypotenuse = upperArmLength;
		
		float targetDistance = Vector3.Distance(UpperArm.position, Target.position);	
		targetDistance = Mathf.Min(targetDistance, armLength - 0.0001f); //Do not allow target distance be further away than the arm's length.
		
		float adjacent = (hypotenuse*hypotenuse - forearmLength*forearmLength + targetDistance*targetDistance) /(2*targetDistance);		
		float ikAngle  = Mathf.Acos(adjacent/hypotenuse) * Mathf.Rad2Deg;
		
		//Store pre-ik info.
		Vector3 targetPosition = Target.position;
		Vector3 elbowTargetPosition = ElbowTarget.position;
		
		Transform upperArmParent = UpperArm.parent;
		Transform forearmParent = Forearm.parent;
		Transform handParent = Hand.parent; 
		
		Vector3 upperArmScale = UpperArm.localScale;
		Vector3 forearmScale = Forearm.localScale;
		Vector3 handScale = Hand.localScale;
		Vector3 upperArmLocalPosition = UpperArm.localPosition;
		Vector3 forearmLocalPosition = Forearm.localPosition;
		Vector3 handLocalPosition = Hand.localPosition;
		
		Quaternion upperArmRotation = UpperArm.rotation;
		Quaternion forearmRotation = Forearm.rotation;
		
		//Reset arm.
		Target.position = targetRelativeStartPosition + UpperArm.position;
		ElbowTarget.position = elbowTargetRelativeStartPosition + UpperArm.position;
		UpperArm.rotation = upperArmStartRotation;
		Forearm.rotation = forearmStartRotation;
		Hand.rotation = handStartRotation;
		
		//Work with temporaty game objects and align & parent them to the arm.
		transform.position = UpperArm.position;
		transform.LookAt(targetPosition, elbowTargetPosition - transform.position);
		
		upperArmAxisCorrection.transform.position = UpperArm.position;
		upperArmAxisCorrection.transform.LookAt(Forearm.position, UpperArm.up);
		UpperArm.parent = upperArmAxisCorrection.transform;
		
		forearmAxisCorrection.transform.position = Forearm.position;
		forearmAxisCorrection.transform.LookAt(Hand.position, Forearm.up);
		Forearm.parent = forearmAxisCorrection.transform;
		
		handAxisCorrection.transform.position = Hand.position;
		Hand.parent = handAxisCorrection.transform;
		
		//Reset targets.
		Target.position = targetPosition;
		ElbowTarget.position = elbowTargetPosition;	
		
		//Apply rotation for temporary game objects.
		upperArmAxisCorrection.transform.LookAt(Target,ElbowTarget.position - upperArmAxisCorrection.transform.position);
		upperArmAxisCorrection.transform.localRotation = Quaternion.Euler(upperArmAxisCorrection.transform.localRotation.eulerAngles - new Vector3(ikAngle, 0, 0));
		
		forearmAxisCorrection.transform.LookAt(Target,ElbowTarget.position - upperArmAxisCorrection.transform.position);
		handAxisCorrection.transform.rotation = Target.rotation;
		
		//Restore limbs.
		UpperArm.parent = upperArmParent;
		Forearm.parent = forearmParent;
		Hand.parent = handParent;
		UpperArm.localScale = upperArmScale;
		Forearm.localScale = forearmScale;
		Hand.localScale = handScale;
		UpperArm.localPosition = upperArmLocalPosition;
		Forearm.localPosition = forearmLocalPosition;
		Hand.localPosition = handLocalPosition;
		
		//Transition.
		transition = Mathf.Clamp01(transition);
		UpperArm.rotation = Quaternion.Slerp(upperArmRotation, UpperArm.rotation, transition);
		Forearm.rotation = Quaternion.Slerp(forearmRotation, Forearm.rotation, transition);
		Hand.rotation = Target.rotation;		
	}
}
