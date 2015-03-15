using UnityEngine;
using System.Collections;

public class FinalCameraScript : MonoBehaviour {
	
	public float cameraDistance = 10f;
	public float paddingLimit = 1f;
	public float cameraSpeed = 3.0f;

	private Transform m_playerTransform;
	private Transform m_cameraTransform;

	private float m_absLimit;

	private float m_sqrt2, m_sqrt3;

	private Vector3 m_finalPosition;

	private int m_count = 1;
	private bool m_rotation = false;

	void Start () {
		m_playerTransform = GameObject.FindGameObjectWithTag(Tags.player).transform;

		m_cameraTransform = Camera.main.transform;

		Vector3 inverseTransform = m_playerTransform.InverseTransformVector (m_playerTransform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;

		m_sqrt2 = Mathf.Sqrt (2);
		m_sqrt3 = Mathf.Sqrt (3);
	}
	
	void Update () {
		setPosition ();
		setOrientation ();
	}

	void setPosition(){
		Vector3 pos = m_playerTransform.position;

		if (pos.x > m_absLimit)
			pos.x = cameraDistance;
		else if (pos.x < -m_absLimit)
			pos.x = -cameraDistance;
		
		if (pos.y > m_absLimit)
			pos.y = cameraDistance;
		else if (pos.y < -m_absLimit)
			pos.y = -cameraDistance;
		
		if (pos.z > m_absLimit)
			pos.z = cameraDistance;
		else if (pos.z < -m_absLimit)
			pos.z = -cameraDistance;

		int count = 0;
		if (m_playerTransform.position.x != pos.x)
			count++;
		if (m_playerTransform.position.y != pos.y)
			count++;
		if (m_playerTransform.position.z != pos.z)
			count++;

		if (count != m_count) {
			m_count = count;
			m_rotation = true;
		}


		Vector3 absPos = new Vector3 (Mathf.Abs (pos.x), Mathf.Abs (pos.y), Mathf.Abs (pos.z)); 
		
		if (absPos.x == cameraDistance && absPos.y == cameraDistance && absPos.z == cameraDistance)
			pos = pos/m_sqrt3;
		else if (absPos.x == cameraDistance && absPos.y == cameraDistance)
			pos = pos/m_sqrt2;
		else if (absPos.x == cameraDistance && absPos.z == cameraDistance)
			pos = pos/m_sqrt2;
		else if (absPos.y == cameraDistance && absPos.z == cameraDistance)
			pos = pos/m_sqrt2;

		m_finalPosition = pos;

		m_cameraTransform.position = Vector3.Lerp (m_cameraTransform.position, m_finalPosition, cameraSpeed * Time.deltaTime);
	}

	void setOrientation(){
		m_cameraTransform.LookAt (m_playerTransform.position);

//		while (Vector3.Dot (m_cameraTransform.up, m_playerTransform.up) < 0.05)
//			m_cameraTransform.Rotate (Vector3.forward * -90);


		for (int i = 0; i < 400; i++)
			if (Vector3.Dot (m_cameraTransform.up, m_playerTransform.up) < 0.05)
				m_cameraTransform.Rotate (Vector3.forward * -90);
			else
				break;
	}
}