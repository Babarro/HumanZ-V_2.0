using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PistolaPunyo : MonoBehaviour, IActivation {

	PlayerDisparoPunyo plDisparoPunyo;

	public int uses = 5;
	//public float power;
	//public float distance; //Distancia a la que llega el arma
	public float cd; //Tiempo de espera desde que lo utilizas hasta su siguiente uso
	private bool _canShoot = false;

	void Start()
	{
		//_canShoot = false;
	}

	public void Activate()
	{
		if (_canShoot)
		{
			if (plDisparoPunyo.isLocalPlayer) {
				plDisparoPunyo.CmdPushPunyo ();
				this.transform.parent.parent.parent.GetComponent<PlayerInventory> ().ActualRecursoLessUses();
			}
			_canShoot = false;
			uses--;

			StartCoroutine("Collect");
		}
	}

	public void Deactivate(){
	}

	public int GetUses(){
		return uses;
	}

	public GameObject GetGameObject (){
		return this.gameObject;
	}

	public void SetUp(){
		plDisparoPunyo = this.transform.parent.parent.parent.GetComponent<PlayerDisparoPunyo> ();
		_canShoot = true;
	}

	IEnumerator Collect()
	{
		yield return new WaitForSeconds(cd);
		plDisparoPunyo.CmdPullPunyo();
		_canShoot = true;
		if (plDisparoPunyo.isLocalPlayer) {
			if (uses == 0) {
				this.transform.parent.parent.parent.GetComponent<PlayerInventory> ().CmdDestroyActualRecurso (this.transform.parent.parent.parent.name);
			}
		}
	}
}
