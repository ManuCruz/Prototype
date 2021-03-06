﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	public float speedMovement = 3f;
	public float jumpForce = 5f;

	public enum InputControls{
		normal,
		pc,
		swipe,
		doubleTap,
	};
	
	public InputControls inputControls = InputControls.normal;

	private Rigidbody m_RG;
	
	private bool m_isJumping = false;
	private bool m_hasLanded = true;
	private bool m_doJump = false;
	
	enum state{stop, right, left};
	private state m_status = state.stop;
	
	private float m_semiWidthScreen;
	private int m_toR = 0;
	private int m_toL = 0;

	private float m_absLimit;
	
	private JumpCollisionScript m_jumpCollisionScript;

	private float m_deltaToSwipe = 10;
	private int m_currentTouchIndex = 0;

	private int m_frameToMove = 5;

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
		AjustPosition();
	}

	void GetInput(){
		m_doJump = false;
		bool pressR = false;
		bool pressL = false;

		switch (inputControls) {
		case InputControls.normal:
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase != TouchPhase.Ended && Input.GetTouch (i).phase != TouchPhase.Canceled) {
					if (Input.GetTouch (i).position.x < m_semiWidthScreen){
						m_toL++;
						pressL = true;
					}
					else{
						m_toR++;
						pressR = true;
					}
				}
			}
			//JUMP
			if (pressR && pressL)
				m_doJump = true;
			
			if (!pressR)
				m_toR=0;
			if (!pressL)
				m_toL=0;
			break;

		case InputControls.pc:
			if (Input.GetKey (KeyCode.RightArrow)){
				m_toR++;
				pressR = true;
			}
			if (Input.GetKey (KeyCode.LeftArrow)){
				m_toL++;
				pressL = true;
			}
			if ((pressR && pressL) || Input.GetKey (KeyCode.Space))
				m_doJump = true;

			if (!pressR)
				m_toR=0;
			if (!pressL)
				m_toL=0;

			//TEST
			if (Input.GetKey (KeyCode.UpArrow))
				transform.Translate (transform.up * speedMovement * Time.deltaTime, Space.World);
			if (Input.GetKey (KeyCode.DownArrow))
				transform.Translate (transform.up * -speedMovement * Time.deltaTime, Space.World);
			break;

		case InputControls.swipe:
			int newTouchIndex = 0;
			bool end = false;
			for (int i = m_currentTouchIndex; i < Input.touchCount; ++i) { //from Current touch index to end
				if (Input.GetTouch (i).phase != TouchPhase.Ended && Input.GetTouch (i).phase != TouchPhase.Canceled) {
					if (Input.GetTouch (i).position.x < m_semiWidthScreen){
						m_toL++;
						pressL = true;
					}
					else{
						m_toR++;
						pressR = true;
					}

					if (Input.GetTouch (i).phase == TouchPhase.Moved && Input.GetTouch (i).deltaPosition.y > m_deltaToSwipe) {
						m_doJump = true;
					}

					newTouchIndex = Input.GetTouch (i).fingerId;
					end = true;
			
					break;
				}
			}

			if(!end){
				for (int i = 0; i < m_currentTouchIndex; ++i) { //from 0 to current touch index
					if (Input.GetTouch (i).phase != TouchPhase.Ended && Input.GetTouch (i).phase != TouchPhase.Canceled) {
						if (Input.GetTouch (i).position.x < m_semiWidthScreen){
							m_toL++;
							pressL = true;
						}
						else{
							m_toR++;
							pressR = true;
						}
						
						if (Input.GetTouch (i).phase == TouchPhase.Moved && Input.GetTouch (i).deltaPosition.y > m_deltaToSwipe) {
							m_doJump = true;
						}

						newTouchIndex = Input.GetTouch (i).fingerId;;

						break;
					}
				}
			}

			if (!pressR)
				m_toR=0;
			if (!pressL)
				m_toL=0;
			m_currentTouchIndex = newTouchIndex;
			break;
		case InputControls.doubleTap:
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase != TouchPhase.Ended && Input.GetTouch (i).phase != TouchPhase.Canceled) {
					if (Input.GetTouch (i).position.x < m_semiWidthScreen){
						m_toL++;
						pressL = true;
					}
					else{
						m_toR++;
						pressR = true;
					}
					if (Input.GetTouch (i).tapCount > 1)
						m_doJump = true;
				}
			}
			if (!pressR)
				m_toR=0;
			if (!pressL)
				m_toL=0;
			break;
		}

	}
	
	void DoMovement(){
		if (!m_isJumping && m_doJump) {  //Jump
			m_RG.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
			m_isJumping = true;
			m_hasLanded = false;
		} 
		
		if(m_hasLanded && !m_doJump)
			m_isJumping = false;
		
		if (m_toR>m_frameToMove && m_toL<=m_frameToMove) {  //Right
			if (m_status == state.stop)
				transform.Rotate (transform.up, -90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, 180, Space.World);
			m_status = state.right;
		} else if (m_toL>m_frameToMove && m_toR<=m_frameToMove) {  //Left
			if (m_status == state.stop)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.right)
				transform.Rotate (transform.up, 180, Space.World);
			m_status = state.left;
		} else if (m_toL<=m_frameToMove && m_toR<=m_frameToMove){  //Stop
			if (m_status == state.right)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, -90, Space.World);
			m_status = state.stop;
		}
		
		bool colWithWall = m_jumpCollisionScript.IsCollidingWithWall ();
		
		if (!colWithWall) {
			if (m_toR>m_frameToMove && m_status == state.right)
				transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
			
			if (m_toL>m_frameToMove && m_status == state.left)
				transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
		}
	}
	
	void DoTransition(){
		float dotForward = Vector3.Dot (transform.position, transform.forward);
		
		if (dotForward > m_absLimit && m_toR>m_frameToMove) { //right side
			transform.Rotate (transform.up, -90, Space.World);
			AjustPosition();
		}
		if (dotForward > m_absLimit && m_toL>m_frameToMove) { //left side
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
		return m_toR>m_frameToMove;
	}
	
	public bool isPlayerToLeft(){
		return m_toL>m_frameToMove;
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
