using UnityEngine;
using System.Collections;

public class CharacterHandler : MonoBehaviour {

	private int currCharacter = 0;
	private CommandHandler comhdlr;
	public Transform[] AllCharacters;

	void Start(){

		comhdlr = GetComponent<CommandHandler>();
		SwitchCharacter (1);

	}

	void Update(){
	
		if (Input.GetButtonDown ("Soldier1")) 
			SwitchCharacter (0);
		if (Input.GetButtonDown ("Soldier2"))
			SwitchCharacter (1);
		if (Input.GetButtonDown ("Soldier3"))
			SwitchCharacter (2);

	}

	public void SwitchCharacter(int charIndex){

		if( charIndex != currCharacter){
			//Disabling Player Control on previous Character
			AllCharacters [currCharacter].GetComponent<InputHandler> ().enabled = false;
			//Enabling AI Control on previous character
			AllCharacters [currCharacter].GetComponent<AIDecisionMaker> ().enabled = true;
			AllCharacters [currCharacter].GetComponent<AISightAndHearing> ().enabled = true;
			AllCharacters [currCharacter].GetComponent<AIAnimationAdapter> ().enabled = true;
			AllCharacters [currCharacter].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
			//Disable Camera of previous Character
			AllCharacters[currCharacter].Find("Camera").gameObject.SetActive(false);
			//Changing Curr Character
			currCharacter = charIndex;
			//Enabling Player Control on Current Character
			AllCharacters [currCharacter].GetComponent<InputHandler> ().enabled = true;
			//Disabling AI Control on Current character
			AllCharacters [currCharacter].GetComponent<AIDecisionMaker> ().enabled = false;
			AllCharacters [currCharacter].GetComponent<AISightAndHearing> ().enabled = false;
			AllCharacters [currCharacter].GetComponent<AIAnimationAdapter> ().enabled = false;
			AllCharacters [currCharacter].GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
			//Enable Camera of current Character
			GameObject cam = AllCharacters[currCharacter].Find("Camera").gameObject;
			cam.SetActive(true);
			comhdlr.cam = cam.GetComponent<Camera>();
			comhdlr.LeaderSpeach = AllCharacters[currCharacter].GetComponent<Speach>();

		}
	}

}
