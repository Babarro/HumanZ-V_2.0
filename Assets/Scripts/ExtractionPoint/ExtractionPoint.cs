using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {
			if (other.GetComponent<ZombieHumanController> ().isZombie && other.GetComponent<PlayerSetup>().isLocalPlayer) {
				GMEndGameController.instance.CmdExtractionPointTaken (other.gameObject.name);
			}
		}

	}
}
