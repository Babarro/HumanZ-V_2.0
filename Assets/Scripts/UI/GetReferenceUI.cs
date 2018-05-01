using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReferenceUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("DisableUI", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisableUI()
	{
		this.gameObject.SetActive (false);
	}
}
