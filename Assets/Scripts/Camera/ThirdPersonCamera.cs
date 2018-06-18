using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
	
	private float xRot;
	private float yMove;
	public Transform targetToLook;
	public float sensibilidad;
	public float maxTargetY;
	public float minTargetY;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.LookAt (targetToLook.position);
	}

	void Update(){
		//Input Mouse Y
		float xRot = Input.GetAxis("Mouse Y");
		yMove = xRot * sensibilidad;

		MoveTargetToLook (yMove);
	}

	// Update is called once per frame
	void LateUpdate () {
		
		this.gameObject.transform.LookAt (targetToLook.position);
		MoveCameraUpDown (-yMove);
	}

	void MoveTargetToLook(float yMove){

		float nextTargetY = targetToLook.localPosition.y + yMove;
		if (nextTargetY < maxTargetY && nextTargetY > minTargetY) {

			targetToLook.transform.position = targetToLook.position + new Vector3 (0f, yMove, 0f);

			//float actualCamY = this.transform.position.y;
			//float nextCamY = this.transform.position.y + (-yMove);
		}			
	}

	void MoveCameraUpDown(float yMove){

		float nextCamPosY = this.transform.localPosition.y + yMove ;

		if(nextCamPosY < maxTargetY && nextCamPosY > minTargetY)
			this.transform.position = this.transform.position + new Vector3 (0f, yMove ,  0f);
	}

}
