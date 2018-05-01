using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpPickUp : NetworkBehaviour, IRecurso {

	[SerializeField]
	List<GameObject> powerUpsList;

	int indexPowerUpChoosen;

	// Use this for initialization
	void Start () {
		indexPowerUpChoosen = Random.Range (0, powerUpsList.Count);
	}

	public void PickedUp(string playerName, bool hasWeapon, bool hasTrap, bool hasPowerUp){
		//CmdPick (playerName);
		if (!hasPowerUp) {
			GameObject powerUpInstantiate = Instantiate (powerUpsList [indexPowerUpChoosen]) as GameObject;
			NetworkServer.Spawn (powerUpInstantiate);
			GameObject.Find (playerName).GetComponent<PlayerInventory> ().RpcPickedPowerUp (playerName, powerUpInstantiate);
			Destroy (this.gameObject);
		}
	}

	public void SetUses (int newUses){
	}
}
