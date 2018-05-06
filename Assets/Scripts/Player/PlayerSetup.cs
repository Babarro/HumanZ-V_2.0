using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class PlayerSetup : NetworkBehaviour {

    //Lista de componentes para deshabilitar
    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCamera;

    public GameObject pistolaPunyo;
    public GameObject weapons;

    public GameObject ztume;
    public GameObject powerUps;

    public GameObject InZendiaryBomb;
    public GameObject traps;

	public string playerName;

	[SerializeField]
	TextMesh playerNameText;
	[SerializeField]
	PlayerTextName playerTextName;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start()
    {
        //CmdSetName("Player " + GetComponent<NetworkIdentity>().netId);
		this.gameObject.transform.name = "Player " + GetComponent<NetworkIdentity>().netId;
		//this.gameObject.transform.name = "Player " + GetComponent<NetworkIdentity>().netId;
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

    }

	void Update(){
		if (!isLocalPlayer)
			return;
		if (Input.GetKeyDown (KeyCode.J)) {
			GetComponent<ZombieHumanController>().CmdChangeForm (this.name,true);
		}
	}

	public override void OnStartLocalPlayer(){
		base.OnStartLocalPlayer ();

		if (!isLocalPlayer || !isClient)
			return;
		
		//CmdPlayerReady ();
		/*if (isLocalPlayer) {
			float zombie = Random.Range(0.0f, 1.0f);
			Debug.Log(zombie);
			if (zombie <= 0.5)
			{
				CmdChangeForm(this.gameObject.transform.name, false);
				//GetComponent<ZombieHumanController>().CmdSetForm(false);
			}
			else
			{
				CmdChangeForm(gameObject.name, true);
				//GetComponent<ZombieHumanController>().CmdSetForm(true);
			}
		}*/

		//hay que spawnearlo en el zombie
		//Creamos el arma Pistola puño para que la tenga cuando se crea el objeto, esto se debera cambiar cuando podamos recoger armas del suelo
		//GameObject pistolaInstantiate = Instantiate(pistolaPunyo, weapons.transform.position, weapons.transform.rotation);
		//pistolaInstantiate.transform.parent = weapons.transform;
		//Creamos el powerUp Ztume para que la tenga cuando se crea el objeto, esto se debera cambiar cuando podamos recoger powerUp del suelo
		//GameObject ztumeInstantiate = Instantiate(ztume, powerUps.transform.position, powerUps.transform.rotation);
		//ztumeInstantiate.transform.parent = powerUps.transform;
		//Creamos la trampa Bomba InZendiaria para que la tenga cuando se crea el objeto, esto se deberia cambiar cuando podamos recoger la trampa del suelo
		//GameObject inZendiaryBombInstantiate = Instantiate(InZendiaryBomb, InZendiaryBomb.transform.position,InZendiaryBomb.transform.rotation);
		//inZendiaryBombInstantiate.transform.parent = traps.transform;
	}
	/*
    [Command]
    void CmdChangeForm(string name, bool isZombie)
    {
        //hzGMController.changeForm(name, isZombie);
		GameObject go = GameObject.Find(name);
		go.GetComponent<ZombieHumanController>().RpcSetForm(name, isZombie);
    }
	*/
	void OnDisable()
	{
		if (sceneCamera != null) 
		{
			sceneCamera.gameObject.SetActive(false);
		}
	}
	[Command]
	public void CmdIsReadyPlayer(){
		GameObject.Find ("GM").GetComponent<GMSetup>().players++;
	}

	[ClientRpc]
	public void RpcSetPosition(Vector3 pos){
		if (!isLocalPlayer)
			return;

		transform.position = pos;
	}

	public void SetUpName(string name){
		playerName = name;
		//playerNameText.text = name;
	}

	[ClientRpc]
	public void RpcSetUpName(string name){
		playerNameText.text = name;
		playerTextName.SetUpCamera ();
	}

	[ClientRpc]
	public void RpcDeactivateLoadingScreen(){
		LobbyManager.s_Singleton.SetLoadingScreen (false);
		Debug.Log ("llega");
	}
}
