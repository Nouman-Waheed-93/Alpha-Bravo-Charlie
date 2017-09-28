using UnityEngine;
using System.Collections;

public class Mission_Objective_Trigger : MonoBehaviour {

	private Globals globs;
	public GameObject Objective1;
	private GUIText text;

	void Awake(){

		globs = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Globals> ();
		text = Objective1.GetComponent<GUIText>();

	}

	void OnTriggerEnter(Collider other){


		if (other.tag == "Player") {
		
			globs.Objective1Completed = true;
			text.color = Color.red;

		}

	}
}
