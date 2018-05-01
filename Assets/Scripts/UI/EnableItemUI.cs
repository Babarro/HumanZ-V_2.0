using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableItemUI : MonoBehaviour {
	public string etiquetaItemUI;
	private UIController uiController;
	private GameObject itemPanel;

	void Start()
	{
		uiController = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<UIController> ();

		switch (etiquetaItemUI) {
		case "TrampaUI":
			itemPanel = uiController.TrampaPanel;
		break;
		case "PowerUpUI":
			itemPanel = uiController.PowerUpPanel;
		break;
		case "PistolaUI":
			itemPanel = uiController.ArmaPanel;
		break;
		}
		//itemPanel = GameObject.FindGameObjectWithTag (etiquetaItemUI);

		if(itemPanel == null)
			Debug.Log("No se ha encontrado la referencia del ItemUI");
	}

	public void OnTriggerEnter(Collider c){
		if (c.CompareTag ("Player")) {
			if (c.GetComponent<PlayerSetup> ().isLocalPlayer && c.GetComponent<ZombieHumanController>().isZombie) {
				uiController.ActivarComponenteUI(itemPanel);
			}

		}

	}

	public void OnTriggerExit(){
		uiController.DesactivarComponenteUI (itemPanel);
	}
		
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.E)) {
			itemPanel.SetActive (false);
		}
	}
}
