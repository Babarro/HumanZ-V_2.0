using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetQuitCameras : MonoBehaviour {

	public CameraMenus cameraMenus;

	void OnEnable(){


		if (gameObject.name == "MainPanel") {
			cameraMenus.CameraLobby.gameObject.SetActive (true);
			cameraMenus.CameraServers.gameObject.SetActive (false);
		} else if (gameObject.name == "ServerListPanel") {
			cameraMenus.CameraLobby.gameObject.SetActive (false);
			cameraMenus.CameraServers.gameObject.SetActive (true);
		}

	}
}
