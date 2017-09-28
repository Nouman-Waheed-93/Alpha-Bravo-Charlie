using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {

	public GameObject MainMenu;

	public void Yes(){

		print ("Yes");
		Application.Quit ();

	}

	public void No(){

		gameObject.SetActive (false);
		MainMenu.SetActive (true);

	}
}
