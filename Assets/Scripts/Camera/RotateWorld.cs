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

	private float actualUpRotation;
	private float actualForwardRotation;
	
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
		actualUpRotation = 0;
		actualForwardRotation = 0;
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
		//VERTICAL
		actualUpRotation = anglesFromRotateUp;

		float dotUp = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.up);
		dotUp = Mathf.Abs (dotUp);
		
		if (dotUp > m_absLimit && topSide)
			if (anglesFromRotate == 0)
				actualUpRotation = anglesFromRotateUp - angleVision;
			else if (anglesFromRotate == 180)
				actualUpRotation = anglesFromRotateUp + angleVision;
			else if (anglesFromRotate == -90)
				actualUpRotation = anglesFromRotateUp + angleVision;
			else
				actualUpRotation = anglesFromRotateUp - angleVision;
			
		else if (dotUp < m_absLimit && topSide)
			actualUpRotation = anglesFromRotateUp;

		/*
		if (dotUp > m_absLimit && bottomSide) {
			m_orientation = Quaternion.Euler (anglesFromRotateUp + angleVision, transform.localRotation.y, transform.localRotation.z);
		} else if (dotUp < m_absLimit && bottomSide) {
			m_orientation = Quaternion.Euler (anglesFromRotateUp, transform.localRotation.y, transform.localRotation.z);
		}*/

		//HORIZONTAL
		if (movement.isPlayerToRight() || movement.isPlayerToLeft()){
			float dotForward = Vector3.Dot (m_player.InverseTransformVector (m_player.position), Vector3.forward);
			dotForward = Mathf.Abs (dotForward);

			if (dotForward > m_absLimit && rightSide)
				actualForwardRotation = anglesFromRotate + angleVision;
			else if (dotForward < m_absLimit && rightSide)
				actualForwardRotation = anglesFromRotate;

			if (dotForward > m_absLimit && leftSide) {
				actualForwardRotation = anglesFromRotate - angleVision;
			} else if (dotForward < m_absLimit && leftSide) {	
				actualForwardRotation = anglesFromRotate;
			}
		}
	
		if (anglesFromRotate == 90 || anglesFromRotate == -90)
			m_orientation = Quaternion.Euler (0, actualForwardRotation, actualUpRotation);
		else 
			m_orientation = Quaternion.Euler (actualUpRotation, actualForwardRotation, 0);
		


	}
}
