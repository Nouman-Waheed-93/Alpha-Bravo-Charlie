using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum TypesOfWeapons{Heavy,Small,Melee};
	public int bulletsPerClip;
	public int maxClips;
	public float firingRate;
	public string weaponName;
	public int weaponRange;
	public int noiseRadius;
	public bool hasScope;
	public TypesOfWeapons weaponType;
	public string holsterPlace;
	public float holsterXRot, holsterYRot, holsterZRot;
	public float holsterXPos, holsterYPos, holsterZPos;
	public float inHandXRotation, inHandYRotation, inHandZRotation;
	public float inHandX, inHandY, inHandZ;

	private int remainingBulletsInClip;
	private int remainingClips;
	private float timer;
	private float reloadTime = 40f;

	//private LineRenderer line;

	void Awake(){

		remainingBulletsInClip = bulletsPerClip;
		remainingClips = maxClips;
		//line = GetComponent<LineRenderer> ();

	}

	void Update(){

		reloadTime += Time.deltaTime;
		timer += Time.deltaTime;

		Debug.DrawRay (transform.position, transform.forward * weaponRange);

	//	line.SetPosition (0, transform.position);

	//	Ray bullet = new Ray(transform.position, transform.forward);
	
	//	line.SetPosition (1, bullet.GetPoint(100));

	}

	public bool useWeapon(){

		if (weaponType != TypesOfWeapons.Melee) {
			if (remainingBulletsInClip > 0) {
				if (timer > firingRate && reloadTime > 3f) {
					remainingBulletsInClip--;
					timer = 0f;
					//instantiate bullets here and play audio clip
					Ray bullet = new Ray (transform.position, transform.forward);
					RaycastHit hit;
					if (Physics.Raycast (bullet, out hit, weaponRange)) {

						if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy" | hit.transform.tag == "Spy")
							hit.transform.GetComponent<Movement_Handler> ().Die ();
					}
					print ("Remaining Bullets : "+remainingBulletsInClip);
					AudioSource AS = GetComponent<AudioSource> ();
					AS.Play ();
				}
				return true;
			} else {
		
				return false;

			}
		} else {
		
			if(timer > firingRate){

				timer = 0f;
				Ray bullet = new Ray (transform.position, transform.forward);
				RaycastHit hit;
				if (Physics.Raycast (bullet, out hit, weaponRange)) {
					
					if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy")
						hit.transform.GetComponent<Movement_Handler> ().Die ();

				}
				AudioSource AS = GetComponent<AudioSource> ();
				AS.Play ();
				return true;

			}
			return false;

		}

	}

	public bool Reload(){

		if (remainingClips > 0) {
			remainingClips--;
			reloadTime = 0f;
			remainingBulletsInClip = bulletsPerClip;
			//play audio clip
			return true;
		} else
			return false;

	}

}
