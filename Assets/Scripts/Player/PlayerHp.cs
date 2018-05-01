using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHp : NetworkBehaviour {

	[SerializeField]
	private int totalTimeToConvert = 50;

	[SerializeField]
	ZombieHumanController zhController;

	//Image hpBar;
	//Text hpBar;
	Slider hpBar;
	UIController timeScript;
	bool transformed = false;

	Vector2 hpBarStartSize;
	bool canChangeBar = false;
	float startTime = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(canChangeBar && zhController.isZombie){
			//hpBar.text = ((int)(totalTimeToConvert - (timeScript.time - startTime))).ToString();
			hpBar.value = (int)(totalTimeToConvert - (timeScript.time - startTime));
			if (((totalTimeToConvert - (timeScript.time - startTime)) <= 0) && !transformed) {
				transformed = true;
				zhController.CmdChangeForm (this.name, false);
			}
			//hpBar.rectTransform.sizeDelta = new Vector2 ((hpBarStartSize.x*(totalTimeToConvert - (timeScript.time - startTime)))/totalTimeToConvert,hpBarStartSize.y);
			//hpBar.rectTransform.sizeDelta = new Vector2(((totalTimeToConvert-timeScript.time -startTime)/totalTimeToConvert)*hpBarStartSize.x,hpBarStartSize.y);
			//hpBar.rectTransform.sizeDelta -= new Vector2((totalTimeToConvert - timeScript.time - startTime)/totalTimeToConvert,0);
		}
	}

	[ClientRpc]
	public void RpcSetUp(){
		if (!isLocalPlayer)
			return;
		//hpBar = GameObject.FindGameObjectWithTag ("HpBar").GetComponent<Text> ();
		hpBar = GameObject.FindGameObjectWithTag ("HpBar").GetComponent<Slider> ();
		timeScript = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();
		//hpBarStartSize = hpBar.value;
		SetStartTime();

		canChangeBar = true;

	}

	public void SetStartTime(){
		if (!isLocalPlayer)
			return;
		startTime = timeScript.time;
		transformed = false;
	}

	/*public void SetUp(){
		hpBar = GameObject.FindGameObjectWithTag ("HpBar").GetComponent<Image> ();
		timeScript = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();
		canChangeBar = true;
	}*/
}
