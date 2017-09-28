using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour {

	public bool coverBlown = false;
	public bool canBeSeen = false;

	private Weapon_Handler wepHdlr;
	private float timer;
	private float coverBlownTime = 5f; //Time after which the cover will be restored

	void Awake(){
	
		timer = 0f;
		wepHdlr = GetComponent<Weapon_Handler> ();

	}

	void Update(){

		if (canBeSeen & wepHdlr.firing) {
		
			StartCoroutine("BlowCover");
			timer = 0f;

		} else if(coverBlown & !canBeSeen)
		{

			timer += Time.deltaTime;

		}

		if (timer > coverBlownTime)
			coverBlown = false;

		}

	IEnumerator BlowCover(){

		yield return new WaitForSeconds(1);
		coverBlown = true;

	}

	}
