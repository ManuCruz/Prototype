using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_isJumping = false;

	enum state{stop, right, left};
	private state m_status = state.stop;

	private float m_semiWidth;
	private bool m_toR = false;
	private bool m_toL = false;

	private float m_absLimit;

	void Start () {
		m_RG = GetComponent<Rigidbody>();

		m_semiWidth = Camera.main.pixelWidth/2;

		float worldSize = GameObject.FindGameObjectWithTag(Tags.world).GetComponent<SizeScript>().worldSize;
		m_absLimit = (worldSize + transform.localScale.z)/2;
	}

	void Update () {
		GetInput ();
		DoMovement ();
		DoTransition ();

		//QUIT
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();
	}

	void GetInput(){
		m_toR = false;
		m_toL = false;

		//movil
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary){
				if (Input.GetTouch(i).position.x < m_semiWidth)
					m_toL = true;
				else
					m_toR = true;
			}
		}

		//PC
		if (Input.GetKey (KeyCode.RightArrow))
			m_toR = true;
		if (Input.GetKey (KeyCode.LeftArrow))
			m_toL = true;
	}

	void DoMovement(){
		if (!m_isJumping && m_toR && m_toL) {  //Jump
			m_RG.velocity = (transform.up * jumpForce);
		//	m_RG.AddRelativeForce (transform.up * jumpForce);
			m_isJumping = true;
		} else if (m_toR && !m_toL) {  //Right
			if (m_status == state.stop)
				transform.Rotate (transform.up, -90);
			if (m_status == state.left)
				transform.Rotate (transform.up, 180);
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
			
			m_status = state.right;
		} else if (m_toL && !m_toR) {  //Left
			if (m_status == state.stop)
				transform.Rotate (transform.up, 90);
			if (m_status == state.right)
				transform.Rotate (transform.up, 180);
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
			
			m_status = state.left;
		} else if (!m_toL && !m_toR){  //Stop
			if (m_status == state.right)
				transform.Rotate (transform.up, 90);
			if (m_status == state.left)
				transform.Rotate (transform.up, -90);
			
			m_status = state.stop;
		}
	}
		
	void DoTransition(){
		float dotForward = Vector3.Dot (transform.position, transform.forward);
		
		if (dotForward > m_absLimit && m_toR) {
			AjustPosition();
			transform.Rotate (transform.up, 90);
		}
		else if (dotForward > m_absLimit && m_toL) {
			AjustPosition();
			transform.Rotate (transform.up, -90);
		}

	}

	void AjustPosition(){
		if (transform.position.x > m_absLimit)
			transform.position = new Vector3(m_absLimit, transform.position.y, transform.position.z);
		else if (transform.position.x < -m_absLimit)
			transform.position = new Vector3(-m_absLimit, transform.position.y, transform.position.z);
		else if (transform.position.y > m_absLimit)
			transform.position = new Vector3(transform.position.x, m_absLimit, transform.position.z);
		else if (transform.position.y < -m_absLimit)
			transform.position = new Vector3(transform.position.x, -m_absLimit, transform.position.z);
		else if (transform.position.z > m_absLimit)
			transform.position = new Vector3(transform.position.x, transform.position.y, m_absLimit);
		else if (transform.position.z < -m_absLimit)
			transform.position = new Vector3(transform.position.x, transform.position.y, -m_absLimit);
	}

	public void ResetJump(){
		m_isJumping = false;
	}
}
