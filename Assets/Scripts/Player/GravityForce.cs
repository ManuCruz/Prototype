﻿using UnityEngine;
using System.Collections;

public class GravityForce : MonoBehaviour {

	public float gravityForce = 9.8f;

	private Rigidbody m_RG;
	
	void Start () {
		m_RG = GetComponent<Rigidbody>();
	}

	void Update () {
		m_RG.AddRelativeForce (-transform.up * gravityForce);
	}
}