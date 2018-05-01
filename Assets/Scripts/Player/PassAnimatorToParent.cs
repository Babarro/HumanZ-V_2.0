using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassAnimatorToParent : MonoBehaviour {

	public RuntimeAnimatorController myAnimatorController;
	public Avatar myAvatar;
	private Animator myParentAnimator;

	void OnEnable(){
		
		//Pasar al padre el animator del hijo activo
		myParentAnimator = GetComponentInParent<Animator>();
		myParentAnimator.runtimeAnimatorController = myAnimatorController;
		myParentAnimator.avatar = myAvatar;

		//Debug.Log("Paso el animator " + myAnimatorController.name + "a mi padre");
	}
}
