using UnityEngine;
using System.Collections;

public class IntermittentPlatform : MonoBehaviour {
	
	public float timeActived = 1f;
	public float timeDesactived = 2f;
	public bool actived = true;

	private bool m_update = true;
	private float m_elapsed = 0f;

	void Start () {
		m_elapsed = 0f;
		if (!actived)
			updateComponents ();
	}

	void Update () {
		if (m_update) {
			m_elapsed += Time.deltaTime;
			checkStatus ();
		}
	}

	void checkStatus(){
		if (actived) {
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
		actived = !actived;
		m_elapsed = 0;
	}

	void updateComponents(){
		renderer.enabled = actived;
		collider.enabled = actived;
		rigidbody.isKinematic = actived;

	}

	public void changeUpdateStatus(bool status){
		m_update = status;
	}

	public bool getActived(){
		return actived;
	}

}
