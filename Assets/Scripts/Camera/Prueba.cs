using UnityEngine;
using System.Collections;

public class Prueba : MonoBehaviour {


	public float paddingLimit = 1f;
	public float angleVision = 45;
	public float rotationSpeed = 5f;

	//Objetos relacionados
	private Transform m_parentWorld;
	private Transform m_player;
	private PlayerMovement movement;

	//Temas de rotacion
	private Vector3 m_angle;
	private bool rotationAplied = false;
	private bool rotationUpAplied = false;
	private Vector3 rotation = Vector3.zero;

	//Cambiar de cara
	private int state = 0;
	private int prevState = 0;
	private bool changeFace = false;

	//Limites de Cara
	private float m_absLimit;
	private float prevPlayerForward = 0;
	private float prevPlayerUp = 0;	

	void Start () {
		m_parentWorld = GameObject.FindGameObjectWithTag(Tags.axis).transform;
		m_player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		movement = m_player.gameObject.GetComponent<PlayerMovement> ();

		m_angle.x = angleVision;
		m_angle.y = angleVision;
		m_angle.z = angleVision;

		Vector3 inverseTransform = m_player.InverseTransformVector (m_player.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;
	}

	void Update () {
		actualAngles();
		LookWorld();
		rotateWorld();
		MoveCamera();
		rotation = Vector3.zero;
	}


	void rotateWorld(){
		transform.Rotate(rotation, Space.World);
	}

	void MoveCamera(){
		Camera.main.transform.position = new Vector3 (m_player.position.x, m_player.position.y, Camera.main.transform.position.z);	
	}

	void actualAngles() {
		float absLimit = Mathf.Max (Mathf.Abs (m_player.localPosition.x), Mathf.Abs (m_player.localPosition.y), Mathf.Abs (m_player.localPosition.z));

		if (absLimit == Mathf.Abs (m_player.localPosition.z)) {
			state = 0;
		} else if (absLimit == Mathf.Abs (m_player.localPosition.x)) {
			state = 1;
		}else if (absLimit == Mathf.Abs (m_player.localPosition.y)) {
			state = 2;
		}
	}

	void LookWorld(){
		if (state != prevState) {
			changeFace = true;
			//rotationAplied = false;
			//rotationUpAplied = false;
		}

		LookHorizontal();
		LookVertical();
		prevState = state;
	}

	void LookHorizontal(){
		//HORIZONTAL
		float dotForward = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.forward);
		dotForward = Mathf.Abs (dotForward);
		if (dotForward > m_absLimit && movement.isPlayerToRight ()) {
			if (!rotationAplied) {
				rotation += m_angle;
				rotation.x *= m_parentWorld.up.x;
				rotation.y *= m_parentWorld.up.y;
				rotation.z *= m_parentWorld.up.z;
				
				rotationAplied = true;
			}
			prevPlayerForward = dotForward;

		} else if (dotForward < m_absLimit && movement.isPlayerToRight () && changeFace) {
			rotation += m_angle;
			rotation.x *= m_parentWorld.up.x;
			rotation.y *= m_parentWorld.up.y;
			rotation.z *= m_parentWorld.up.z;
			changeFace = false;
			rotationAplied = false;
		}

		if (dotForward > m_absLimit && movement.isPlayerToLeft()) {
			if (!rotationAplied){
				rotation -= m_angle;
				rotation.x *= m_parentWorld.up.x;
				rotation.y *= m_parentWorld.up.y;
				rotation.z *= m_parentWorld.up.z;
				rotationAplied = true;
			}
			prevPlayerForward = dotForward;
		} else if (dotForward < m_absLimit && movement.isPlayerToLeft () && changeFace) {
			rotation -= m_angle;
			rotation.x *= m_parentWorld.up.x;
			rotation.y *= m_parentWorld.up.y;
			rotation.z *= m_parentWorld.up.z;
			changeFace = false;
			rotationAplied = false;
		}
	}

	void LookVertical(){
		//VERTICAL
		float dotUp = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.up);
		dotUp = Mathf.Abs (dotUp);
		if (dotUp > m_absLimit && movement.isPlayerToUp ()) {
			if (!rotationAplied) {
				rotation -= m_angle;
				rotation.x *= m_parentWorld.right.x;
				rotation.y *= m_parentWorld.right.y;
				rotation.z *= m_parentWorld.right.z;
				rotationAplied = true;
			}
			prevPlayerUp = dotUp;
		} else if (dotUp < m_absLimit && movement.isPlayerToUp () && changeFace) {
			rotation -= m_angle;
			rotation.x *= m_parentWorld.right.x;
			rotation.y *= m_parentWorld.right.y;
			rotation.z *= m_parentWorld.right.z;
			changeFace = false;
			rotationAplied = false;
		}

		if (dotUp > m_absLimit && movement.isPlayerToDown()) {
			if (!rotationAplied){
				rotation += m_angle;
				rotation.x *= m_parentWorld.right.x;
				rotation.y *= m_parentWorld.right.y;
				rotation.z *= m_parentWorld.right.z;
				rotationAplied = true;
			}
			prevPlayerUp = dotUp;
		}else if (dotUp < m_absLimit && movement.isPlayerToDown () && changeFace) {
			rotation += m_angle;
			rotation.x *= m_parentWorld.right.x;
			rotation.y *= m_parentWorld.right.y;
			rotation.z *= m_parentWorld.right.z;
			changeFace = false;
			rotationAplied = false;
		}
	}

}
