using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HumanInitialSpawn : NetworkBehaviour {

	public int spawnChoose = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1))
			CmdSetSpawnChoose (0);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			CmdSetSpawnChoose (1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			CmdSetSpawnChoose (2);
		if (Input.GetKeyDown (KeyCode.Alpha4))
			CmdSetSpawnChoose (3);
	}

	[Command]
	public void CmdSetSpawnChoose(int choose){
		spawnChoose = choose;
	}

}
