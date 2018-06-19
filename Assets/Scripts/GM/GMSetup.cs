﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class GMSetup : NetworkBehaviour {

	public int players = 0;

	[SerializeField]
	GameObject spawnPositionParent;

	UIController uiController;

	[SerializeField]
	GMRecursos gmRecursos;

	[SerializeField]
	GameObject canvasPrefab;

	[SerializeField]
	Transform[] humanTpPoint;
	[SerializeField]
	GameObject minimapResources;

	[SerializeField]
	GameObject iaZombieHumanPref;

	public float lootPhaseTime = 10f;
	public float minimapPhaseTime = 5f;

	bool hasEnter = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasEnter) {
			foreach (NetworkConnection conn in NetworkServer.connections) {
				if (!conn.isReady) {
					return;
				}
			}
			if (LobbyManager.s_Singleton._playerNumber == players) {
				OnPlayersReady ();
				hasEnter = true;
			}
		}
	}

	void OnPlayersReady(){
		GameObject[] playersList = GameObject.FindGameObjectsWithTag ("Player");
		List<GameObject> humanzPlayers = new List<GameObject> ();
		SetUpFirst (playersList);

		//Se convierten en Humanz dos jugadores aleatorios
		int numberOfHumanzs = Mathf.Clamp(playersList.Length / 5,1,2);
		for (int i = 0; i < numberOfHumanzs; i++) {
			int lucky;
			do{
				lucky = Random.Range (0, playersList.Length);
			}while(playersList[lucky] == null);
			playersList [lucky].GetComponent<ZombieHumanController> ().RpcSetFirstHuman();
			humanzPlayers.Add (playersList[lucky]);
			playersList [lucky] = null;
		}

		//Se inicializa a los jugadores a su posicion de spawn
		int spawnPosIndex = 0;
		for (int i = 0; i < playersList.Length; i++) {
			if (playersList [i] != null) {

				playersList [i].GetComponent<PlayerSetup> ().RpcSetPosition (spawnPositionParent.transform.GetChild(spawnPosIndex).position);
				spawnPosIndex++;

			}
		}
		foreach (GameObject human in humanzPlayers) {
			human.AddComponent<HumanInitialSpawn> ();
			human.GetComponent<PlayerSetup> ().RpcSetPosition(new Vector3(122,-79,-141));
		}


		//Se instancian las ias
		SpawnIA(numberOfHumanzs);

		gmRecursos.SpawnRecursos ();
		SetTimers ();
		GetComponent<GMEndGameController> ().SetUp (LobbyManager.s_Singleton._playerNumber,numberOfHumanzs);
		uiController.RpcStartMessage ();
		SetUpBeforePhaseTime (playersList,humanzPlayers);
		StartCoroutine (MinimapTime(playersList,humanzPlayers));
		StartCoroutine (PhaseTime(playersList,humanzPlayers));
	}
		
	void SetTimers(){
		uiController.startTime = Time.time;
		uiController.time = Time.time;
	}

	IEnumerator MinimapTime(GameObject[] zombies,List<GameObject> humanz){
		yield return new WaitForSeconds (minimapPhaseTime);
		foreach (GameObject zombie in zombies) {
			if (zombie != null) {
				zombie.GetComponent<PlayerSetup> ().RpcDeactivateArrow();
			}
		}

		foreach (GameObject human in humanz) {
			human.GetComponent<PlayerSetup> ().RpcDeactivateArrow ();
		}
	}

	IEnumerator PhaseTime(GameObject[] playersList,List<GameObject> humanzPlayers){
		yield return new WaitForSeconds (lootPhaseTime);
		GetComponent<GMCurasController> ().SetUp ();
		SetUpAfterPhaseTime (playersList,humanzPlayers);
		RpcDestroyMinimapResources ();
		Destroy (this);
	}

	[ClientRpc]
	void RpcDestroyMinimapResources(){
		Destroy (minimapResources);
	}

	void SetUpFirst(GameObject[] players){

		SpawnCanvas ();


		foreach (GameObject player in players) {
			if (player != null) {
				player.GetComponent<PlayerHp> ().RpcSetUpFirst();
				player.GetComponent<PlayerInventory> ().RpcSetup();
				player.GetComponent<PlayerSetup> ().RpcSetUp ();
				player.GetComponent<PlayerSetup> ().RpcSetUpName (player.GetComponent<PlayerSetup>().playerName);
				player.GetComponent<ZombieHumanController> ().RpcSetup ();
			}
		}


	}

	void SetUpBeforePhaseTime(GameObject[] zombies, List<GameObject> humanz){
		
		foreach (GameObject zombie in zombies) {
			if (zombie != null) {
				zombie.GetComponent<PlayerSetup> ().RpcDeactivateLoadingScreen();
			}
		}

		foreach (GameObject human in humanz) {
			PlayerSetup pSetUp = human.GetComponent<PlayerSetup> ();
			pSetUp.RpcActivateMinimap ();
			pSetUp.RpcActivateChooseSpawnUI ();
			pSetUp.RpcDeactivateLoadingScreen();
		}


	}

	void SetUpAfterPhaseTime(GameObject[] zombies, List<GameObject> humanz){
	
		foreach (GameObject zombie in zombies) {
			if (zombie != null) {
				zombie.GetComponent<PlayerHp> ().RpcSetUpFinal();
			}
		}

		foreach (GameObject human in humanz) {
			human.GetComponent<PlayerHp> ().RpcSetUpFinal ();
			PlayerSetup psetup = human.GetComponent<PlayerSetup> ();
			psetup.RpcDeactivateMinimap ();
			psetup.RpcDeactivateChooseSpawnUI();
			psetup.RpcSetPositionSpecial(humanTpPoint[human.GetComponent<HumanInitialSpawn>().spawnChoose].position,humanTpPoint[human.GetComponent<HumanInitialSpawn>().spawnChoose].rotation,"HumanAfterLoot");
			Destroy(human.GetComponent<HumanInitialSpawn> ());
		}

	}

	void SpawnCanvas(){
		GameObject canvas = Instantiate (canvasPrefab, canvasPrefab.transform.position, canvasPrefab.transform.rotation) as GameObject;
		NetworkServer.Spawn (canvas);
		uiController = canvas.GetComponent<UIController> ();
		gmRecursos.uiController = uiController;
		GetComponent<GMCurasController> ().uiController = uiController;

	}

	void SpawnIA(int numberOfHumanzs){
		for (int i = players-numberOfHumanzs; i < spawnPositionParent.transform.childCount; i++) {
			GameObject newIa = Instantiate(iaZombieHumanPref,spawnPositionParent.transform.GetChild(i).transform.position,Quaternion.identity) as GameObject;
			NetworkServer.Spawn (newIa);
		}
	}

}
