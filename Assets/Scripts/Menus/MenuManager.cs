using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void _InvasionModeScene(){
		Debug.Log("He pulsado el boton Invasion");
		SceneManager.LoadScene("Lobby");
	}
}
