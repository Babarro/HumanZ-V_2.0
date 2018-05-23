using System.Collections;
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
	ParticlesManager particlesManager;

	private UIController uiController;

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
		particlesManager = GetComponent<ParticlesManager> ();
		if (particlesManager == null)
			Debug.LogError ("ParticlesManager not found");	
    }

    // Use this for initialization
    void Start() {
        if (zombieGO.activeSelf && humanGO.activeSelf)
            Debug.LogError("Human and zombie are active in the same time");

        isZombie = zombieGO.activeSelf;
    }
	[ClientRpc]
	public void RpcSetup(){
		uiController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();
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
		//Desactivar Barra infeccion
		if(isLocalPlayer)
			uiController.DesactivarComponenteUI (uiController.barraInfeccion);
		
		playerZtume.CancelInvoke ();
		foreach (MonoBehaviour script in humanGO.GetComponents<MonoBehaviour>()) {
			script.enabled = true;
		}
        humanGO.SetActive(true);
        zombieGO.SetActive(false);
        isZombie = false;
		playerInventory.DestroyAllItems ();
		playerInput.ChangeVelocity(isZombie);
		particlesManager.conversionHuman.Play ();
		if (isLocalPlayer) {
			StartCoroutine (StunTime(stunTime));
		}
    }

    public void HumanToZombie()
    {
		//Activar Barra infeccion
		if(isLocalPlayer)
			uiController.ActivarComponenteUI (uiController.barraInfeccion);
		
        zombieGO.SetActive(true);
        humanGO.SetActive(false);
        isZombie = true;
		playerHp.SetStartTime ();
		playerInput.ChangeVelocity(isZombie);
    }

	[ClientRpc]
	public void RpcSetHuman()
	{
		if(isLocalPlayer)
			uiController.DesactivarComponenteUI (uiController.barraInfeccion);

		playerZtume.CancelInvoke ();
		foreach (MonoBehaviour script in humanGO.GetComponents<MonoBehaviour>()) {
			script.enabled = true;
		}
		humanGO.SetActive(true);
		zombieGO.SetActive(false);
		isZombie = false;
		playerInventory.DestroyAllItems ();
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
