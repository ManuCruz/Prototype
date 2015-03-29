using UnityEngine;
using System.Collections;

public class EnterToAltar : MonoBehaviour {

	public bool requireKey = false;

	private GameManager m_gameManager;
	private PlayerInventory m_playerInventory;

	void Start () {
		m_gameManager = GameObject.FindGameObjectWithTag(Tags.gameManager).GetComponent<GameManager>();
		m_playerInventory = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerInventory>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == Tags.player) {
			if (transform.up == other.transform.up) {
				if (!requireKey) {
					m_gameManager.PlayerVictory ();
				} else {
					if (m_playerInventory.hasKey) {
						m_gameManager.PlayerVictory ();
					}
				}
			}
		}
	}
}
