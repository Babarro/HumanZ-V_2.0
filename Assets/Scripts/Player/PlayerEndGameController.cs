using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerEndGameController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	[ClientRpc]
	public void RpcWinLooseHud(string nameWinner){
		if (!isLocalPlayer)
			return;

		UIController ui = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController>();
		Debug.Log (this.gameObject.transform.name+" - "+nameWinner);
		if (this.gameObject.transform.name == nameWinner) {
			ui.WinGame ();
		} else {
			ui.LooseGame ();
		}
	}

	public void ExtractPointToServer(string nameIn){
		CmdExtractionPointTaken (nameIn);
	}

	[Command]
	public void CmdExtractionPointTaken(string playerName){
		GameObject[] playersList = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in playersList) {
			player.GetComponent<PlayerEndGameController> ().RpcWinLooseHud (playerName);
		}
	}

}
