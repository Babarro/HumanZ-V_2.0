using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GMRecursos : NetworkBehaviour {

	public bool oneZombie = false;

	public UIController uiController;

	public int respawnTime = 150;
	bool enterRespawnTime = false;

	bool isSetup = false;

	[SerializeField]
	Transform recursosSpawnPositionParent;

	[System.Serializable]
	public class recursoProbability{

		public GameObject recurso;
		[Range(0.0f,1.0f)]
		public float prob;

		public recursoProbability(GameObject recIn, float probIn){
			this.recurso = recIn;
			this.prob = probIn;
		}

	}

	[SerializeField]
	List<recursoProbability> weaponsList;
	[SerializeField]
	List<recursoProbability> trapsList;
	[SerializeField]
	GameObject powerUp;

	[Range(0,1)]
	public float probOfSpawn = 0.5f;

	[Range(0,1)]
	public float probWeapon = 0.33f;
	[Range(0,1)]
	public float probTrap = 0.33f;
	[Range(0,1)]
	public float probPowerUp = 0.34f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isSetup) {
			if (uiController.gameDuration - (int)(uiController.time - uiController.startTime) <= respawnTime && !enterRespawnTime && !oneZombie) {
				SpawnRecursos ();
				enterRespawnTime = true;
			}
		}
	}

	public void SpawnRecursos(){

		for (int i = 0; i < recursosSpawnPositionParent.childCount; i++) {

			float spawnRecurso = Random.Range (0.0f, 1.0f);
			if(spawnRecurso>=1-probOfSpawn){
				float typeOfRecurso = Random.Range (0.0f,1.0f);
				if(typeOfRecurso<probWeapon){

					float weapon = Random.Range (0.0f,1.0f);
					float acum = 0;
					foreach (recursoProbability item in weaponsList) {
						acum += item.prob;
						if (weapon < acum) {
							GameObject weaponInstance = Instantiate (item.recurso,recursosSpawnPositionParent.GetChild(i).position,Quaternion.identity);
							NetworkServer.Spawn (weaponInstance);
							break;
						}
					}

				}else if(typeOfRecurso<probWeapon+probTrap){

					float trap = Random.Range (0.0f,1.0f);
					float acum = 0;
					foreach (recursoProbability item in trapsList) {
						acum += item.prob;
						if (trap < acum) {
							GameObject trapInstance = Instantiate (item.recurso,recursosSpawnPositionParent.GetChild(i).position,Quaternion.identity);
							NetworkServer.Spawn (trapInstance);
							break;
						}
					}

				}else{

					GameObject powerUpInstance = Instantiate (powerUp,recursosSpawnPositionParent.GetChild(i).position,Quaternion.identity);
					NetworkServer.Spawn (powerUpInstance);

				}
			}

		}

	}

	public void SetUp(){
		isSetup = true;
	}
}
