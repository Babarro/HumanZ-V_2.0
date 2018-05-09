using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GMCurasController : NetworkBehaviour {

	public UIController uiController;

	[SerializeField]
	GameObject curasPositionParent;
	[SerializeField]
	GameObject curaPrefab;

	[SerializeField]
	GameObject curasParent;

	public int timeToChangeCuras = 45;
	public int activeCurasAtTheSameTime = 3;

	float startTime;
	bool canStart = false;

	int[] lastCuras;

	void Awake(){
		lastCuras = new int[activeCurasAtTheSameTime];
		for (int i = 0; i < activeCurasAtTheSameTime; i++) {
			lastCuras [i] = 0;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canStart) {
			if ((uiController.time - startTime) >= timeToChangeCuras) {
				ChangeCuras ();
				startTime = uiController.time;
			}
		}
	}

	public void SetUp(){
		SpawnCuras ();
		startTime = uiController.time;
		ChangeCuras ();

		canStart = true;
	}

	void SpawnCuras(){
		for (int i = 0; i < curasPositionParent.transform.childCount; i++) {
			GameObject newCura = Instantiate (curaPrefab, curasPositionParent.transform.GetChild (i).transform.position, Quaternion.identity) as GameObject;
			newCura.transform.parent = curasParent.transform;
			NetworkServer.Spawn (newCura);
		}
	}

	void ChangeCuras(){
		for (int i = 0; i < activeCurasAtTheSameTime; i++) {
			curasParent.transform.GetChild (lastCuras[i]).GetComponent<CuraCollision> ().RpcDisableCura ();
			lastCuras [i] = -1;
		}

		for (int i = 0; i < activeCurasAtTheSameTime; i++) {
			int curaToActivate = -1;
			int founded = 0;
			while (founded != activeCurasAtTheSameTime) {
				founded = 0;
				curaToActivate = Random.Range (0,curasParent.transform.childCount);
				for (int j = 0; j < activeCurasAtTheSameTime; j++) {
					if(lastCuras[j]!=curaToActivate){
						founded++;
					}
				}
			}
			lastCuras [i] = curaToActivate;
			curasParent.transform.GetChild (curaToActivate).GetComponent<CuraCollision> ().RpcEnableCura ();
		}
	}

	public void DeactivateAllCuras(){
		for (int i = 0; i < activeCurasAtTheSameTime; i++) {
			curasParent.transform.GetChild (lastCuras[i]).GetComponent<CuraCollision> ().RpcDisableCura ();
			lastCuras [i] = -1;
		}
		canStart = false;
	}
}
