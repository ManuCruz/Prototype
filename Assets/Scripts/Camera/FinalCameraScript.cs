﻿using UnityEngine;
using System.Collections;

public class FinalCameraScript : MonoBehaviour {
	
	public float cameraDistance = 10f;
	public float paddingLimit = 1f;
	public float cameraSpeed = 3.0f;

	private Transform m_playerTransform;
	private Transform m_cameraTransform;

	private float m_absLimit;

	private float m_sqrt2, m_sqrt3;

	private Vector3 m_finalPosition;
	private float m_elapsed = 4.1f;
	private bool m_lerp = false;

	void Start () {
		m_playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;

		m_cameraTransform = Camera.main.transform;

		Vector3 inverseTransform = m_playerTransform.InverseTransformVector (m_playerTransform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;

		m_sqrt2 = Mathf.Sqrt (2);
		m_sqrt3 = Mathf.Sqrt (3);  //tengo dudas de si esto es asi, o hay que utilizar la raiz cubica
	}
	
	void Update () {
		setPosition ();
		setOrientation ();
	}

	void setPosition(){
		Vector3 pos = m_playerTransform.position;
		if (pos.x > m_absLimit)
			pos.x = cameraDistance;
		else if (pos.x < -m_absLimit)
			pos.x = -cameraDistance;
		
		if (pos.y > m_absLimit)
			pos.y = cameraDistance;
		else if (pos.y < -m_absLimit)
			pos.y = -cameraDistance;
		
		if (pos.z > m_absLimit)
			pos.z = cameraDistance;
		else if (pos.z < -m_absLimit)
			pos.z = -cameraDistance;
		
		Vector3 absPos = new Vector3 (Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)); 

		if (absPos.x == cameraDistance && absPos.y == cameraDistance && absPos.z == cameraDistance) {
			pos = pos / m_sqrt3;
			m_lerp = true;
		} else if (absPos.x == cameraDistance && absPos.y == cameraDistance) {
			pos.x = pos.x / m_sqrt2;
			pos.y = pos.y / m_sqrt2;
			m_lerp = true;
		} else if (absPos.x == cameraDistance && absPos.z == cameraDistance) {
			pos.x = pos.x / m_sqrt2;
			pos.z = pos.z / m_sqrt2;
			m_lerp = true;
		} else if (absPos.y == cameraDistance && absPos.z == cameraDistance) {
			pos.y = pos.y / m_sqrt2;
			pos.z = pos.z / m_sqrt2;
			m_lerp = true;
		} else {
			m_lerp = false;
		}
		
		m_finalPosition = pos;
		
		if (m_lerp){
			m_elapsed = 0;
			m_cameraTransform.position = Vector3.Lerp (m_cameraTransform.position, m_finalPosition, cameraSpeed * Time.deltaTime);
		}else {
			if (m_elapsed >= 4){
				m_cameraTransform.position = m_finalPosition;
			}else{
				m_elapsed += Time.deltaTime;
				m_cameraTransform.position = Vector3.Lerp (m_cameraTransform.position, m_finalPosition, cameraSpeed * Time.deltaTime);
			}
		}
	}

	void setOrientation(){
		m_cameraTransform.forward = m_playerTransform.position - m_cameraTransform.position;
		m_cameraTransform.rotation = Quaternion.LookRotation(m_cameraTransform.forward, m_playerTransform.up);




	//	m_cameraTransform.LookAt (m_playerTransform.position);

//		while (Vector3.Dot (m_cameraTransform.up, m_playerTransform.up) < 0.5) {
//			m_cameraTransform.Rotate (Vector3.forward * -90);
//		}

/*		for (int i = 0; i < 4; i++) {
			if (Vector3.Dot (m_cameraTransform.up, m_playerTransform.up) < 0.66)
				m_cameraTransform.Rotate (Vector3.forward * -90);
			else
				break;
		}*/
	}
}