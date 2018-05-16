using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDisparoPunyo : NetworkBehaviour {

	[SerializeField]
	PistolaPunyoCollision pistolaPunyoCollisionScript;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

	}

	[Command]
	public void CmdPushPunyo(string enterName, float distance, float power){
		GameObject.Find (enterName).GetComponent<PlayerDisparoPunyo> ().RpcPushPunyo (enterName, distance, power);
	}

	[Command]
	public void CmdPullPunyo(string enterName, float distance){
		GameObject.Find (enterName).GetComponent<PlayerDisparoPunyo> ().RpcPullPunyo (enterName, distance);
	}

	[ClientRpc]
	public void RpcPushPunyo(string enterName, float distance, float enterPower){
		if (this.name == enterName) {
			pistolaPunyoCollisionScript.Push (distance, enterPower);
		}
	}

	[ClientRpc]
	public void RpcPullPunyo(string enterName, float distance){
		if (this.name == enterName) {
			pistolaPunyoCollisionScript.Pull (distance);
		}
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
