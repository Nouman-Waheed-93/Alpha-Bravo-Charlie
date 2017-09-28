
using UnityEngine;
using System.Collections;

public class HeadCameraWeapon : MonoBehaviour
{
	public Transform Target;
	public float DefaultDistance = 3f;
	public float xSpeed = 100.0f;
	public float ySpeed = 100.0f;
	public float yMinLimit = -2.0f;
	public float yMaxLimit = 8.0f;
	public Transform Head;
	public float Distance;
	public float DefaultYPos = 1;
	
	private float x;
	private float y;
	private Movement_Handler movHdlr;
	private Weapon_Handler wh;
	private Camera cam;

	void Awake()
	{

		cam = GetComponent<Camera>();
		wh = GetComponentInParent<Weapon_Handler> ();
		movHdlr = transform.root.GetComponent<Movement_Handler> ();
		Distance = DefaultDistance;
		Vector3 angles = transform.eulerAngles;
		x = angles.x;
		y = angles.y;

		if(GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}

	}

	void OnEnable(){
	
		transform.localPosition = new Vector3(0, DefaultYPos, DefaultDistance);

	}
	void LateUpdate()
	{
		if (Time.timeScale != 0) {
			if (!GetComponentInParent<Movement_Handler> ().isAlive) {
		
				this.enabled = false;

			}
			if (Target != null) {
				x += (float)(Input.GetAxis ("Mouse X") * xSpeed * 0.02f);
				y -= (float)(Input.GetAxis ("Mouse Y") * ySpeed * 0.02f);

				if (y > yMaxLimit)
					y = yMaxLimit;
				else if (y < yMinLimit)
					y = yMinLimit;
		
				movHdlr.rotate (Input.GetAxis ("Mouse X") * xSpeed * 0.02f);
				transform.localPosition = new Vector3 (0, y, -Distance);
				transform.LookAt (Target);
			}

			Aim ();
		}
	}

	private void Aim(){
	
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay (new Vector3(Screen.width / 2, Screen.height / 2, 10));
		GameObject weapon = wh.GetCurrWeapon ().gameObject;
		if (Physics.Raycast (ray, out hit, 1000f)) {
		
			if(!(Vector3.Distance(transform.position, hit.point)< 5)){
			weapon.transform.LookAt (hit.point);
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
			}
		}
		else {
			weapon.transform.LookAt (ray.GetPoint (1000));

		}
		Head.LookAt (ray.GetPoint (1000));
	}
}

