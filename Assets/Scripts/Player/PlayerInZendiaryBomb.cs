using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInZendiaryBomb : NetworkBehaviour {

	[SerializeField]
	private GameObject inZendiaryBomb;

	PlayerMotor playerMotorScript;
	RecibirImpacto recibirImpactoScript;

	public GameObject pisarTrampaFX;

	// Use this for initialization
	void Start () {
		playerMotorScript = GetComponent<PlayerMotor> ();
		recibirImpactoScript = GetComponent<RecibirImpacto> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Command]
	public void CmdCreateInZendiaryBomb(Vector3 position, Quaternion rotation){
		//gmScript.SpawnInZendiaryBomb (position, rotation);
		GameObject go = Instantiate (inZendiaryBomb, position, rotation) as GameObject;
		NetworkServer.Spawn (go);
		go.GetComponent<InZendiaryBomb> ().RpcActivateGraphics ();
	}

	[Command]
	public void CmdDestroyInZendiaryBomb(string trapName){
		//gmScript.GMInZendiaryBombEffect (this.transform.name, trapName, power,stunTime);
		//GameObject player = GameObject.Find (this.name);
		Destroy(GameObject.Find (trapName));
	}

	public void InZendiaryBombEffect(float power, float stunTime){
		if (!isLocalPlayer)
			return;
		
		recibirImpactoScript.AddImpact (transform.forward,power,0);
		StartCoroutine (StunTime(stunTime));
	}

	IEnumerator StunTime(float time){
		//GetComponent<PlayerMotor> ().stuned = true;
		playerMotorScript.stuned = true;
		yield return new WaitForSeconds (time);
		//GetComponent<PlayerMotor> ().stuned = false;
		playerMotorScript.stuned = false;
	}

	[Command]
	public void CmdTrampaPisada(Vector3 pos)
	{
		RpcDoPisarTrampaEffect (pos);
	}

	[ClientRpc]
	void RpcDoPisarTrampaEffect(Vector3 pos){
		GameObject trampaSP =  Instantiate (pisarTrampaFX, pos, pisarTrampaFX.transform.rotation)as GameObject;
		trampaSP.GetComponent<ParticleSystem> ().Play ();
		Destroy (trampaSP, 1);
	}
}
