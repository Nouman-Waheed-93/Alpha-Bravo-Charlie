using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class Turn_Handler : MonoBehaviour {

	public float sensitivityX = 5F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;

	private Movement_Handler movHdlr;
	
	void Update ()
	{
		if (movHdlr.isAlive) {
			float rotation = Input.GetAxis ("Mouse X") * sensitivityX;
			//transform.Rotate(0, rotation, 0);
			movHdlr.rotate (rotation);
		}
	}
	
	void Start ()
	{

		movHdlr = GetComponent<Movement_Handler> ();
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		
	}
}