using UnityEngine;
using System.Collections;

public class Menu_Handler : MonoBehaviour {

	public GameObject Menu;
	public GameObject Options;
	public GameObject ExitMenu;
	public GameObject Map;

	void Start(){

		Cursor.visible = false;
		Menu.SetActive (false);

	}

	void Update(){

		if (Input.GetKeyDown("escape")) {

			if(Cursor.visible == false){
				Cursor.visible = true;
				Time.timeScale = 0.0f;
				Menu.SetActive(true);

			}
			else{
				Cursor.visible = false;
				Time.timeScale = 1.0f;
				Menu.SetActive(false);
				Options.SetActive(false);
				ExitMenu.SetActive(false);
				Map.SetActive(false);
			}
		}

	}
}
