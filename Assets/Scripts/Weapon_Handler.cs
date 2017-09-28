using UnityEngine;
using System.Collections;

public class Weapon_Handler : MonoBehaviour {

	public GameObject[] weaponsInInventory;
	public bool firing = false;

	private int currWeaponIndex;
	private Weapon[] weaponsObjects;
	private Weapon currWeapon;
	private Animation_Handler anim;
	private ArmIK ik;
	private Movement_Handler mov;
	public Transform ScopeCamera;

	void Start(){

		mov = GetComponent<Movement_Handler> ();
		ik = FindInChildren(transform, "Left Limb IK").GetComponent<ArmIK>();
		weaponsObjects = new Weapon[weaponsInInventory.Length];

		for (int i = 0; i<weaponsInInventory.Length; i++) {
		
			weaponsObjects[i] = weaponsInInventory[i].GetComponent<Weapon>();

		}

		currWeaponIndex = 0;
		currWeapon = weaponsObjects[currWeaponIndex];
		anim = new Animation_Handler(gameObject.GetComponent<Animator> ());
		WeaponTransition (0);

	}

	public void Aim(){
	
		if (currWeapon.hasScope) {
			anim.Aim ();
			Camera TPCamera = FindInChildren (transform.root, "Camera").GetComponent<Camera>();
			if (ScopeCamera.GetComponent<Camera>().enabled) {
				ScopeCamera.GetComponent<Camera>().enabled = false;;
				TPCamera.enabled = true;

			} else {
				ScopeCamera.GetComponent<Camera>().enabled = true;;
				TPCamera.enabled = false;

			}

		}
	}

	void Update(){
	
		if (!mov.isAlive) {
			
			ik.enabled = false;
			
		}

	}

	public void useWeapon(bool pressed){

		if (pressed) {
			
			if(currWeapon.useWeapon()){
				firing = true;
				anim.Shoot(true);
				mov.MakeNoise(currWeapon.noiseRadius);
			}
			else{
				firing = false;
				Reload();
				anim.Shoot(false);
			}
			
		} else {

			firing = false;
			anim.Shoot(false);
			
		}

	}

	public void Reload(){

		if (currWeapon.Reload ()) {

			ik.enabled = false;
			anim.Reload ();
			StartCoroutine(Delay());
			
		}
	}

	IEnumerator Delay() {
		print(Time.time);
		yield return new WaitForSeconds(5);
		ik.enabled = true;
		print(Time.time);
	}

	public void changeWeapon(int sign){

		if(sign != 0f){
			print (currWeaponIndex + sign);
			if((currWeaponIndex + sign) > -1 && (currWeaponIndex + sign)< weaponsInInventory.Length){
				WeaponTransition(sign);
			}
		}
		
	}

	private void WeaponTransition(int sign){

		Transform holsterPlace = FindInChildren(transform, currWeapon.holsterPlace);
		Transform hand = FindInChildren (transform, "RightHand");
		currWeaponIndex += sign;
		currWeapon.transform.parent = holsterPlace;
	//	currWeapon.transform.SetParent (holsterPlace);
		currWeapon.transform.localEulerAngles = new Vector3 (currWeapon.holsterXRot, currWeapon.holsterYRot, currWeapon.holsterZRot);
		currWeapon.transform.localPosition = new Vector3(currWeapon.holsterXPos,currWeapon.holsterYPos,currWeapon.holsterZPos);
		currWeapon = weaponsObjects[currWeaponIndex];
		ik.IsActive = false;
		anim.DrawWeapon(currWeapon);
		currWeapon.transform.parent = hand;
		currWeapon.transform.localEulerAngles = new Vector3 (currWeapon.inHandXRotation, currWeapon.inHandYRotation, currWeapon.inHandZRotation);
			currWeapon.transform.localPosition = new Vector3 (currWeapon.inHandX, currWeapon.inHandY, currWeapon.inHandZ);
		if (currWeapon.weaponType == Weapon.TypesOfWeapons.Heavy)
			SetIKTarget ();
		
	}

	private void SetIKTarget(){

		Transform leftgrip = FindInChildren(currWeapon.transform, "Left Grip");
		ik.Target = leftgrip;
		ik.IsActive = true;

	}

	public Transform FindInChildren(Transform TR, string name){

		if (TR.name == name)
			return TR;
		else {
		
			Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
			foreach(Transform t in transforms){

				if(t.name == name)
					return t;

			}
			return null;

		}

	}

	public Weapon GetCurrWeapon(){

		return currWeapon;

	}

}
