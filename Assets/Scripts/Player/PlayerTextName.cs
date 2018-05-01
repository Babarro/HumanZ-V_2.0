using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextName : MonoBehaviour {

	Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(cam != null){
			Vector3 v = cam.transform.position;
			transform.LookAt (v);
			transform.Rotate (new Vector3(0,180,0));
		}
	}

	public void SetUpCamera(){
		GameObject[] playersList = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in playersList) {
			if (player.GetComponent<PlayerSetup> ().isLocalPlayer) {
				cam = player.GetComponent<PlayerMotor> ().getCamera ();
				break;
			}
		}
	}
}
