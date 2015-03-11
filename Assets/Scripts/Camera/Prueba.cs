using UnityEngine;
using System.Collections;

public class Prueba : MonoBehaviour {


	public float paddingLimit = 1f;
	public float angleVision = 45;
	public float rotationSpeed = 5f;

	private Transform m_player;
	private PlayerMovement movement;
	private Quaternion m_orientation;

	private float anglesFromRotate;
	private float anglesFromRotateUp;

	//private float actualUpRotation;
	//private float actualForwardRotation;
	private float m_angle;
	private bool leftSide = false;
	private bool rightSide = false;
	private bool topSide = false;
	private bool bottomSide = false;

	private bool rotationAplied = false;
	private bool rotationUpAplied = false;
	private bool rotationOther = false;

	private int state = 0;
	private int prevState = 0;

	private Vector3 rotation = Vector3.zero;

	private float m_absLimit;

	void Start () {
		m_player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		movement = m_player.gameObject.GetComponent<PlayerMovement> ();
		m_orientation = Quaternion.Euler (0, 0, 0);
		anglesFromRotate = 0;
		//actualUpRotation = 0;
		//actualForwardRotation = 0;
		m_angle = angleVision;
		Vector3 inverseTransform = m_player.InverseTransformVector (m_player.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;
	}

	void Update () {
		actualAngles();
		LookWorld();
		rotateWorld();
		MoveCamera();
	}


	void rotateWorld(){
		Debug.Log ("Antes:" + transform.eulerAngles);
		transform.eulerAngles = rotation;
		/*transform.eulerAngles.Set(transform.eulerAngles.x + rotation.x, 
		                          transform.eulerAngles.y + rotation.y,
		                          transform.eulerAngles.z + rotation.z);*/
		Debug.Log ("rotacion:" + rotation);
		
		Debug.Log ("Despues:" + transform.eulerAngles);
		/*IDEA Para hacerlo poco a poco:
		 v3Current = Vector3.Lerp(v3Current, v3To, Time.deltaTime * speed);
     transform.eulerAngles = v3Current; */
	}

	void MoveCamera(){
		Camera.main.transform.position = new Vector3 (m_player.position.x, m_player.position.y, Camera.main.transform.position.z);	
	}

	void actualAngles() {
		float absLimit = Mathf.Max (Mathf.Abs (m_player.localPosition.x), Mathf.Abs (m_player.localPosition.y), Mathf.Abs (m_player.localPosition.z));

		if (absLimit == Mathf.Abs (m_player.localPosition.z)) {
			state = 0;
			anglesFromRotate = (m_player.localPosition.z < 0) ? 0 : 180;
			anglesFromRotateUp = 0;
			if (anglesFromRotate == 0){
				m_angle = angleVision;
				rightSide = (m_player.localPosition.x > 0);
				leftSide = (m_player.localPosition.x < 0);
				topSide =  (m_player.localPosition.y > 0);
				bottomSide =  (m_player.localPosition.y < 0);
				
			}else{
				rightSide = (m_player.localPosition.x < 0);
				leftSide = (m_player.localPosition.x > 0);
				topSide =  (m_player.localPosition.y > 0);
				bottomSide =  (m_player.localPosition.y < 0);
			}
		} else if (absLimit == Mathf.Abs (m_player.localPosition.x)) {
			state = 1;
			anglesFromRotate = (m_player.localPosition.x > 0) ? 90 : -90;
			anglesFromRotateUp = 0;
			if(anglesFromRotate == 90){
				//m_angle = -angleVision;
				
				rightSide = (m_player.localPosition.z > 0);
				leftSide = (m_player.localPosition.z < 0);
				topSide =  (m_player.localPosition.y > 0);
				bottomSide =  (m_player.localPosition.y < 0);
			}else{
				rightSide = (m_player.localPosition.z < 0);
				leftSide = (m_player.localPosition.z > 0);
				topSide =  (m_player.localPosition.y > 0);
				bottomSide =  (m_player.localPosition.y < 0);
			}
		}else if (absLimit == Mathf.Abs (m_player.localPosition.y)) {
			state = 2;
			anglesFromRotateUp = (m_player.localPosition.y > 0) ? -90 : 90;
			if(anglesFromRotateUp == -90){
				rightSide = (m_player.localPosition.x > 0);
				leftSide = (m_player.localPosition.x < 0);
				topSide =  (m_player.localPosition.z > 0);
				bottomSide =  (m_player.localPosition.z < 0);
			}else{
				rightSide = (m_player.localPosition.x > 0);
				leftSide = (m_player.localPosition.x < 0);
				topSide =  (m_player.localPosition.z < 0);
				bottomSide =  (m_player.localPosition.z > 0);
			}
		}
	}

	void LookWorld(){
		if (state != prevState) {
			rotationAplied = false;
			rotationUpAplied = false;
		}

		LookHorizontal();
		LookVertical();
		prevState = state;
	}

	void LookHorizontal(){
		//HORIZONTAL
		float dotForward = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.forward);
		dotForward = Mathf.Abs (dotForward);
		if (dotForward > m_absLimit && movement.isPlayerToRight()){
			if (!rotationAplied){
				//m_orientation = transform.rotation * Quaternion.Euler (0, m_angle,0);
				rotation.y += m_angle;
				rotationAplied = true;
				rotationOther = true;
			}
		}
		/*PROBAR:si es dot forward despues de hacer el movimiento es menor que el limite se pone a cero el aplied*/
		if (dotForward > m_absLimit && movement.isPlayerToLeft()) {
			if (!rotationAplied){
				//m_orientation = transform.rotation * Quaternion.Euler (0, -m_angle,0);
				rotation.y -= m_angle;
				rotationAplied = true;
				rotationOther = true;
			}
		}
	}

	void LookVertical(){
		//VERTICAL
		
		float dotUp = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.up);
		dotUp = Mathf.Abs (dotUp);
		if (dotUp > m_absLimit) {
			if (!rotationUpAplied){
					//m_orientation = transform.rotation * Quaternion.Euler (-m_angle,0,0);	
				rotation.x -= m_angle;
				rotationUpAplied = true;
			}
		}
	}

	/*Transform.rotation is relative to the world, but you use it wrong ;)

.rotation returns a Quaternion. You shouldn't access the members of a Quaternion unless you know what you're doing. I guess you want the rotation in Euler angles. That's what you see in the inspector.

transform.rotation.eulerAngles.x will give you the x-rotation in world space.
transform.localRotation.eulerAngles.x will give you the x-rotation in object space.

Note that tranform has two shortcut properties to access them faster ;)

transform.eulerAngles is the same as transform.rotation.eulerAngles

transform.localEulerAngles is the same as transform.localRotation.eulerAngles

The localEulerAngles are the ones you can see in the inspector.*/
}
