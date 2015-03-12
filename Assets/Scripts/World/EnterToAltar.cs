using UnityEngine;
using System.Collections;

public class EnterToAltar : MonoBehaviour {

	private GameManager m_gameManager;
	
	void Start () {
		m_gameManager = GameObject.FindGameObjectWithTag(Tags.gameManager).GetComponent<GameManager>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == Tags.player) {
			if(transform.up == other.transform.up)
				m_gameManager.PlayerVictory();
		}
	}
}
