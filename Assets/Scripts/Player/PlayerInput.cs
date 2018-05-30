using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerInput : NetworkBehaviour
{

    [SerializeField]    private float velocidad = 9f;
    [SerializeField]    private float velocidadZombie = 9f;
    [SerializeField]    private float velocidadHuman = 14f;
    [SerializeField]    private float sensibilidad = 3f;

	private bool salto;

    private PlayerMotor motor;
	private RecibirImpacto impact;
	[SerializeField]
	PlayerInventory playerInventory;
	[SerializeField]
	PlayerPickUp playerPickUp;
	[SerializeField]
	ZombieHumanController zhController;

   // private ZombieHumanController zombieHumanController;

    private void Awake()
    {
       /* zombieHumanController = GetComponent<ZombieHumanController>();
        if (zombieHumanController == null)
            Debug.LogError("ZombieHumanController script not found");*/
    }

    void Start()
    {
		//Obtener componente PLayerMotor
        motor = GetComponent<PlayerMotor>();
		impact = GetComponent<RecibirImpacto> ();

        /*if (zombieHumanController.isZombie)
        {
            velocidad = velocidadZombie;
        }
        else
        {
            velocidad = velocidadHuman;
        }*/
    }

	/*void Update()
	{
		if (!salto) {
			salto = Input.GetButtonDown ("Jump");
		}
	}*/
    void FixedUpdate()
    {
		
        //Calcular la velocidad de movimiento como vector 3d

        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

		//Debug.Log ("Input Horizontal -> " + xMov);
		//Debug.Log ("Input Vertical -> " + zMov);

        //Calcular el vector de movimiento 3d
        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

		Vector3 movFinal = (movHorizontal + movVertical).normalized * velocidad;

        //Aplicar el movimiento
		motor.Move(movFinal, zMov, xMov);
		//motor.Jump (salto);
		//salto = false;

        //Calcular la rotacion en vector 3d
        float yRot = Input.GetAxis("Mouse X");

        Vector3 rotacion = new Vector3(0, yRot, 0) * sensibilidad;

        //Aplicar la rotacion
        motor.Rotar(rotacion);

		/* //Calcular la rotacion de la camara como vector 3d
        float xRot = Input.GetAxis("Mouse Y");

        float camRotacionX = xRot * sensibilidad;

        Aplicar la rotacion de la camara --> Implementacion 
        motor.RotarCamara(camRotacionX);
		motor.UpDownCamera (camRotacionX);
		*/
    }

	public void Update()
	{
		if (!isLocalPlayer)
			return;
		
		//Aplicar fuerza impacto 
		if(Input.GetKeyDown(KeyCode.F)){
			Debug.Log ("Aplicar fuerza");
			impact.RpcAddImpact (new Vector3 (1, 0, 1), 30, 1);
		}

		if (Input.GetButtonDown ("Fire1")) {
			if (zhController.isZombie) {
				playerInventory.CmdActivateRecurso ();
			} else {
				playerPickUp.CmdPickUpAction ();
			}
		} else if (!zhController.isZombie) {
			playerPickUp.CmdCantPickUpAction ();
		}
		if (Input.GetButtonUp ("Fire1"))
			playerInventory.CmdDeactivateRecurso ();

		if (Input.GetButtonDown ("L1") && zhController.isZombie)
			playerInventory.CmdPressL1 ();

		if (Input.GetButtonDown ("R1") && zhController.isZombie)
			playerInventory.CmdPressR1 ();

		if (Input.GetKeyDown(KeyCode.B) && zhController.isZombie) {
			playerInventory.CmdActivatePowerUp ();
		}


	}

    public void ChangeVelocity(bool isZombieEnter)
    {
        if (isZombieEnter)
        {
            velocidad = velocidadZombie;
        }
        else
        {
            velocidad = velocidadHuman;
        }
    }
}