using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private bool m_alive;
	private bool m_victory;

	private float m_score = 0f;

	private SceneFadeInOut m_sceneFadeInOut;

	private PlayerMovement m_playerMovement;

	private ChangeScene m_changeScene;
	
	void Start (){
		m_changeScene = GetComponent<ChangeScene>();
		m_alive = true;
		m_victory = false;

		m_sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponentInChildren<SceneFadeInOut>();
		m_playerMovement = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMovement>();

		m_score = 0f;
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
			m_changeScene.NextScene();
			m_sceneFadeInOut.End ("Victory");
			m_playerMovement.enabled = false;
		}
		m_victory = true;
	}

	public void IncreaseScore(){
		m_score++;
	}
	
	public float GetScore(){
		return m_score;
	}
}
