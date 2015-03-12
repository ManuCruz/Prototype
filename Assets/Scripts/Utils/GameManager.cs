using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private bool m_alive;
	private bool m_victory;
	
	private SceneFadeInOut m_sceneFadeInOut;

	private PlayerMovement m_playerMovement;

	void Start(){
		m_alive = true;
		m_victory = false;
		m_sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponentInChildren<SceneFadeInOut>();
		m_playerMovement = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMovement>();
	}

	void Update(){
		//QUIT
		if (Input.GetKeyDown (KeyCode.Escape)) 
			Application.Quit ();
	}

	public void PlayerDead(){
		if (m_alive) {
			m_sceneFadeInOut.End ("Game Over");
			m_playerMovement.enabled = false;
		}
		m_alive = false;
	}

	public void PlayerVictory(){
		if (!m_victory) {
			m_sceneFadeInOut.End ("Victory");
			m_playerMovement.enabled = false;
		}
		m_victory = true;
	}
}
