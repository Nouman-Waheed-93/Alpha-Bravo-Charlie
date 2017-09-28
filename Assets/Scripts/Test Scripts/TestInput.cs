using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour {

	Animator anim;
	float currVer = 0;
	float currHor = 0;
	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		currVer = Mathf.Lerp (currVer, Input.GetAxisRaw ("Vertical"), 0.2f);
		currHor = Mathf.Lerp (currHor, Input.GetAxisRaw ("Horizontal"), 0.2f);
		anim.SetFloat ("Vertical Movements", currVer);
		anim.SetFloat ("Horizontal Movements", currHor);
		anim.SetBool ("Fire", Input.GetButton ("Fire"));
		anim.SetBool("Aim", Input.GetButton("Aim"));

		if(Input.GetButtonDown("Raise Stance"))
			anim.SetTrigger ("Raise Stance");
		if (Input.GetButtonDown ("Lower Stance"))
			anim.SetTrigger ("Drop Stance");

		if(Input.GetButtonDown("Reload"))
		anim.SetTrigger ("Reload");


	}
}
