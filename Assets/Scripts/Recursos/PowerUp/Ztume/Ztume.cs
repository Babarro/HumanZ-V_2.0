using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ztume : MonoBehaviour, IPowerUp {

    public float timeEffect;
    private PlayerZtume playerZtumeScript;
	private PlayerInventory playerInventoryScript;

	bool isSetUp = false;

    private void Start()
    {
    }

	public void SetUp(){
		GameObject player = this.transform.parent.parent.parent.gameObject;
		if (player == null)
			Debug.LogError("Player not founded");

		playerZtumeScript = player.GetComponent<PlayerZtume>();
		if (playerZtumeScript == null)
			Debug.LogError("PlayerZtume not founded");

		playerInventoryScript = player.GetComponent<PlayerInventory> ();
		if (playerInventoryScript == null)
			Debug.LogError ("Playerinventory not founded");
		
		isSetUp = true;
	}

    public void Activate()
    {
		if (!isSetUp)
			return;
        playerZtumeScript.CmdActivateOnServer(timeEffect);
		playerInventoryScript.CmdDestroyPowerUp ();
    }
}
