using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActivatePowerUp : NetworkBehaviour {

    [SerializeField]
    GameObject powerUp;

    ZombieHumanController zhControllerScript;

	// Use this for initialization
	void Start () {
        zhControllerScript = this.gameObject.GetComponent<ZombieHumanController>();
        if (zhControllerScript == null)
            Debug.LogError("ZombieHumanController script not founded");
	}
	
	// Update is called once per frame
	void Update () {
        if (zhControllerScript.isZombie) {
			if (isLocalPlayer) {
				if (Input.GetButtonDown ("PowerUp")) {
					IPowerUp powerUpScript = powerUp.GetComponentInChildren<IPowerUp> ();
					if (powerUpScript != null)
						powerUpScript.Activate ();
				}
			}
        }
    }
}
