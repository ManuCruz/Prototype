using UnityEngine;
using System.Collections;

public class RotateWorld : MonoBehaviour {


	public float paddingLimit = 1f;
	public float angleVision = 45;
	public float rotationSpeed = 5f;

	private Transform m_player;
	private PlayerMovement movement;
	private Quaternion m_orientation;
	private float anglesFromRotate;
	private float anglesFromRotateUp;
	
	private bool leftSide = false;
	private bool rightSide = false;
	private bool topSide = false;
	private bool bottomSide = false;

	private float m_absLimit;

	void Start () {
		m_player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		movement = m_player.gameObject.GetComponent<PlayerMovement> ();
		m_orientation = Quaternion.Euler (0, 0, 0);
		anglesFromRotate = 0;

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
		transform.localRotation = Quaternion.Lerp (transform.localRotation, m_orientation, rotationSpeed * Time.deltaTime);
	}

	void MoveCamera(){
		Camera.main.transform.position = new Vector3 (m_player.position.x, m_player.position.y, Camera.main.transform.position.z);	
	}

	void actualAngles() {
		float absLimit = Mathf.Max (Mathf.Abs (m_player.localPosition.x), Mathf.Abs (m_player.localPosition.y), Mathf.Abs (m_player.localPosition.z));
	
		if (absLimit == Mathf.Abs (m_player.localPosition.z)) {
			anglesFromRotate = (m_player.localPosition.z < 0) ? 0 : 180;
			anglesFromRotateUp = 0;
			if (anglesFromRotate == 0){
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
			anglesFromRotate = (m_player.localPosition.x > 0) ? 90 : -90;
			anglesFromRotateUp = 0;
			if(anglesFromRotate == 90){
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
		}
	}

	void LookWorld(){
		//setlookrotation crea una rotacion con  el forward al que mirar y donde esta el up
		//HORIZONTAL
		if (movement.isPlayerToRight() || movement.isPlayerToLeft()){
			float dotForward = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.forward);
			dotForward = Mathf.Abs (dotForward);
			if (dotForward > m_absLimit && rightSide) {
				m_orientation = Quaternion.Euler (0, anglesFromRotate + angleVision, 0);
			} else if (dotForward < m_absLimit && rightSide) {
				m_orientation = Quaternion.Euler (0, anglesFromRotate, 0);
			}

			if (dotForward > m_absLimit && leftSide) {
				m_orientation = Quaternion.Euler (0, anglesFromRotate - angleVision, 0);
			} else if (dotForward < m_absLimit && leftSide) {	
				m_orientation = Quaternion.Euler (0, anglesFromRotate, 0);
			}
		}
		//VERTICAL
		
		float dotUp = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.up);
		dotUp = Mathf.Abs (dotUp);

		if (dotUp > m_absLimit && topSide) {
			if (anglesFromRotate == 0 || anglesFromRotate == 180){
				m_orientation = Quaternion.Euler (anglesFromRotateUp - angleVision, 0, 0);
				//m_orientation = Quaternion.Euler (anglesFromRotate + angleVision,0, 0);
				//m_orientation = Quaternion.AngleAxis(anglesFromRotate - angleVision, transform.right);
			}else {
				//m_orientation = Quaternion.AngleAxis(anglesFromRotate - angleVision, transform.forward);
				m_orientation = Quaternion.Euler (transform.localRotation.x, transform.localRotation.y, anglesFromRotateUp - angleVision);
				//m_orientation = Quaternion.Euler (anglesFromRotate + angleVision, 0, 0);
			}

		} else if (dotUp < m_absLimit && topSide) {
			if (anglesFromRotate == 0 || anglesFromRotate == 180)
				m_orientation = Quaternion.Euler(anglesFromRotateUp, transform.localRotation.y, transform.localRotation.z);
			else 
				m_orientation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, anglesFromRotateUp);	
		}
		/*
		if (dotUp > m_absLimit && bottomSide) {
			m_orientation = Quaternion.Euler (anglesFromRotateUp + angleVision, transform.localRotation.y, transform.localRotation.z);
		} else if (dotUp < m_absLimit && bottomSide) {
			m_orientation = Quaternion.Euler (anglesFromRotateUp, transform.localRotation.y, transform.localRotation.z);
		}*/

	}
}
