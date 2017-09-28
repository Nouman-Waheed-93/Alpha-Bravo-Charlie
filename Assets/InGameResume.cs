using UnityEngine;
using System.Collections;

public class InGameResume : MonoBehaviour {

	public void ResumePressed(){
	
		Cursor.visible = false;
		Time.timeScale = 1.0f;
		transform.parent.gameObject.SetActive (false);

	}
}
