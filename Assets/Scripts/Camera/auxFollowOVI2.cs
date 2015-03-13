using UnityEngine;
using System.Collections;

public class auxFollowOVI2 : MonoBehaviour {
	
	public float distance = 10f;
	public float paddingLimit = 1f;

	private Transform m_playerTransform;
	private Transform m_cameraTransform;

	private float m_absLimit;

	void Start () {
		m_playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;

		m_cameraTransform = Camera.main.transform;

		Vector3 inverseTransform = m_playerTransform.InverseTransformVector (m_playerTransform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;
	}
	
	void Update () {
		Vector3 pos = m_playerTransform.position;

		//probar con "=" y con "+="
		if (pos.x > m_absLimit)
			pos.x = distance;
		else if (pos.x < -m_absLimit)
			pos.x = -distance;

		if (pos.y > m_absLimit)
			pos.y = distance;
		else if (pos.y < -m_absLimit)
			pos.y = -distance;

		if (pos.z > m_absLimit)
			pos.z = distance;
		else if (pos.z < -m_absLimit)
			pos.z = -distance;

		m_cameraTransform.position = pos;










		//m_cameraTransform.up = m_playerTransform.up; // no works

		//m_cameraTransform.rotation = m_playerTransform.rotation; // no works




		m_cameraTransform.LookAt (m_playerTransform.position);


		Debug.Log ("m_cameraTransform.up " + m_cameraTransform.up);
		Debug.Log ("m_playerTransform.up " + m_playerTransform.up);

		Debug.Log ("Vector3.Dot " + Vector3.Dot (m_cameraTransform.up, m_playerTransform.up));

		while (Vector3.Dot (m_cameraTransform.up, m_playerTransform.up) < 0.05) {
			m_cameraTransform.Rotate (Vector3.forward * -90);
			Debug.Log ("dentro");
		}




	}
}