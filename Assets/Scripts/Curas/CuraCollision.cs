using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CuraCollision : NetworkBehaviour {

	bool taken = true;

	[SerializeField]
	GameObject lightAllo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {
			if (other.GetComponent<ZombieHumanController> ().isZombie && other.GetComponent<PlayerSetup>().isLocalPlayer && !taken) {
				other.GetComponent<PlayerHp> ().SetStartTime ();
				//meshRenderer.material.color = Color.red;
				lightAllo.SetActive(false);
				taken = true;
			}
		}

	}

	[ClientRpc]
	public void RpcEnableCura(){
		taken = false;
		//meshRenderer.material.color = Color.green;
		lightAllo.SetActive(true);
	}

	[ClientRpc]
	public void RpcDisableCura(){
		taken = true;
		//meshRenderer.material.color = Color.red;
		lightAllo.SetActive(false);
	}
}
