using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class RenderScore : MonoBehaviour {
	
	private Text m_text;
	
	private GameManager m_gameManager;
	
	void Start () {
		m_text = GetComponentInParent<Text> ();

		m_gameManager = GameObject.FindGameObjectWithTag (Tags.gameManager).GetComponent<GameManager>();
		m_text.text = m_gameManager.GetScore().ToString ();
	}

	void Update () {
		m_text.text = m_gameManager.GetScore().ToString ();
	}
}
