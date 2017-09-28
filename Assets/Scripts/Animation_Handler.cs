using UnityEngine;
using System.Collections;

public class Animation_Handler {

	Animator anim;

	private static int Vertical = Animator.StringToHash("Vertical");
	private static int Horizontal = Animator.StringToHash("Horizontal");
	private static int Turning = Animator.StringToHash("Turn");
	private static int Firing = Animator.StringToHash("Firing");
	private static int Reloading = Animator.StringToHash("Reloading");
	private static int Aiming = Animator.StringToHash("Aim");
	private static int RaiseStance = Animator.StringToHash("RaiseStance");
	private static int LowerStance = Animator.StringToHash("LowerStance");
	private static int HeavyWeapon = Animator.StringToHash("HoldingHeavyWeapon");
	private static int HandGun = Animator.StringToHash("HoldingHandGun");
	private static int MeleeWeapon = Animator.StringToHash("HoldingMeleeWeapon");
	private static int Dead = Animator.StringToHash("Dead");
	private static int Standing = Animator.StringToHash("Base Layer.Standing Locomotion");
	private static int Prone = Animator.StringToHash("Base Layer.Prone Locomotion");

	public Animation_Handler(Animator anim){

		this.anim = anim;

	}

	public void MoveStraight(float speed){

		anim.SetFloat (Vertical, speed, 0.1f, Time.deltaTime);

	}

	public void MoveSideWays(float speed){

		anim.SetFloat (Horizontal, speed, 0.1f, Time.deltaTime);

	}

	public void Turn(float speed){

		anim.SetFloat (Turning, speed);

	}

	public void Shoot(bool pressed){

		anim.SetBool (Firing, pressed); 

	}

	public void Reload(){

		anim.SetTrigger (Reloading);

	}

	public void Aim(){

		anim.SetTrigger(Aiming);

	}

	public void ChangeStance(float stance){

		if (stance > 0 && anim.GetCurrentAnimatorStateInfo (0).nameHash != Standing)
			anim.SetTrigger (RaiseStance);
		else if (stance < 0 && anim.GetCurrentAnimatorStateInfo (0).nameHash != Prone)
			anim.SetTrigger (LowerStance);

	}

	public void DrawWeapon(Weapon weapon){

		if (weapon.weaponType == Weapon.TypesOfWeapons.Heavy)
			anim.SetTrigger (HeavyWeapon);
		else if (weapon.weaponType == Weapon.TypesOfWeapons.Small)
			anim.SetTrigger (HandGun);
		else if (weapon.weaponType == Weapon.TypesOfWeapons.Melee)
			anim.SetTrigger (MeleeWeapon);

	}

	public void Die(){

		anim.SetLayerWeight (1, 0f);
		anim.SetTrigger(Dead);

	}

}
