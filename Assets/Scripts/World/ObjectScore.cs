using UnityEngine;
using System.Collections;

public class ObjectScore : MonoBehaviour {

	private GameObject m_player;
	private GameManager m_gameManager;
	
	void Start(){
		m_player = GameObject.FindGameObjectWithTag (Tags.player);
		m_gameManager = GameObject.FindGameObjectWithTag (Tags.gameManager).GetComponent<GameManager>();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject == m_player) {
			m_gameManager.IncreaseScore();
			Destroy(gameObject);
		}
	}
}
