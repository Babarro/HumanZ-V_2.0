using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	//public GameObject balaPrefab;
	//public Transform balaSpawn;

	//public PlayerWeapon arma;

    public GameObject weapon;

    ZombieHumanController zhControllerScript;

    [SerializeField]
	private Camera cam;

	void Start ()
	{
		if (cam == null) 
		{
			//Debug.Log ("No camara reference!");
			this.enabled = false;
		}

        zhControllerScript = this.gameObject.GetComponent<ZombieHumanController>();
        if (zhControllerScript == null)
            Debug.LogError("ZombieHumanController script not founded");
    }

	void Update ()
	{
        if (zhControllerScript.isZombie)
        {
			if (isLocalPlayer) {
				if (Input.GetButtonDown ("Fire1")) {
					IWeapon weaponScript = weapon.GetComponentInChildren<IWeapon> ();
					if (weaponScript != null)
						weaponScript.Shoot ();
					//CmdShoot ();
				}
			}
        }
	}

	/*[Command]
	void CmdShoot()
	{
		//Crea la bala del Prefab
		var bala = (GameObject)Instantiate(balaPrefab, balaSpawn.position, balaSpawn.rotation);

		//Dar velocidad a la bala
		bala.GetComponent<Rigidbody>().velocity = bala.transform.forward * 15;

		//Crear la balas en los clientes
		NetworkServer.Spawn(bala);

		//Destruir la bala despues de 2 segundos
		Destroy(bala, 2f);
	}*/

}
