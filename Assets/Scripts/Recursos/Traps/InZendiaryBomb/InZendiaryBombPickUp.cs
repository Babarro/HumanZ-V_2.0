using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InZendiaryBombPickUp : NetworkBehaviour, IRecurso {

	[SerializeField]
	GameObject InZendiaryBombPrefab;

	public int uses = 3;

	public void PickedUp(string playerName, bool hasWeapon, bool hasTrap, bool hasPowerUp){
		if (!hasTrap) {
			GameObject inZendiaryBombInstantiate = Instantiate (InZendiaryBombPrefab) as GameObject;
			inZendiaryBombInstantiate.GetComponent<InZendiaryBomb> ().uses = this.uses;
			NetworkServer.Spawn (inZendiaryBombInstantiate);
			GameObject.Find (playerName).GetComponent<PlayerInventory> ().RpcPickedInZendiaryBomb (playerName, inZendiaryBombInstantiate);
			Destroy (this.gameObject);
		}
	}

	public void SetUses (int newUses){
		this.uses = newUses;
	}

}
