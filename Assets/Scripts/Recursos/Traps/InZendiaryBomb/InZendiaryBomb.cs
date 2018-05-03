using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InZendiaryBomb : NetworkBehaviour, IActivation, ITrap {

    //distancia a la que spawnea la trampa respecto al jugador
    float farFromPlayer = 1.5f;

    //cantidad de veces que puedes colocar esta trampa
    public int uses = 3;

	public float power =300;
	public float stunTime = 5;
	public float timeToPlaceTrap = 2;

    [SerializeField]
    GameObject graphics;

	GameObject player;
	PlayerInZendiaryBomb playerInZendiaryBombScript;

    // Use this for initialization
    void Start () {
		//No funciona y no se porque
		/*
		player = this.transform.parent.parent.parent.gameObject;
		if (player == null)
			Debug.LogError("Player not founded");
		playerInZendiaryBombScript = player.GetComponent<PlayerInZendiaryBomb> ();
		*/
		if (isClient) {
			this.transform.name = "InZendiary Bomb " + GetComponent<NetworkIdentity> ().netId;
		}
    }
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void Deactivate(){
	}

	public void Activate(){
		//mejorar estas dos lineas de codigo en el start (da un error inesperado en start)
		player = this.transform.parent.parent.parent.gameObject;
		playerInZendiaryBombScript = player.GetComponent<PlayerInZendiaryBomb> ();

		Quaternion playerRot = player.transform.rotation;
		Vector3 playerPos = player.transform.position;
		Vector3 direction = player.transform.TransformDirection (new Vector3 (0, 0.1f, 1));
		direction = direction.normalized;
		if(playerInZendiaryBombScript.isLocalPlayer){
			playerInZendiaryBombScript.CmdCreateInZendiaryBomb (new Vector3 (playerPos.x, playerPos.y, playerPos.z) + direction * farFromPlayer, playerRot);
		}
		if (uses > 1) {
			uses--;
			if (playerInZendiaryBombScript.isLocalPlayer) {
				player.GetComponent<PlayerInventory> ().ActualRecursoLessUses();
			}
		} else {
			if (playerInZendiaryBombScript.isLocalPlayer) {
				player.GetComponent<PlayerInventory> ().CmdDestroyActualRecurso (player.name);
			}
		}  
	}

	public int GetUses(){
		return uses;
	}

	public GameObject GetGameObject (){
		return this.gameObject;
	}

	[ClientRpc]
    public void RpcActivateGraphics()
    {
        graphics.SetActive(true);
		GetComponent<BoxCollider> ().enabled = true;	
    }

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerInZendiaryBomb> ().CmdDestroyInZendiaryBomb (this.transform.name);
			collision.gameObject.GetComponent<PlayerInZendiaryBomb> ().InZendiaryBombEffect (power, stunTime);
		}
	}

	public float GetTimeToPlace(){
		return timeToPlaceTrap;
	}

}
