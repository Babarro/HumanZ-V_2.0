using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMEndGameController : MonoBehaviour {

	[SerializeField]
	GMRecursos gmRecursos;

	public int numberOfPlayers;
	public int humanzs;
	UIController uiController;
	public static GMEndGameController instance;
	public int timeLastZombieHasToSurvive =10;

	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUp(int players, int numberOfHumanzs){
		gmRecursos.oneZombie = true;
		numberOfPlayers = players;
		humanzs = numberOfHumanzs;
		uiController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController>();
		uiController.numberOfZombie = numberOfPlayers - humanzs;
	}

	public void ChangeHumanz(int toChange){
		humanzs += toChange;

		//Si solo queda un zombie
		if(humanzs == numberOfPlayers-1){
			uiController.startTime = uiController.time;
			uiController.gameDuration = timeLastZombieHasToSurvive;
			uiController.oneZombie = true;
		}else if(humanzs == numberOfPlayers){
			uiController.RpcLooseGame();
		}
		uiController.numberOfZombie = numberOfPlayers - humanzs;
	}
}
