using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Animator))]

public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	public Vector3 offsetSperecastCenter;
	public float maxDistanceSpherecast;
	private float velocidadVertical;
	public float gravedad = 14.0f;
	public float fuerzaSalto = 7.5f;
	private bool canJump = true;
	public float timeAnimJump;

	private CharacterController charCtrl;

	//Spectator mode
	private PlayerInput playerInput;

	[HideInInspector]
	public Animator animator;

	public bool stuned = false;

	void Start ()
	{
		charCtrl = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
		playerInput = GetComponent<PlayerInput> ();
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

			if (Input.GetButtonDown("Jump") && canJump) 
			{
				animator.SetTrigger ("Jump");
				//Impulso de salto
				velocidadVertical = fuerzaSalto;
				StartCoroutine (TimeJump (timeAnimJump));

				//Debug.Log("Entro");
			}
		}
		else //Si no estoy en el suelo
		{
			velocidadVertical -= gravedad * Time.deltaTime;
			animator.SetBool("IsGrounded", false);

		}
	
		Vector3 vectorCaida = new Vector3 (0, velocidadVertical, 0);
		if(playerInput.spectatorMode)
			vectorCaida = new Vector3 (0, 0, 0);
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

	//Obtener el vector de rotacion
	public void Rotar (Vector3 rotacion)
	{
		transform.Rotate(0,rotacion.y,0);
	}

	public bool Grounded()
	{
		RaycastHit hit;
		Vector3 p1 = transform.position + offsetSperecastCenter + Vector3.up * charCtrl.radius;
		//Vector3 p2 = p1 + Vector3.up * charCtrl.height;
		bool isGrounded;

		if (Physics.SphereCast(p1, charCtrl.radius , Vector3.down, out hit, maxDistanceSpherecast)) {
			isGrounded = true;
			//Debug.Log("MetodoGrounded -> Estoy en el suelo");
		} else {
			isGrounded = false;
			//Debug.Log("MetodoGrounded -> Estoy cayendo");
		}
			
		return isGrounded;
	}

	public Camera getCamera(){
		return cam;
	}

	IEnumerator TimeJump(float jumpTime){
		canJump = false;
		yield return new WaitForSeconds (jumpTime);
		canJump = true;
	}
}