using UnityEngine;
using System.Collections;

public class Volume_Handler : MonoBehaviour {
	
	public void SetVolume(float value){

		PlayerPrefs.SetFloat ("Game Volume", value);
		AudioListener.volume = value;

	}

}
