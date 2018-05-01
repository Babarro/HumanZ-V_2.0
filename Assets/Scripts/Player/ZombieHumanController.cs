﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZombieHumanController : NetworkBehaviour {

    [SerializeField]
    private GameObject zombieGO;
    [SerializeField]
    private GameObject humanGO;
    public bool isZombie;

    PlayerInput playerInput;
	PlayerHp playerHp;
	PlayerInventory playerInventory;
	PlayerZtume playerZtume;
	PlayerMotor playerMotor;

	[SerializeField]
	float stunTime;

    private void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		if (playerInput == null)
			Debug.LogError("PlayerInput not found");

		playerHp = GetComponent<PlayerHp> ();
		if (playerHp == null)
			Debug.LogError ("PlayerHp not found");

		playerInventory = GetComponent<PlayerInventory> ();
		if (playerInventory == null)
			Debug.LogError ("PlayerInventory not found");

		playerZtume = GetComponent<PlayerZtume> ();
		if (playerZtume == null)
			Debug.LogError ("PlayerZtume not found");

		playerMotor = GetComponent<PlayerMotor> ();
		if (playerMotor == null)
			Debug.LogError ("PlayerMotor not found");
    }

    // Use this for initialization
    void Start() {
        if (zombieGO.activeSelf && humanGO.activeSelf)
            Debug.LogError("Human and zombie are active in the same time");

        isZombie = zombieGO.activeSelf;
    }

    [ClientRpc]
    public void RpcSetForm(bool isZombieEnter)
    {
        if (isZombieEnter)
        {
            HumanToZombie();
        }
        else
		{
            ZombieToHuman();
        }
    }
    public void ZombieToHuman()
    {
		playerZtume.CancelInvoke ();
		foreach (MonoBehaviour script in humanGO.GetComponents<MonoBehaviour>()) {
			script.enabled = true;
		}
        humanGO.SetActive(true);
        zombieGO.SetActive(false);
        isZombie = false;
		playerInventory.DestroyAllItems ();
		playerInput.ChangeVelocity(isZombie);
		if (isLocalPlayer) {
			StartCoroutine (StunTime(stunTime));
		}
    }

    public void HumanToZombie()
    {
        zombieGO.SetActive(true);
        humanGO.SetActive(false);
        isZombie = true;
		playerHp.SetStartTime ();
		playerInput.ChangeVelocity(isZombie);
    }

    [Command]
    public void CmdChangeForm(string name, bool isZombieEnter)
    {
		GMEndGameController.instance.ChangeHumanz((isZombieEnter? -1 : 1));
		GameObject go = GameObject.Find(name);
		go.GetComponent<ZombieHumanController>().RpcSetForm(isZombieEnter);
    }

	IEnumerator StunTime(float time){
		playerMotor.stuned = true;
		yield return new WaitForSeconds (time);
		playerMotor.stuned = false;
	}
}
