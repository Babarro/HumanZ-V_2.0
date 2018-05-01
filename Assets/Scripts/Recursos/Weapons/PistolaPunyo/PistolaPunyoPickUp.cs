using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PistolaPunyoPickUp : NetworkBehaviour,IRecurso {

	[SerializeField]
	GameObject pistolaPunyoPref;

	public int uses = 5;

	// Use this for initialization
	void Start () {
		
	}

	public void PickedUp(string name, bool hasWeapon, bool hasTrap, bool hasPowerUp){
		if (!hasWeapon) {
			GameObject pistolaPunyoInstantiate = Instantiate (pistolaPunyoPref) as GameObject;
			pistolaPunyoInstantiate.GetComponent<PistolaPunyo> ().uses = uses;
			NetworkServer.Spawn (pistolaPunyoInstantiate);
			GameObject.Find (name).GetComponent<PlayerInventory> ().RpcPickedPistolaPunyo (name, pistolaPunyoInstantiate);
			Destroy (this.gameObject);
		}
	}

	public void SetUses (int newUses){
		this.uses = newUses;
	}
}
