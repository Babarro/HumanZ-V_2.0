using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtractionPoint : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {
			if (other.GetComponent<ZombieHumanController> ().isZombie && other.GetComponent<PlayerSetup>().isLocalPlayer) {
				other.gameObject.GetComponent<PlayerEndGameController>().ExtractPointToServer (other.name);
			}
		}

	}
		
}
