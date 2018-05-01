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

	// Use this for initialization
	void Start () {
		charCtrl = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Aplicar fuerza impacto
		if(vecImpacto.magnitude > 0.2) {
			/* Metodo 1
			tiempoCalc = Time.time - tiempoInicial ;
			//Debug.Log ("TiempoCalc: " + tiempoCalc);
			if (tiempoCalc < 2) {
			
				float t = tiempoCalc / 2;
				//value = EaseInOutQuint (0, 1, t);
				//value = EaseOutCubic (0, 1, t);
				//value = EaseOutElastic(0, 1, t);
				//value = EaseOutQuart(0, 1, t);
				//value = EaseInOutBounce(0, 1, t);
				value = EaseOutExpo(0, 1, t);

				//Debug.Log ("f(" + t + ") = " + value);
				//newPos = Vector3.Lerp (transform.position, vecImpacto, value * Time.fixedDeltaTime);
				//Debug.Log (newPos);

				transform.position = Vector3.Lerp (transform.position, vecImpacto, value * Time.fixedDeltaTime);

				//Debug.Log ("VecImpacto: " + vecImpacto);
			}*/

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

	void Impact(Vector3 direccion, float force){
		if (!isLocalPlayer)
			return;

		direccion.Normalize ();
		tiempoInicial = Time.time;
		Debug.Log ("Tiempo Inicial: " + tiempoInicial);

		if (direccion.y < 0)
			direccion.y = -direccion.y;

		vecImpacto = transform.position + (direccion.normalized * force / masa);
		vecImpacto2 = (direccion.normalized * force / masa); 
	}

	public static float EaseOutCubic(float start, float end, float value)
	{
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= .5f;
		end -= start;
		if (value < 1) return end * 0.5f * value * value * value * value * value + start;
		value -= 2;
		return end * 0.5f * (value * value * value * value * value + 2) + start;
	}

	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;

		float d = 1f;
		float p = d * .3f;
		float s;
		float a = 0;

		if (value == 0) return start;

		if ((value /= d) == 1) return start + end;

		if (a == 0f || a < Mathf.Abs(end))
		{
			a = end;
			s = p * 0.25f;
		}
		else
		{
			s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
		}

		return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
	}

	public static float EaseOutQuart(float start, float end, float value)
	{
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
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

	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}

}
