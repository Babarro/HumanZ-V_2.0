using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HumanInitialSpawn : NetworkBehaviour {

	public int spawnChoose = 0;

	UIController uiController;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
			return;

		uiController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();

		if (uiController == null)
			Debug.LogError ("UiController not founded!");

		uiController.SetSpawnPointHumanUI (1);
	}
	
	// Update is called once per frame
	void Update () {
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

}
