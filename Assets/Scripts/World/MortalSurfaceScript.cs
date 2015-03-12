using UnityEngine;
using System.Collections;

public class MortalSurfaceScript : MonoBehaviour {

	private PlayerStats m_playerStats;
	
	void Start () {
		m_playerStats = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStats>();
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == Tags.player) {
			m_playerStats.PlayerDead ();
		}
	}
}
