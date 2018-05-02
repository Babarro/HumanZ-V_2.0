using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Animator))]

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private float velocidadVertical;
	private float gravedad = 14.0f;
	private float fuerzaSalto = 7.5f;
//	private bool salto = false ;

    private float currentCameraRotationX = 0f;

	private CharacterController charCtrl;
	[HideInInspector]
	public Animator animator;

    [SerializeField]
    private float cameraRotationLimit = 60f;

	public bool stuned = false;

	void Start ()
	{
		charCtrl = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
	}

	//Se ejecuta cada iteracion de fisicas
	void Update ()
	{
		//Debug.Log ("UpdateMotor - " + Time.time);
		//Comprobar si el jugador esta en el suelo

		if (Grounded()) {
			//Debug.Log ("UpdateMotor-EstoySuelo -> " + Time.time);
			animator.SetBool("IsGrounded", true);
			//Aplicacion de la gravedad 
			velocidadVertical = -gravedad * Time.deltaTime;

			if (Input.GetButtonDown("Jump")  ) 
			{
				animator.SetTrigger ("Jump");
				//Impulso de salto
				velocidadVertical = fuerzaSalto;
				//StartCoroutine("Jump");
				//Debug.Log("Entro");
			}
		}
		else //Si no estoy en el suelo
		{
			velocidadVertical -= gravedad * Time.deltaTime;
			animator.SetBool("IsGrounded", false);

		}

		Vector3 vectorCaida = new Vector3 (0, velocidadVertical, 0);
		charCtrl.Move (vectorCaida * Time.deltaTime);

	}
		
	// Obtener el vector de movimiento
	public void Move (Vector3 move, float zMov, float xMov)
	{
		if (!stuned) {
			animator.SetBool ("Stun", false);
			charCtrl.Move (move * Time.deltaTime);
			//Debug.Log ("Movimiento: " + move);
			animator.SetFloat ("Forward", zMov);
			animator.SetFloat ("Right", xMov);
			//Debug.Log ("InputZ -> " + zMov + " InputX -> " + xMov);
		}else{
			animator.SetBool ("Stun", true);
		}
	}

	/*public void Jump(bool jump)
	{
		salto = jump;
		//Debug.Log ("Salto: " + salto);

	}*/

	//Obtener el vector de rotacion
	public void Rotar (Vector3 rotacion)
	{
		transform.Rotate(0,rotacion.y,0);
	}

	/*//Obtener el vector de Rotacion de la camara
	public void RotarCamara (float camRotacionX)
	{
		if (cam != null) 
		{
			//Limitar la rotacion en vertical de la camara
			currentCameraRotationX -= camRotacionX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit,cameraRotationLimit);

			//Aplicacion de la rotacion a la camara
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f,0f);
		}

	}
	public void UpDownCamera(float camRotationX){

		float upDownValue;
		float nextPosition;

		upDownValue = camRotationX * (-0.05f);
		nextPosition = cam.transform.position.y + upDownValue; 

		//upDownValue = Mathf.Clamp (upDownValue, 0.5f, 2f);
		if (nextPosition >= 1f && nextPosition <= 2f) {
			cam.transform.position = cam.transform.position + new Vector3 (0f, upDownValue, 0f);
		}
		Debug.Log ("upDownValue-> " + upDownValue);
		//cam.transform.position
	}*/

	public bool Grounded()
	{
		RaycastHit hit;
		Vector3 p1 = transform.position + Vector3.up * charCtrl.radius;
		Vector3 p2 = p1 + Vector3.up * charCtrl.height;
		bool isGrounded;

		if (Physics.SphereCast(p1, charCtrl.radius , Vector3.down, out hit, 0.05f)) {
			isGrounded = true;
			//Debug.Log("MetodoGrounded -> Estoy en el suelo");
		} else {
			isGrounded = false;
			//Debug.Log("MetodoGrounded -> Estoy cayendo");
		}
		/*if (Physics.Raycast(transform.position + charCtrl.center, Vector3.down, charCtrl.bounds.extents.y + 0.01f) ) {
			isGrounded = true;
			Debug.Log("MetodoGrounded -> Estoy en el suelo");
		} else {
			isGrounded = false;
			Debug.Log("MetodoGrounded -> Estoy cayendo");
		}*/
			
		return isGrounded;
	}

	IEnumerator Jump(){
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("He llegado a saltar");
		velocidadVertical = fuerzaSalto;
	}

	public Camera getCamera(){
		return cam;
	}
}
//new Vector3(transform.position.x ,charCtrl.bounds.extents.y + 0.1f, transform.position.z)