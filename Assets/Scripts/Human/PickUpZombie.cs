using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpZombie : MonoBehaviour {

	[SerializeField]
	ZombieHumanController zhController;

	public float probOfConvertToZombie = 0.5f;
	float startProbOfConvertToZombie;

	public bool canPick = false;

	void Awake(){
		startProbOfConvertToZombie = probOfConvertToZombie;
	}

    // Use this for initialization
    void Start () {
		/*
		zhController = transform.parent.GetComponent<ZombieHumanController> ();
		if(zhController!=null)
			Debug.LogError("ZombieHumanController not founded!");
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
		if (zhController.isServer) {
			if (other.gameObject.tag == "Player") {
				if (canPick) {
					if (other.GetComponent<ZombieHumanController> ().isZombie) {
						float convert = Random.Range (0.0f, 1.0f);
						if (convert <= probOfConvertToZombie) {
							zhController.CmdChangeForm (transform.parent.name, true);
							probOfConvertToZombie = startProbOfConvertToZombie;
						}
						zhController.CmdChangeForm (other.gameObject.name, false);
					}
				}
			}
		}
    }
   
}
