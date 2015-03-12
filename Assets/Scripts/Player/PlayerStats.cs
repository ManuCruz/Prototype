using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	
	private bool m_alive;
	private bool m_victory;
	
	private SceneFadeInOut m_sceneFadeInOut;

	void Start(){
		m_alive = true;
		m_victory = false;
		m_sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponentInChildren<SceneFadeInOut>();
	}

	public void PlayerDead(){
		if(m_alive)
			m_sceneFadeInOut.End("Game Over");

		m_alive = false;
	}

	public void PlayerVictory(){
		if(!m_victory)
		m_sceneFadeInOut.End("Victory");

		m_victory = true;
	}
}
