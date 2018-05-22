using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInventory : NetworkBehaviour {

	/*
	[SerializeField]
	GameObject pistolaPunyoPickUp, inZendiaryBombPickUp;

	public float offsetRadiusOfDroppedRecurso = 3.0f;
	*/
	PlayerMotor playerMotor;

	[SerializeField]
	GameObject pistolaPunyoGraphics;
	[SerializeField]
	GameObject inZendiaryBombGraphics;

	public bool isWeapon = true;
	GameObject weaponsGraphics;
	GameObject trapsGraphics;
	IActivation weaponsScript;
	IActivation trapsScript;

	[SerializeField]
	GameObject weaponsInventory, trapsInventory, powerUpInventory;

	Animator animator;

	UIInventoryController uiInventoryController;

	[SerializeField]
	Sprite pistolaPunyoSprite, inzendiaryBombSprite, ztumeSprite;

	// Use this for initialization
	void Start () {
		playerMotor = GetComponent<PlayerMotor> ();
		animator = GetComponent<Animator> ();
		if (playerMotor == null)
			Debug.LogError ("PlayerMotor not founded!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool HasWeapon(){
		return weaponsScript != null;
	}

	public bool HasTrap(){
		return trapsScript != null;
	}

	public bool HasPowerUp(){
		return powerUpInventory.transform.childCount != 0;
	}

	[Command]
	public void CmdActivateRecurso(){
			RpcActivateRecurso ();
	}

	[ClientRpc]
	public void RpcActivateRecurso(){
		if (isWeapon) {
			if (weaponsScript != null)
				weaponsScript.Activate ();
		} else {
			if (trapsScript != null) {
				playerMotor.stuned = true;
				//Activar anim
				animator.SetBool("ColocandoTrampa", true);
				if(isLocalPlayer)
					uiInventoryController.StartTimer (((ITrap)trapsScript).GetTimeToPlace());
				Invoke ("PlacingTrap", ((ITrap)trapsScript).GetTimeToPlace());
			}

		}
	}

	void PlacingTrap(){
		//Colocar trampa
		animator.SetTrigger("ColocarTrampa");
		animator.SetBool ("ColocandoTrampa", false);
		if(isLocalPlayer)
			uiInventoryController.StopTimer ();
		playerMotor.stuned = false;
		trapsScript.Activate ();
	}

	[Command]
	public void CmdDeactivateRecurso(){
		RpcDeactivateRecurso ();
	}

	[ClientRpc]
	public void RpcDeactivateRecurso(){
		if (isWeapon) {
			if (weaponsScript != null)
				weaponsScript.Deactivate ();
		} else {
			if (trapsScript != null) {
				playerMotor.stuned = false;
				if(isLocalPlayer)
					uiInventoryController.StopTimer ();
				//Cancelar anim trampa
				animator.SetBool("ColocandoTrampa", false);
				CancelInvoke ();
			}
		}
	}

	[Command]
	public void CmdActivatePowerUp(){
		RpcActivatePowerUp ();
	}

	[ClientRpc]
	public void RpcActivatePowerUp(){
		if (powerUpInventory.transform.childCount <= 0)
			return;
		powerUpInventory.transform.GetChild (0).GetComponent<IPowerUp>().Activate();
	}

	[Command]
	public void CmdDestroyPowerUp(){
			
		RpcDestroyPowerUp ();

	}

	[ClientRpc]
	public void RpcDestroyPowerUp(){
		if (isLocalPlayer)
			uiInventoryController.DestroyPowerUp ();
		
		if (powerUpInventory.transform.childCount <= 0)
			return;
		Destroy(powerUpInventory.transform.GetChild (0).gameObject);
	}

	/*
	 * Dropeo de items, habria que cambiarlo puesto que esta preparado para un inventario de 5 puestos (2armas/2tràmpas/1Powerup)
	[Command]
	public void CmdDropRecurso(){

		float offsetX = Random.Range (-offsetRadiusOfDroppedRecurso,offsetRadiusOfDroppedRecurso);
		float offsetZ = Random.Range (-offsetRadiusOfDroppedRecurso,offsetRadiusOfDroppedRecurso);
		Vector3 offsetVector = new Vector3 (offsetX,0.0f,offsetZ);
		GameObject recursoInstantiate = null;
		int remainingUses;

		if (isWeapon) {
			remainingUses = weaponsScript [0].GetUses ();
			if (weaponsScript [0].GetType() == typeof(PistolaPunyo)) {
				recursoInstantiate = Instantiate (pistolaPunyoPickUp, transform.position+offsetVector,Quaternion.identity) as GameObject;
			}
		} else {
			remainingUses = trapsScript [0].GetUses ();
			if (trapsScript [0].GetType() == typeof(InZendiaryBomb)) {
				recursoInstantiate = Instantiate (inZendiaryBombPickUp, transform.position+offsetVector,Quaternion.identity) as GameObject;
			}
		}

		recursoInstantiate.GetComponent<IRecurso> ().SetUses (remainingUses);
		NetworkServer.Spawn (recursoInstantiate);
	}

	[Command]
	public void CmdDropSpecificRecurso(bool isWeaponEnter, int index){
		float offsetX = Random.Range (-offsetRadiusOfDroppedRecurso,offsetRadiusOfDroppedRecurso);
		float offsetZ = Random.Range (-offsetRadiusOfDroppedRecurso,offsetRadiusOfDroppedRecurso);
		Vector3 offsetVector = new Vector3 (offsetX,0.0f,offsetZ);
		GameObject recursoInstantiate = null;
		int remainingUses;

		if (isWeaponEnter) {
			remainingUses = weaponsScript [index].GetUses ();
			if (weaponsScript [index].GetType() == typeof(PistolaPunyo)) {
				recursoInstantiate = Instantiate (pistolaPunyoPickUp, transform.position+offsetVector,Quaternion.identity) as GameObject;
			}
		} else {
			remainingUses = trapsScript [index].GetUses ();
			if (trapsScript [index].GetType() == typeof(InZendiaryBomb)) {
				recursoInstantiate = Instantiate (inZendiaryBombPickUp, transform.position+offsetVector,Quaternion.identity) as GameObject;
			}
		}

		recursoInstantiate.GetComponent<IRecurso> ().SetUses (remainingUses);
		NetworkServer.Spawn (recursoInstantiate);
	}
	*/

	[ClientRpc]
	public void RpcDestroyActualRecurso(string name){
		if(this.name == name){
			if (isWeapon) {
				weaponsGraphics.SetActive (false); 
				weaponsGraphics = null;

				if (isLocalPlayer)
					uiInventoryController.DestroyWeapon1 ();
				
				Destroy (weaponsScript.GetGameObject());
				weaponsScript = null;
				Destroy (weaponsInventory.transform.GetChild (0).gameObject);
			} else {
				trapsGraphics.SetActive (false);
				trapsGraphics = null;

				if (isLocalPlayer)
					uiInventoryController.DestroyTrap1 ();

				Destroy (trapsScript.GetGameObject());
				trapsScript =  null;
				Destroy (trapsInventory.transform.GetChild (0).gameObject);
			}
		}
	}

	[Command]
	public void CmdDestroyActualRecurso(string name){
		if(this.name == name)
			RpcDestroyActualRecurso (name);
	}

	public void DestroyAllItems(){
		if (weaponsGraphics != null)
			weaponsGraphics.SetActive (false);
		if (trapsGraphics != null)
			trapsGraphics.SetActive (false);
		if (weaponsScript != null) {
			weaponsScript = null;
		if (trapsScript != null) 
			trapsScript = null;
		
		if(weaponsInventory.transform.GetChild(0) != null){
			Destroy (weaponsInventory.transform.GetChild (0).gameObject);
		}
		if(trapsInventory.transform.GetChild(0) != null){
			Destroy (trapsInventory.transform.GetChild (0).gameObject);
		}
		if(powerUpInventory.transform.GetChild(0) != null)
			Destroy (powerUpInventory.transform.GetChild (0).gameObject);
		}

		if (isLocalPlayer) {
			uiInventoryController.DestroyPowerUp ();
			uiInventoryController.DestroyTrap1 ();
			uiInventoryController.DestroyWeapon1 ();
			uiInventoryController.NothingInUse ();
		}
	}

	[ClientRpc]
	public void RpcPickedPistolaPunyo(string name, GameObject pistolaPunyoInstantiate){

		if (this.name == name) {
			pistolaPunyoGraphics.SetActive (true);
			weaponsGraphics = pistolaPunyoGraphics;
			weaponsScript = pistolaPunyoInstantiate.GetComponent<IActivation> ();
			if (!isWeapon) {
				if (trapsGraphics != null)
					trapsGraphics.SetActive (false);
				isWeapon = true;
			}

			//UI
			if (isLocalPlayer) {
				uiInventoryController.SetWeapon1 (pistolaPunyoSprite, pistolaPunyoInstantiate.GetComponent<IActivation> ().GetUses ());
				uiInventoryController.SetInUse (isWeapon);
			}
			
			pistolaPunyoInstantiate.transform.parent = weaponsInventory.transform;
			pistolaPunyoInstantiate.GetComponent<PistolaPunyo> ().SetUp ();

		}

	}

	[ClientRpc]
	public void RpcPickedInZendiaryBomb(string name, GameObject inZendiaryBombInstantiate){

		if (this.name == name) {
			
			inZendiaryBombGraphics.SetActive (true);
			trapsGraphics = inZendiaryBombGraphics;
			trapsScript = inZendiaryBombInstantiate.GetComponent<IActivation> ();
			if (isWeapon) {
				if (weaponsGraphics != null)
					weaponsGraphics.SetActive (false);
				isWeapon = false;
			}

			//UI
			if (isLocalPlayer) {
				uiInventoryController.SetTrap1 (inzendiaryBombSprite, inZendiaryBombInstantiate.GetComponent<IActivation> ().GetUses ());
				uiInventoryController.SetInUse (isWeapon);
			}

			inZendiaryBombInstantiate.transform.parent = trapsInventory.transform;
		}

	}

	[ClientRpc]
	public void RpcPickedPowerUp(string name, GameObject powerUpInstantiate){

		if (this.name == name) {
			powerUpInstantiate.transform.parent = powerUpInventory.transform;
			powerUpInstantiate.GetComponent<IPowerUp> ().SetUp ();

			//UI
			if (isLocalPlayer)
				uiInventoryController.SetPowerUp(ztumeSprite);

		}

	}

	[Command]
	public void CmdPressL1(){
		RpcPressL1 ();
	}

	[Command]
	public void CmdPressR1(){
		RpcPressR1 ();
	}

	[ClientRpc]
	public void RpcPressL1(){
		if(!isWeapon){
			if(trapsGraphics!=null)
				trapsGraphics.SetActive (false);
			if(weaponsGraphics!=null){
				weaponsGraphics.SetActive (true);
				Debug.Log("Activar Layer Weapon");
				animator.SetLayerWeight (1, 1f); //Layer Weapon
				animator.SetLayerWeight (2, 0f); //Layer Traps
			}
			isWeapon = true;
		}
		if(isLocalPlayer)
			uiInventoryController.SetInUse (isWeapon);
	}

	[ClientRpc]
	public void RpcPressR1(){
		if (isWeapon) {
			if(weaponsGraphics!=null)
				weaponsGraphics.SetActive (false);
			if (trapsGraphics != null) {
				trapsGraphics.SetActive (true);
				Debug.Log("Activar Layer Traps");
				animator.SetLayerWeight (1, 0f); //Layer Weapon
				animator.SetLayerWeight (2, 1f); //Layer Traps
			}
			isWeapon = false;
		}
		if(isLocalPlayer)
			uiInventoryController.SetInUse (isWeapon);
	}
		
	public void ActualRecursoLessUses(){
		if (isLocalPlayer) {
			if (isWeapon) {
				uiInventoryController.SetWeapon1 (uiInventoryController.GetWeapon1Uses () - 1);
			} else {
				uiInventoryController.SetTrap1 (uiInventoryController.GetTrap1Uses () - 1);
			}
		}
	}

	[ClientRpc]
	public void RpcSetup(){

		if (!isLocalPlayer)
			return;

		uiInventoryController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIInventoryController> ();

		if (uiInventoryController == null)
			Debug.LogError ("UIInventoryController not founded!");

	}
}
