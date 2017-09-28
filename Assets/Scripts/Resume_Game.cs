using UnityEngine;
using System.Collections;

public class Resume_Game : MonoBehaviour {

	public string level;

	public void Resume(){

		int CompletedLevels = PlayerPrefs.GetInt("CompletedLevels",0);

		if ((level == "Level1") || (level == "Level2" && CompletedLevels == 1) || (level == "Level3" && CompletedLevels == 2))
			Application.LoadLevel (level);
		else
			GetComponent<Exit> ().Quit ();

	}

}
