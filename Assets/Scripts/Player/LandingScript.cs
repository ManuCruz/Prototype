using UnityEngine;
using System.Collections;

public class LandingScript : MonoBehaviour {

	private PlayerMovement m_playerMovement;
	
	void Start () {
		m_playerMovement = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMovement>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == Tags.floor || other.tag == Tags.altar)
			m_playerMovement.ResetJump ();
	}
}
