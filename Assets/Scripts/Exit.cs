using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public GameObject ConfirmationMsg;

	public void Quit(){

		transform.parent.gameObject.SetActive (false);
		ConfirmationMsg.SetActive (true);
		print ("Clicked:)");

	}
}
