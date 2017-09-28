using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {

	public GameObject showMenu;
	
	public void ShowHideOptions(){
		
		transform.root.gameObject.SetActive (false);
		showMenu.SetActive (true);
		print ("Clicked:)");
		
	}

}
