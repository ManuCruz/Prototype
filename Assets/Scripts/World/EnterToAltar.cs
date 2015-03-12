using UnityEngine;
using System.Collections;

public class EnterToAltar : MonoBehaviour {

	private PlayerStats m_playerStats;
	
	void Start () {
		m_playerStats = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStats>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == Tags.player) {
			Debug.Log ("colision con puerta");
			if(transform.up == other.transform.up)
				m_playerStats.PlayerVictory();
		}
	}
}
