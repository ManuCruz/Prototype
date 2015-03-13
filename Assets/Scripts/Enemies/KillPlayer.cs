using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

	private GameManager m_gameManager;
	
	void Start () {
		m_gameManager = GameObject.FindGameObjectWithTag(Tags.gameManager).GetComponent<GameManager>();
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.player) {
			m_gameManager.PlayerDead ();
		}
	}
}
