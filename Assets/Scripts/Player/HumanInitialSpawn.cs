using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HumanInitialSpawn : NetworkBehaviour {

	public int spawnChoose = 0;

	UIController uiController;

	bool isSetUp = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSetUp || !isLocalPlayer)
			return;
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			CmdSetSpawnChoose (0);
			uiController.SetSpawnPointHumanUI (1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			CmdSetSpawnChoose (1);
			uiController.SetSpawnPointHumanUI (2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			CmdSetSpawnChoose (2);
			uiController.SetSpawnPointHumanUI (3);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			CmdSetSpawnChoose (3);
			uiController.SetSpawnPointHumanUI (4);
		}
	}

	[Command]
	public void CmdSetSpawnChoose(int choose){
		spawnChoose = choose;
	}

	[ClientRpc]
	public void RpcSetUp(){

		uiController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();

		if (uiController == null)
			Debug.LogError ("UiController not founded!");

		uiController.SetSpawnPointHumanUI (1);
		isSetUp = true;
	}

}
