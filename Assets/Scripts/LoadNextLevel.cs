using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour {

	public void NxtLvl(){

		int cpltdLvls = PlayerPrefs.GetInt ("CompletedLevels", 0);
		string levelname;

		if (cpltdLvls == 1)
			levelname = "Level2";
		else if (cpltdLvls == 2)
			levelname = "Level3";
		else
			levelname = "Level1";

		Application.LoadLevel (levelname);

	}

}
