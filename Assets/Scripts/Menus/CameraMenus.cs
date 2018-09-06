using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenus : MonoBehaviour {

	public Camera CameraLobby;
	public Camera CameraServers;

	// Use this for initialization
	void Awake() {
		SetCamerasRef ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetCamerasRef(){
		CameraLobby = GameObject.FindGameObjectWithTag ("CameraLobby").GetComponent<Camera>();
		CameraServers = GameObject.FindGameObjectWithTag ("CameraServer").GetComponent<Camera>();

		if (CameraLobby == null || CameraServers == null)
			Debug.Log ("No se ha obtenido referencia");
	}

}
