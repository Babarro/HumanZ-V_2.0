using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RecibirImpacto : NetworkBehaviour {

	public float masa = 3.0f;
	private Vector3 vecImpacto = Vector3.zero;
	private Vector3 vecImpacto2 = Vector3.zero;
	private float tiempoInicial;
	private float tiempoCalc;
	private float value;
	private Vector3 newPos;
	private bool collisionDetected = false;

	private CharacterController charCtrl;
	private Animator animator;
	private ParticlesManager particlesManager;

	// Use this for initialization
	void Start () {
		charCtrl = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
		particlesManager = GetComponent<ParticlesManager> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Aplicar fuerza impacto
		if(vecImpacto.magnitude > 0.2) {
			
			//Metodo 2
			tiempoCalc = Time.time - tiempoInicial ;
			if (tiempoCalc < 2) {
				//Aplicar la fuerza
				charCtrl.Move (vecImpacto2 * Time.deltaTime);

				float t = tiempoCalc / 2;
				value = EaseInOutBounce (0, 1, t);
				//value = EaseOutExpo(0, 1, t);

				//Consumir energia 
				vecImpacto2 = Vector3.Lerp (vecImpacto2, Vector3.zero, value * Time.fixedDeltaTime);
			} else {
				animator.SetBool ("Quemado", false);
			}
				
		}
	}

	[ClientRpc]
	public void RpcAddImpact(Vector3 direccion, float force){
		Impact (direccion, force);
 	}
		
	public void AddImpact(Vector3 direccion, float force){
		Impact (direccion, force);
	}

	//Se llama en el server cuando un jugador es empujado
	[Command]
	void CmdOnFire(){
		RpcDoFireEffect ();
	}

	//Se llama en todos los clientes, cuando se tiene que hace el efecto de fuego
	[ClientRpc]
	void RpcDoFireEffect(){
		particlesManager.fireFX.Play ();
		//StartCoroutine (StopParticleSystem (particlesManager.fireFX, 2.0f));
	}

	IEnumerator StopParticleSystem (ParticleSystem particle, float sec){
		yield return new WaitForSeconds (sec);
		particle.Stop ();
	}

	void Impact(Vector3 direccion, float force){
		if (!isLocalPlayer)
			return;

		animator.SetBool ("Quemado", true);

		//Estamos quemando, llama al metodo OnFire en el server
		CmdOnFire ();

		direccion.Normalize ();
		tiempoInicial = Time.time;
		Debug.Log ("Tiempo Inicial: " + tiempoInicial);

		if (direccion.y < 0)
			direccion.y = -direccion.y;

		vecImpacto = transform.position + (direccion.normalized * force / masa);
		vecImpacto2 = (direccion.normalized * force / masa); 
	}



	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float d = 1f;
		if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
		else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
	}
	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float d = 1f;
		return end - EaseOutBounce(0, end, d - value) + start;
	}

	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f))
		{
			return end * (7.5625f * value * value) + start;
		}
		else if (value < (2 / 2.75f))
		{
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		}
		else if (value < (2.5 / 2.75))
		{
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		}
		else
		{
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}
}
