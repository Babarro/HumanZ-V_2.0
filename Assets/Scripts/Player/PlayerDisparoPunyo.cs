using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDisparoPunyo : NetworkBehaviour {

	[SerializeField]
	PistolaPunyoCollision pistolaPunyoCollisionScript;

	[SerializeField]
	Animator plAnimator;
	[SerializeField]
	BoxCollider pistolaPunyoCollider;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

	}

	[Command]
	public void CmdPushPunyo(){
		//GameObject.Find (enterName).GetComponent<PlayerDisparoPunyo> ().RpcPushPunyo (enterName, distance, power);
		RpcPushPunyo ();
	}

	[Command]
	public void CmdPullPunyo(){
		RpcPullPunyo ();
	}

	[ClientRpc]
	public void RpcPushPunyo(){
			//pistolaPunyoCollisionScript.Push (enterPower);
		pistolaPunyoCollider.enabled = true;
		plAnimator.SetTrigger("Shoot");
		// Sonido Disparo Punch Gun
		if(isLocalPlayer)
			FindObjectOfType<AudioManager>().PlaySound("ShootPunchGun");
	}

	[ClientRpc]
	public void RpcPullPunyo(){
		pistolaPunyoCollider.enabled = false;
		// Sonido Recarga Punch Gun
		if(isLocalPlayer)
			FindObjectOfType<AudioManager>().PlaySound("ReloadPunchGun");
	}

    [Command]
	public void CmdPunyoPush(string name, Vector3 direction, float power)
    {
		//gmScript.PistolaPunyoPush(name, direction, power);
		GameObject go = GameObject.Find(name);
		go.GetComponent<RecibirImpacto> ().RpcAddImpact (direction, power, 1);
    }

	[Command]
	public void CmdDestroyWeapon(GameObject go){
		Destroy (go);
	}
}
