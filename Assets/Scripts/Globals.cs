using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

	public ArrayList DeadSoldiers;
	public bool Objective1Completed;
	public bool Objective2Completed;

	void Awake(){

		Objective1Completed = false;
		Objective2Completed = false;
		DeadSoldiers = new ArrayList();
		AudioListener.volume = PlayerPrefs.GetFloat ("Game Volume");

	}

}
