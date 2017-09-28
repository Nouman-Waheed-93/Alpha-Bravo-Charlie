using UnityEngine;
using System.Collections;

public class AimIk : MonoBehaviour {

	public Transform HipAimPoint;
	public Transform AimPoint;
	public Transform Bullseye;
	public Transform Character;
	private IKControl ikCntrl;
	private Ray AimLine;
	public Transform Gun;
	// Use this for initialization
	void Start () {
	
		ikCntrl = Character.GetComponent<IKControl> ();

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = Bullseye.position - HipAimPoint.position;
		AimLine = new Ray (HipAimPoint.position, direction.normalized);
		Debug.DrawRay (HipAimPoint.position, direction, Color.red);
		print (HipAimPoint.position+" and "+ Bullseye.position);
		ikCntrl.rightHandObj.position = AimLine.GetPoint (0.4f);
		Gun.LookAt (Bullseye.position);

	}
}
