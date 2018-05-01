using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTraps : NetworkBehaviour {

    [SerializeField]
    GameObject traps;

    ZombieHumanController zhControllerScript;

    // Use this for initialization
    void Start () {
        zhControllerScript = this.gameObject.GetComponent<ZombieHumanController>();
        if (zhControllerScript == null)
            Debug.LogError("ZombieHumanController script not founded");
    }
	
	// Update is called once per frame
	void Update () {
        if (zhControllerScript.isZombie)
        {
			if(isLocalPlayer){
	            if (Input.GetButtonDown("Trap"))
	            {
	                ITrap trapScript = traps.GetComponentInChildren<ITrap>();
	                if(trapScript!=null)
	                    trapScript.PlaceTrap();
				}
			}
        }
	}
}
