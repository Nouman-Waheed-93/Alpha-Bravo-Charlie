using UnityEngine;
using System.Collections;

public class CommandHandler : MonoBehaviour {

	public Transform[] SoldiersTransforms;
	public enum Soldiers{All, Soldier1, Soldier2, Soldier3};
	public Speach LeaderSpeach;

	private Soldiers CommandTo = Soldiers.All;
	private AIDecisionMaker aidm;
	private AIDecisionMaker[] allDMs;
	private AudioSource adiosrc;
	private const float nextCommandTIme = 3;
	private float cmndTimer=0;
	public Camera cam;

	void Start(){

		adiosrc = GetComponent<AudioSource> ();
		allDMs = new AIDecisionMaker[SoldiersTransforms.Length];
		for(int i = 0; i < SoldiersTransforms.Length; i++)
		 allDMs[i] = SoldiersTransforms[i].GetComponent<AIDecisionMaker>(); 

	}
	void Update(){
	
		cmndTimer += Time.deltaTime;

	}

	public void CommandSoldiers(Soldiers s){

		if(s == Soldiers.All)
			CommandTo = Soldiers.All;
		else if(s == Soldiers.Soldier1){

			CommandTo = Soldiers.Soldier1;
			aidm = allDMs[0];

		}
		else if(s == Soldiers.Soldier2){

			CommandTo = Soldiers.Soldier2;
			aidm = allDMs[1];

		}
		else if(s == Soldiers.Soldier3){

			CommandTo = Soldiers.Soldier3;
			aidm = allDMs[2];

		}

	}

	public void MoveCommand(){

		if (cmndTimer > nextCommandTIme) {
			adiosrc.PlayOneShot (LeaderSpeach.GoTo);
			cmndTimer = 0;
		}
		//AudioSource.PlayClipAtPoint (LeaderSpeach.GoTo, transform.position);

		if(CommandTo == Soldiers.All)
		{
			//if(allDMs[0].holdPosition)
			foreach ( AIDecisionMaker dm in allDMs) 
				dm.GotoPosition(GetCameraTarget());
					
			}

		else {

				aidm.GotoPosition(GetCameraTarget());
				print ("goto " + GetCameraTarget());

			}
		}

	public void HoldCommand(){
	
		if (cmndTimer > nextCommandTIme) {
			adiosrc.PlayOneShot (LeaderSpeach.HoldPos);
			cmndTimer =0;
		}
		//AudioSource.PlayClipAtPoint (LeaderSpeach.HoldPos, transform.position);

		if(CommandTo == Soldiers.All)
		{
			foreach ( AIDecisionMaker dm in allDMs) 
				dm.HoldPosition();
			
		}
		
		else {
			
			aidm.HoldPosition();

		}

	}

		public void FollowMe(){

		if (cmndTimer > nextCommandTIme) {
			adiosrc.PlayOneShot (LeaderSpeach.Follow);
			cmndTimer = 0;
		}
			//AudioSource.PlayClipAtPoint (LeaderSpeach.Follow, transform.position);

			if(CommandTo == Soldiers.All)
			foreach (AIDecisionMaker dm in allDMs) 
			{

				dm.Follow(cam.transform.parent);
				
			}
			else {

				aidm.Follow(cam.transform.parent);
				
			}
		}

		public void RulesOfEngagement(){

		if(CommandTo == Soldiers.All)
		{

			if(!allDMs[0].fireAtWill)
			foreach ( AIDecisionMaker dm in allDMs) 
			{
				if (cmndTimer > nextCommandTIme) {
				adiosrc.PlayOneShot (LeaderSpeach.FireAtWill);
					cmndTimer =0;
				}
				//AudioSource.PlayClipAtPoint (LeaderSpeach.FireAtWill, transform.position);
				dm.FireAtWill();
					
			}
				else {
					foreach (AIDecisionMaker dm in allDMs) 
					{

					if (cmndTimer > nextCommandTIme) {
						adiosrc.PlayOneShot (LeaderSpeach.HoldFire);
						cmndTimer =0;
					}
						//AudioSource.PlayClipAtPoint (LeaderSpeach.HoldFire, transform.position);
						dm.HoldFire();
						
					}
				}			
			}

		else {

				if(aidm.fireAtWill){
				if (cmndTimer > nextCommandTIme) {
					adiosrc.PlayOneShot (LeaderSpeach.FireAtWill);
					cmndTimer =0;
				}
			//	AudioSource.PlayClipAtPoint (LeaderSpeach.FireAtWill, transform.position);
				aidm.HoldFire();
			}
				else {
				if (cmndTimer > nextCommandTIme) {
					adiosrc.PlayOneShot (LeaderSpeach.HoldFire);
					cmndTimer =0;
				}
				//AudioSource.PlayClipAtPoint (LeaderSpeach.HoldFire, transform.position);
				aidm.FireAtWill();
			}
			}

		}

		public Vector3 GetCameraTarget(){

			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay (new Vector3(Screen.width / 2, Screen.height / 2, 10));
			if (Physics.Raycast (ray, out hit, 1000f)) 
			return hit.point;
			else 
			return new Vector3(-1,-1,-1);

		}
	}

