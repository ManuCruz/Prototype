using UnityEngine;
using System.Collections;

public class MaterializationTrigger : MonoBehaviour {
	
	private IntermittentPlatform m_intermite;

	void Start () {
		m_intermite = transform.parent.GetComponent<IntermittentPlatform> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Tags.player) {
			m_intermite.changeUpdateStatus (false);
			Debug.Log("ColliderEnter");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == Tags.player) {
			m_intermite.changeUpdateStatus (false);
			Debug.Log("ColliderStay");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == Tags.player) {
			m_intermite.changeUpdateStatus (true);
			Debug.Log("ColliderExit");
			
		}
	}

}
