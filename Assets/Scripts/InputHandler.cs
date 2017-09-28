using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	Movement_Handler mov;
	Weapon_Handler wep;
	CommandHandler cmdHdlr;
	//private bool m_isAxisInUse = false;
	private bool cursorHidden = false;

	// Use this for initialization
	void Awake () {
	
		cmdHdlr = GameObject.Find ("System").GetComponent<CommandHandler>();
		mov = GetComponent<Movement_Handler> ();
		wep = GetComponent<Weapon_Handler> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		bool sprint = Input.GetButton ("Sprint");
		float z = mov.VerticalMovements(Input.GetAxisRaw ("Vertical"),sprint);
		float x = mov.HorizontalMovements (Input.GetAxisRaw ("Horizontal"));
		mov.move (x, z);

			if (Input.GetButtonDown("Raise Stance")) {
			//	if (m_isAxisInUse == false) {
					//float stance = Input.GetAxisRaw ("Change Stance");
					mov.Stance (1);
					//m_isAxisInUse = true;
			//	}
			}
			else if (Input.GetButtonDown ("Lower Stance")) {
			mov.Stance(-1);
			}   

	}

	void Update(){

		if (Time.timeScale != 0) {

			if (!mov.isAlive) {
		
				this.enabled = false;

			}

			if (Input.GetButton ("Fire"))
				wep.useWeapon (true);
			if (Input.GetButtonUp ("Fire"))
				wep.useWeapon (false);


			if (Input.GetButtonDown ("Reload"))
				wep.Reload ();

			if (Input.GetButtonDown ("Next Weapon"))
				wep.changeWeapon (1);
			else if (Input.GetButtonDown ("Previous Weapon"))
				wep.changeWeapon (-1);

			if (Input.GetKey ("escape")) {
				if (cursorHidden)
					Cursor.visible = true;
				else
					Cursor.visible = false;

			}

			if (Input.GetButtonDown ("Aim")) {

				wep.Aim ();

			}

			if (Input.GetButtonDown ("Follow"))
				cmdHdlr.FollowMe ();

			if (Input.GetButtonDown ("Move"))
				cmdHdlr.MoveCommand ();

			if (Input.GetButtonDown ("Hold Fire"))
				cmdHdlr.RulesOfEngagement ();

			if (Input.GetButtonDown ("Hold Position"))
				cmdHdlr.HoldCommand ();

			if (Input.GetButtonDown ("Cmnd Sldr All"))
				cmdHdlr.CommandSoldiers (CommandHandler.Soldiers.All);

			if (Input.GetButtonDown ("Cmnd Sldr 1"))
				cmdHdlr.CommandSoldiers (CommandHandler.Soldiers.Soldier1);

			if (Input.GetButtonDown ("Cmnd Sldr 2"))
				cmdHdlr.CommandSoldiers (CommandHandler.Soldiers.Soldier2);

			if (Input.GetButtonDown ("Cmnd Sldr 3"))
				cmdHdlr.CommandSoldiers (CommandHandler.Soldiers.Soldier3);

		}

	}

	}
