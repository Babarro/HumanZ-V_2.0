using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInZendiaryBomb : NetworkBehaviour {

	[SerializeField]
	private GameObject inZendiaryBomb;

	PlayerMotor playerMotorScript;
	RecibirImpacto recibirImpactoScript;

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
		
		recibirImpactoScript.AddImpact (transform.forward,power);
		StartCoroutine (StunTime(stunTime));
	}

	IEnumerator StunTime(float time){
		//GetComponent<PlayerMotor> ().stuned = true;
		playerMotorScript.stuned = true;
		yield return new WaitForSeconds (time);
		//GetComponent<PlayerMotor> ().stuned = false;
		playerMotorScript.stuned = false;
	}
}
