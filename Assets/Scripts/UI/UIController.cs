 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : NetworkBehaviour {

	[SerializeField]
	GameObject winPanel;
	[SerializeField]
	GameObject loosePanel;
	[SerializeField]
	GameObject HumanText;
	[SerializeField]
	GameObject ZombieText;
	[SerializeField]
	Text numberOfZombiesLeftUI;
	[SerializeField]
	GameObject chooseSpawnHumanGO;
	[SerializeField]
	Text spawnPosText;

	public GameObject TrampaPanel;
	public GameObject PowerUpPanel;
	public GameObject ArmaPanel;
	public GameObject barraInfeccion;


	private GameObject[] listaPlayers;
	private ZombieHumanController myZombieHumanController; 

	public bool oneZombie = false;

	[SerializeField]
	Text timeTextUI;

	[SyncVar]
	public int gameDuration = 300;

	[SyncVar]
	public float time = 0;

	[SyncVar]
	public float startTime = -1;

	[SyncVar(hook = "OnZombiesLeftChange")]
	public int numberOfZombie = 0;

	[SyncVar]
	public bool endOfGame = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (endOfGame)
			return;

		if (isServer) {
			time += Time.deltaTime; 
			if (gameDuration - (int)(time - startTime) <= 0) {
				if (oneZombie)
					RpcEndGame ();
				else
					GMEndGameController.instance.ExtractionPoint ();
			
				endOfGame = true;
			}
				
		}

		timeTextUI.text = ((gameDuration - (int)(time - startTime))/60).ToString() +":"+ ((gameDuration - (int)(time - startTime))%60).ToString("D2");
	}

	[ClientRpc]
	public void RpcEndGame(){
		bool win = false;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetComponent<PlayerSetup> ().isLocalPlayer) {
				win = players [i].GetComponent<ZombieHumanController> ().isZombie;
				break;
			}
		}

		if (win)
			winPanel.SetActive (true);
		else
			loosePanel.SetActive (true);
	}

	[ClientRpc]
	public void RpcLooseGame(){
		loosePanel.SetActive (true);
	}

	[ClientRpc]
	public void RpcWinGame(){
		if(isLocalPlayer)
			winPanel.SetActive (true);
	}
		
	public void LooseGame(){
		loosePanel.SetActive (true);

		Debug.Log ("loose");
	}
		
	public void WinGame(){
		winPanel.SetActive (true);


		Debug.Log ("win");
	}

	void OnZombiesLeftChange(int numberOfZombie){
		numberOfZombiesLeftUI.text = numberOfZombie.ToString();
	}

	public void ActivarComponenteUI(GameObject UIcomponent){
		UIcomponent.SetActive (true);
	}

	public void DesactivarComponenteUI(GameObject UIcomponent, float delayTime = 0f){
		if(delayTime == 0f)
			UIcomponent.SetActive (false);
		else
			StartCoroutine(DisableComponentUICorrutine(UIcomponent,delayTime));
	}

	IEnumerator DisableComponentUICorrutine(GameObject UIcomponent, float delayTime){
		yield return new WaitForSeconds (delayTime);
		UIcomponent.SetActive (false);
	}

	[ClientRpc]
	public void RpcStartMessage(){
		// Rellenar la lista de players y obtener el Local
		listaPlayers = GameObject.FindGameObjectsWithTag("Player");

		for (int i = 0; i < listaPlayers.Length; i++) {
			if (listaPlayers [i].GetComponent<PlayerSetup> ().isLocalPlayer) {
				myZombieHumanController = listaPlayers [i].GetComponent<ZombieHumanController> ();
			}
		}

		//Comprobar si el local player es Zombie o Humano 
		if(myZombieHumanController.isZombie){
			ActivarComponenteUI(ZombieText);
			DesactivarComponenteUI(ZombieText,5f);
		}else{
			ActivarComponenteUI(HumanText);
			DesactivarComponenteUI(HumanText,5f);
		}
	}

	public void ActivateChooseSpawn(){
		chooseSpawnHumanGO.SetActive (true);
	}

	public void DeactivateChooseSpawn(){
		chooseSpawnHumanGO.SetActive (false);
	}

	public void SetSpawnPointHumanUI(int i)
	{
		spawnPosText.text = i.ToString ();
	}
}
