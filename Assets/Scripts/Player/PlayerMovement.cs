﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_isJumping = false;
	private bool m_hasLanded = true;

	enum state{stop, right, left};
	private state m_status = state.stop;

	private float m_semiWidthScreen;
	private bool m_toR = false;
	private bool m_toL = false;

	private float m_absLimit;

	private JumpCollisionScript m_jumpCollisionScript;

	void Start () {
		m_RG = GetComponent<Rigidbody>();

		m_semiWidthScreen = Camera.main.pixelWidth/2;

		Vector3 inverseTransform = transform.InverseTransformVector (transform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z));

		m_jumpCollisionScript = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<JumpCollisionScript>();
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
				if (Input.GetTouch(i).position.x < m_semiWidthScreen)
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

		//TEST
		if (Input.GetKeyDown (KeyCode.Space)){
			m_RG.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
		}
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate (transform.up * speedMovement * Time.deltaTime, Space.World);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate (transform.up * -speedMovement * Time.deltaTime, Space.World);
	}

	void DoMovement(){
		if (!m_isJumping && m_toR && m_toL) {  //Jump
			m_RG.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
			m_isJumping = true;
			m_hasLanded = false;
		} 

		if(m_hasLanded && ((m_toR && !m_toL) || (!m_toR && m_toL)))
			m_isJumping = false;

		if (m_toR && !m_toL) {  //Right
			if (m_status == state.stop)
				transform.Rotate (transform.up, -90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, 180, Space.World);
			m_status = state.right;
		} else if (m_toL && !m_toR) {  //Left
			if (m_status == state.stop)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.right)
				transform.Rotate (transform.up, 180, Space.World);
			m_status = state.left;
		} else if (!m_toL && !m_toR){  //Stop
			if (m_status == state.right)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, -90, Space.World);
			m_status = state.stop;
		}

		if (m_toR && m_status == state.right && !m_jumpCollisionScript.IsCollidingWithWall ())
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);

		if (m_toL && m_status == state.left && !m_jumpCollisionScript.IsCollidingWithWall ())
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
	}
		
	void DoTransition(){
		float dotForward = Vector3.Dot (transform.position, transform.forward);
		
		if (dotForward > m_absLimit && m_toR) { //right side
			transform.Rotate (transform.up, -90, Space.World);
			AjustPosition();
		}
		else if (dotForward > m_absLimit && m_toL) { //left side
			transform.Rotate (transform.up, 90, Space.World);
			AjustPosition();
		}

		float dotUp = Vector3.Dot (transform.position, transform.up);

		if (dotUp > m_absLimit || dotUp < -m_absLimit) { //up and down sides
			bool toUp = dotUp > m_absLimit;

			Vector3 vel = m_RG.velocity;  //get the velocity
			m_RG.velocity = new Vector3(0,0,0);

			switch (m_status){
			case state.stop: 
				transform.Rotate (transform.right, toUp? -90 : 90, Space.World);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.right) * vel;
				break;
			case state.right:
				transform.Rotate (transform.forward, toUp? 90 : -90, Space.World);
				vel = Quaternion.AngleAxis(toUp? 90 : -90, transform.forward) * vel;
				break;
			case state.left:
				transform.Rotate (transform.forward, toUp? -90 : 90, Space.World);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.forward) * vel;
				break;
			}

			AjustPosition();

			m_RG.velocity = vel;  //restore the velocity (no se usa AddForce, porque ese metodo actualiza en el fixedupdate y ocasiona los parones)
		}
	}

	void AjustPosition(){
		Vector3 inverseTransform = transform.InverseTransformVector (transform.position);

		if (inverseTransform.x > m_absLimit)
			inverseTransform = new Vector3(m_absLimit, inverseTransform.y, inverseTransform.z);
		else if (inverseTransform.x < -m_absLimit)
			inverseTransform = new Vector3(-m_absLimit, inverseTransform.y, inverseTransform.z);

		if (inverseTransform.y > m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, m_absLimit, inverseTransform.z);
		else if (inverseTransform.y < -m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, -m_absLimit, inverseTransform.z);

		if (inverseTransform.z > m_absLimit)
			inverseTransform = new Vector3 (inverseTransform.x, inverseTransform.y, m_absLimit);
		else if (inverseTransform.z < -m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, inverseTransform.y, -m_absLimit);
		
		transform.position = transform.TransformVector (inverseTransform);
	}

	public void ResetJump(){
		m_hasLanded = true;
	}

	public bool isPlayerToRight(){
		return m_toR;
	}
	
	public bool isPlayerToLeft(){
		return m_toL;
	}
	public bool isPlayerToUp(){
		float dotProduct = Vector3.Dot(transform.InverseTransformDirection(m_RG.velocity), new Vector3(1,1,1));
		//Debug.Log("dotProduct:" + dotProduct);
		if (dotProduct > 0)
			return true;
		else
			return false;
	}
	public bool isPlayerToDown(){
		float dotProduct = Vector3.Dot(transform.InverseTransformDirection(m_RG.velocity), new Vector3(1,1,1));
		if (dotProduct < 0)
			return true;
		else
			return false;
	}
}
