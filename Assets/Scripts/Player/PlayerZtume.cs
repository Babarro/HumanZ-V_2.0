using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerZtume : NetworkBehaviour
{
    [SerializeField]
    GameObject zombieModel;
    [SerializeField]
    GameObject zTumeModel;
    [SerializeField]
    GameObject weapons;
    [SerializeField]
    GameObject traps;

	PlayerZtume playerZtume;
	ParticlesManager particlesManager;

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        if (zombieModel == null || zTumeModel == null)
            Debug.Log("zombieModel or zTumeModel not founded");
        if (weapons == null || traps == null)
            Debug.Log("weapons or traps not founded");
		playerZtume = GetComponent<PlayerZtume> ();
		particlesManager = GetComponent<ParticlesManager> ();
		if (playerZtume == null)
			Debug.LogError ("PlayerZtume not found");
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    public void CmdActivateOnServer(float timeEffect)
    {
		PlayerZtume playerZtumeScript = gameObject.GetComponent<PlayerZtume>();
		playerZtumeScript.RpcActivateZtume(this.name);
		//playerZtume.RpcActivateZtume(this.name);
		//StartCoroutine(ZtumeActivateOnServerCourutine(timeEffect));
		Invoke("ZtumeActivateOnServerCourutine",timeEffect);
    }

    [ClientRpc]
    public void RpcActivateZtume(string name)
    {
        if (this.transform.name == name)
        {
			foreach (MonoBehaviour script in zTumeModel.GetComponents<MonoBehaviour>()) {
				script.enabled = false;
			}
            zTumeModel.SetActive(true);
            zombieModel.SetActive(false);
            weapons.SetActive(false);
            traps.SetActive(false);
			particlesManager.ztumeToHuman.Play ();
        }
    }

    [ClientRpc]
    public void RpcDeactivateZtume(string name)
    {
        if (this.transform.name == name)
        {
            zombieModel.SetActive(true);
            zTumeModel.SetActive(false);
			foreach (MonoBehaviour script in zTumeModel.GetComponents<MonoBehaviour>()) {
				script.enabled = true;
			}
            weapons.SetActive(true);
            traps.SetActive(true);
			particlesManager.ztumeToZombie.Play ();
        }
    }

	void ZtumeActivateOnServerCourutine()
	{
		PlayerZtume playerZtumeScript = gameObject.GetComponent<PlayerZtume>();
		playerZtumeScript.RpcDeactivateZtume(this.name);
		//playerZtume.RpcDeactivateZtume(this.name);
	}
		
}
