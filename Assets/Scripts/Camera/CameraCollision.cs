using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	public Transform target;

	[SerializeField]
	GameObject player;

	[System.Serializable]
	public class PositionSettings
	{
		public Vector3 targetPosOffset = new Vector3 (0,3.4f,0);
		public float lookSmooth = 100f;
		public float distanceFromTarget = -8;
		public bool smoothFollow = true;
		public float smooth = 0.05f;

		[HideInInspector]
		public float adjustmentDistance = -8;
	}

	[System.Serializable]
	public class OrbitSettings
	{
		public float xRotation = -20;
		public float maxXRotation = 25;
		public float minXRotation = -85;
		public float vOrbitSmooth = 150;
	}

	[System.Serializable]
	public class DebugSettings
	{
		public bool drawDesiredCollisionLines = true;
		public bool drawAdjustedCollisionLines = true;
	}

	public OrbitSettings orbit = new OrbitSettings ();
	public PositionSettings position = new PositionSettings ();
	public DebugSettings debug = new DebugSettings();
	public CollisionHandler collision = new CollisionHandler ();


	Vector3 targetPos = Vector3.zero;
	Vector3 destination = Vector3.zero;
	float vOrbitInput;
	Vector3 adjustedDestination = Vector3.zero;
	Vector3 camVel = Vector3.zero;


	[System.Serializable]
	public class CollisionHandler
	{
		public LayerMask collisionLayer;

		[HideInInspector]
		public bool colliding = false;
		[HideInInspector]
		public Vector3[] adjustedCameraClipPoints;
		[HideInInspector]
		public Vector3[] desiredCameraClipPoints;

		Camera camera;

		public void Initialize(Camera cam)
		{
			camera = cam;
			adjustedCameraClipPoints = new Vector3[5];
			desiredCameraClipPoints = new Vector3[5];
		}

		public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
		{
			if (!camera)
				return;

			intoArray = new Vector3[5];

			float z = camera.nearClipPlane;
			float x = Mathf.Tan (camera.fieldOfView / 3.41f) * z;
			float y = x / camera.aspect;

			//top-left
			intoArray[0] = (atRotation* new Vector3 (-x,y,z))+cameraPosition;
			//top-right
			intoArray[1] = (atRotation* new Vector3 (x,y,z))+cameraPosition;
			//bottom-left
			intoArray[2] = (atRotation* new Vector3 (-x,-y,z))+cameraPosition;
			//botom-right
			intoArray[3] = (atRotation* new Vector3 (x,-y,z))+cameraPosition;
			//camera pos
			intoArray[4] = cameraPosition - camera.transform.forward;
		}

		bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
		{
			for (int i = 0; i < clipPoints.Length; i++) {
				Ray ray = new Ray(fromPosition,clipPoints[i]-fromPosition);
				float distance = Vector3.Distance(clipPoints[i],fromPosition);
				if (Physics.Raycast (ray, distance, collisionLayer)) {
					return true;
				}
			}
			return false;
		}

		public float GetAdjustedDistanceWithRayFrom(Vector3 from)
		{
			float distance = -1;

			for (int i = 0; i < desiredCameraClipPoints.Length; i++) {
				Ray ray = new Ray (from, desiredCameraClipPoints[i]-from);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {

					if (distance == -1)
						distance = hit.distance;
					else {
						if (hit.distance < distance)
							distance = hit.distance;
					}

				}
			}

			if (distance == -1)
				return 0;
			else
				return distance;
		}

		public void CheckColliding(Vector3 targetPosition)
		{
			colliding = CollisionDetectedAtClipPoints (desiredCameraClipPoints, targetPosition);
		}
	}



	// Use this for initialization
	void Start () {
		SetCameraTarget (target);

		vOrbitInput = 0;

		MoveToTarget ();

		collision.Initialize (GetComponent<Camera>());
		collision.UpdateCameraClipPoints (transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
		collision.UpdateCameraClipPoints (destination, transform.rotation, ref collision.desiredCameraClipPoints);
	}

	void SetCameraTarget(Transform t){
		target = t;

		if (target == null) {
			Debug.LogError ("Your camera needs a target.");
		}
	}

	// Update is called once per frame
	void Update () {
		GetInput ();
		OrbitTarget ();
	}

	void LateUpdate()
	{
		LookAtTarget ();
		MoveToTarget ();
	}

	void FixedUpdate(){
		MoveToTarget ();
		collision.UpdateCameraClipPoints (transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
		collision.UpdateCameraClipPoints (destination, transform.rotation, ref collision.desiredCameraClipPoints);

		for (int i = 0; i < 5; i++) {
			if (debug.drawDesiredCollisionLines) {
				Debug.DrawLine (targetPos, collision.desiredCameraClipPoints [i], Color.white);
			}
			if (debug.drawAdjustedCollisionLines) {
				Debug.DrawLine (targetPos, collision.adjustedCameraClipPoints[i],Color.green);
			}
		}

		collision.CheckColliding (targetPos);
		position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom (targetPos);

	}

	void GetInput(){
		vOrbitInput = -Input.GetAxis("Mouse Y");
	}

	void MoveToTarget (){

		targetPos = target.position + position.targetPosOffset;
		destination = Quaternion.Euler (orbit.xRotation, 180+target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
		destination += targetPos;

		if (collision.colliding) {
			adjustedDestination = Quaternion.Euler (orbit.xRotation, 180+target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
			adjustedDestination += targetPos;

			if (position.smoothFollow) {
				transform.position = Vector3.SmoothDamp (transform.position,adjustedDestination, ref camVel, position.smooth);
			} else {
				transform.position = adjustedDestination;
			}
		} else {
			if (position.smoothFollow) {
				transform.position = Vector3.SmoothDamp (transform.position,destination, ref camVel, position.smooth);
			} else {
				transform.position = destination;
			}
		}
	}
	void LookAtTarget(){
		Quaternion targetRotation = Quaternion.LookRotation (targetPos - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation,position.lookSmooth*Time.deltaTime);
	}

	void OrbitTarget(){
		

		orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;

		if (orbit.xRotation > orbit.maxXRotation) 
			orbit.xRotation = orbit.maxXRotation;

		if (orbit.xRotation < orbit.minXRotation)
			orbit.xRotation = orbit.minXRotation;
	}
}
