using UnityEngine;
using System.Collections;

public class Prueba : MonoBehaviour {
	
	
	public float paddingLimit = 1f;
	public float rotationSpeed = 5f;
	
	//Objetos relacionados
	private Transform m_playerTransform;
	private PlayerMovement m_playerMovement;
	
	//Temas de rotacion
	private float m_angleX = 0;
	private float m_angleY = 0;

	//Cambiar de cara
//	private int m_state = 0;
//	private int m_prevState = 0;
	private float m_angleVision = 45;
	
	//Limites de Cara
	private float m_absLimit;
	private float m_prevPositionX = 0;
	private float m_prevPositionY = 0;

	void Start () {
		m_playerTransform = GameObject.FindGameObjectWithTag (Tags.player).transform;
		m_playerMovement = m_playerTransform.gameObject.GetComponent<PlayerMovement> ();

		Vector3 inverseTransform = m_playerTransform.InverseTransformVector (m_playerTransform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;
	}
	
	void Update () {
//		actualState();
		LookWorld();

		MoveCamera();
	}

	void MoveCamera(){
		Camera.main.transform.position = new Vector3 (m_playerTransform.position.x, m_playerTransform.position.y, Camera.main.transform.position.z);	
	}
	
/*	void actualState() {
		float absLimit = Mathf.Max (Mathf.Abs (m_playerTransform.localPosition.x), Mathf.Abs (m_playerTransform.localPosition.y), Mathf.Abs (m_playerTransform.localPosition.z));
		
		if (absLimit == Mathf.Abs (m_playerTransform.localPosition.z)) {
			m_state = 0;
		} else if (absLimit == Mathf.Abs (m_playerTransform.localPosition.x)) {
			m_state = 1;
		}else if (absLimit == Mathf.Abs (m_playerTransform.localPosition.y)) {
			m_state = 2;
		}
	}*/
	
	void LookWorld(){
		//HORIZONTAL
		float dotForward = Vector3.Dot (m_playerTransform.InverseTransformVector (m_playerTransform.position), Vector3.forward);
		dotForward = Mathf.Abs (dotForward);
		
		bool rotateX = false;
		float angleX = 0;
		if (dotForward > m_absLimit && m_playerMovement.isPlayerToRight ()) {
			if (m_prevPositionX < m_absLimit ) {
				rotateX = true;
				angleX = m_angleVision;
			}
			m_prevPositionX = dotForward;
		}
		if (dotForward < m_absLimit && m_playerMovement.isPlayerToRight ()) {
			if(m_prevPositionX > m_absLimit){
				rotateX = true;
				angleX = m_angleVision;
			}
			m_prevPositionX = dotForward;
		}
		
		if (dotForward > m_absLimit && m_playerMovement.isPlayerToLeft()) {
			if (m_prevPositionX < m_absLimit){
				rotateX = true;
				angleX = -m_angleVision;
			}
			m_prevPositionX = dotForward;
		}
		
		if (dotForward < m_absLimit && m_playerMovement.isPlayerToLeft ()) {
			if(m_prevPositionX > m_absLimit){
				rotateX = true;
				angleX = -m_angleVision;
			}
			m_prevPositionX = dotForward;
		}
		
		//VERTICAL
		float dotUp = Vector3.Dot (m_playerTransform.InverseTransformVector (m_playerTransform.position), Vector3.up);
		dotUp = Mathf.Abs (dotUp);
		
		bool rotateY = false;
		float angleY = 0;
		if (dotUp > m_absLimit && m_playerMovement.isPlayerToUp ()) {
			if (m_prevPositionY < m_absLimit) {
				rotateY = true;
				angleY = -m_angleVision;
			}
			m_prevPositionY = dotUp;
		} 
		if (dotUp < m_absLimit && m_playerMovement.isPlayerToUp ()) {
			if(m_prevPositionY > m_absLimit){
				rotateY = true;
				angleY = -m_angleVision;
			}
			m_prevPositionY = dotUp;
		}
		
		if (dotUp > m_absLimit && m_playerMovement.isPlayerToDown()) {
			if (m_prevPositionY < m_absLimit){
				rotateY = true;
				angleY = m_angleVision;
			}
			m_prevPositionY = dotUp;
		}
		if (dotUp < m_absLimit && m_playerMovement.isPlayerToDown ()) {
			if(m_prevPositionY > m_absLimit){
				rotateY = true;
				angleY = m_angleVision;
			}
			m_prevPositionY = dotUp;
		}
		
		if (rotateX) {
			m_angleX += angleX;
			m_angleX = Mathf.Repeat(m_angleX, 360);
			float aux_angleX = Mathf.Repeat(m_angleX, 90);

			if(aux_angleX == 0){
				transform.Rotate(transform.up * -angleX, Space.World);	
				transform.Rotate(transform.right * -m_angleY, Space.World);
				transform.Rotate(transform.up * angleX, Space.World);	
			}

			transform.Rotate(transform.up * angleX, Space.World);	

			if(aux_angleX == 0)
				transform.Rotate(transform.right * m_angleY, Space.World);
		}
		
		if (rotateY) {
			m_angleY += angleY;
			m_angleY = Mathf.Repeat(m_angleY, 360);
			float aux_angleY = Mathf.Repeat(m_angleY, 90);

			if(aux_angleY == 0){
				transform.Rotate(transform.right * angleY, Space.World);	
				transform.Rotate(transform.up * -m_angleX, Space.World);
				transform.Rotate(transform.right * -angleY, Space.World);	
			}
			
			transform.Rotate(transform.right * angleY, Space.World);	
			
			if(aux_angleY == 0)
				transform.Rotate(transform.up * m_angleX, Space.World);
		}
	}
	
}
