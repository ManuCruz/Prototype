using UnityEngine;
using System.Collections;

public class IntermittentPlatform : MonoBehaviour {
	
	public float timeActived = 1f;
	public float timeDesactived = 2f;
	
	private bool m_update = true;
	private float m_elapsed = 0f;
	private bool m_actived;

	void Start () {
		m_elapsed = 0f;
		m_actived = renderer.enabled;
	}

	void Update () {
		if (m_update) {
			m_elapsed += Time.deltaTime;
			checkStatus ();
		}
	}

	void checkStatus(){
		if (m_actived) {
			if (m_elapsed >= timeActived) {
				resetStatus();
				updateComponents();
			}
		} else {
			if (m_elapsed >= timeDesactived) {
				resetStatus();
				updateComponents();
			}
		}
	}

	void resetStatus(){
		m_actived = !m_actived;
		m_elapsed = 0;
	}

	void updateComponents(){
		renderer.enabled = m_actived;
		collider.enabled = m_actived;
		rigidbody.isKinematic = m_actived;

	}

	public void changeUpdateStatus(bool status){
		m_update = status;
	}

	public bool getActived(){
		return m_actived;
	}

}
