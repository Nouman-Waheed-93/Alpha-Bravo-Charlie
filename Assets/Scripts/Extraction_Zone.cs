using UnityEngine;
using System.Collections;

public class Extraction_Zone : MonoBehaviour {

	private Globals globs;
	public GameObject Objective1;
	private GUIText text;
	
	void Awake(){
		
		globs = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Globals> ();
		text = Objective1.GetComponent<GUIText>();
		
	}
	
	void OnTriggerEnter(Collider other){
		
		
		if (other.tag == "Player") {
			
			if(globs.Objective1Completed){
				globs.Objective2Completed = true;
				text.color = Color.red;
				int CmpltdLvls = PlayerPrefs.GetInt("CompletedLevels", 0);
				PlayerPrefs.SetInt("CompletedLevels", CmpltdLvls + 1);
				Application.LoadLevel("DebreifScreen");
			}
			
		}
		
	}
}
