using UnityEngine;
using System.Collections;

public class GravityForce : MonoBehaviour {

	public float gravityForce = 9.8f;

	private Rigidbody m_RG;
	
	void Start () {
		m_RG = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		m_RG.AddForce (-transform.up * gravityForce, ForceMode.Acceleration);
	//	m_RG.AddRelativeForce (-Vector3.up * gravityForce, ForceMode.Acceleration);

	}
}
