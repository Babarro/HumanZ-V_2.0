using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCollision : NetworkBehaviour {

	ZombieHumanController zhController;
	PlayerInventory playerInventory;

	// Use this for initialization
	void Start () {
		zhController = GetComponent<ZombieHumanController> ();
		if (zhController == null)
			Debug.LogError ("ZombieHumanController not found!");

		playerInventory = GetComponent<PlayerInventory> ();
		if (playerInventory == null)
			Debug.LogError ("PlayerInventory not found!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other){
		if (!isLocalPlayer)
			return;
		
		if (other.tag == "Recurso" && zhController.isZombie) {
			if (Input.GetButtonDown ("Action"))
				CmdPickObject(other.gameObject);
		}
	}

	[Command]
	void CmdPickObject(GameObject go){
		go.GetComponent<IRecurso> ().PickedUp (this.name,playerInventory.HasWeapon(),playerInventory.HasTrap(),playerInventory.HasPowerUp());
	}
}
