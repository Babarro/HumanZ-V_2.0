using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryController : MonoBehaviour {

	[SerializeField]
	Image powerUpImage;

	[SerializeField]
	Image weaponImage1;
	[SerializeField]
	Text weaponText1;
	[SerializeField]
	GameObject inUseWeapon;

	[SerializeField]
	Image trapImage1;
	[SerializeField]
	Text trapText1;
	[SerializeField]
	GameObject inUseTrap;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetInUse(bool isWeaponInUse){
		if (isWeaponInUse) {
			inUseTrap.SetActive (false);
			inUseWeapon.SetActive (true);
		} else {
			inUseWeapon.SetActive (false);
			inUseTrap.SetActive (true);
		}
			
	}

	public void NothingInUse(){
		inUseTrap.SetActive (false);
		inUseWeapon.SetActive (false);
	}

	public void SetPowerUp(Sprite newImage){
		if (powerUpImage.enabled==false)
			powerUpImage.enabled = true;

		powerUpImage.sprite = newImage;
	}

	public void DestroyPowerUp(){
		powerUpImage.enabled = false;
	}


	public void SetWeapon1(Sprite newImage, int newUses){
		if (weaponImage1.enabled==false)
			weaponImage1.enabled = true;
		if (weaponText1.enabled == false)
			weaponText1.enabled = true;

		weaponImage1.sprite = newImage;
		weaponText1.text = newUses.ToString ();
	}

	public void SetWeapon1(int newUses){
		if (weaponText1.enabled == false)
			weaponText1.enabled = true;

		weaponText1.text = newUses.ToString ();
	}

	public void SetWeapon1(Sprite newImage){
		if (weaponImage1.enabled==false)
			weaponImage1.enabled = true;

		weaponImage1.sprite = newImage;
	}

	public Sprite GetWeapon1Sprite(){
		return weaponImage1.sprite;
	}

	public int GetWeapon1Uses(){
		return int.Parse(weaponText1.text);
	}

	public void DestroyWeapon1(){
		weaponImage1.enabled = false;
		weaponText1.enabled = false;
	}


	public void SetTrap1(Sprite newImage, int newUses){
		if (trapImage1.enabled==false)
			trapImage1.enabled = true;
		if (trapText1.enabled == false)
			trapText1.enabled = true;

		trapImage1.sprite = newImage;
		trapText1.text = newUses.ToString ();
	}

	public void SetTrap1(int newUses){
		if (trapText1.enabled == false)
			trapText1.enabled = true;

		trapText1.text = newUses.ToString ();
	}

	public void SetTrap1(Sprite newImage){
		if (trapImage1.enabled==false)
			trapImage1.enabled = true;

		trapImage1.sprite = newImage;
	}

	public Sprite GetTrap1Sprite(){
		return trapImage1.sprite;
	}

	public int GetTrap1Uses(){
		return int.Parse(trapText1.text);
	}

	public void DestroyTrap1(){
		trapImage1.enabled = false;
		trapText1.enabled = false;
	}
		
}
