using UnityEngine;
using System.Collections;

public class auxFollowOVI2 : MonoBehaviour {

	public float distance = 15f;

	private GameObject m_player;
	private Transform m_playerTransform;
	private PlayerMovement m_playerMovement;

	private float m_preDotForward;
	private float m_dotForward;

	public float paddingLimit = 1f;
	private float m_absLimit;
	
	private float m_angleVision = 45;

	void Start () {
		m_player = GameObject.FindGameObjectWithTag(Tags.player);
		m_playerTransform = m_player.transform;
		m_playerMovement = m_playerTransform.gameObject.GetComponent<PlayerMovement> ();

		m_dotForward = Vector3.Dot (m_playerTransform.position, m_playerTransform.forward);
		m_preDotForward = m_dotForward;

		Vector3 inverseTransform = transform.InverseTransformVector (transform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z)) - paddingLimit;
	}

	void Update () {
		Vector3 pos = m_player.transform.position.normalized;
		Camera.main.transform.position = pos * distance;
		Camera.main.transform.LookAt (pos);


	//	Camera.main.transform.position = new Vector3 (m_playerTransform.position.x, m_playerTransform.position.y, Camera.main.transform.position.z);
	//	Camera.main.transform.LookAt (new Vector3 (m_playerTransform.position.x, m_playerTransform.position.y, 0));


		m_dotForward = Vector3.Dot (m_playerTransform.position, m_playerTransform.forward);


		if (m_dotForward > m_absLimit && m_playerMovement.isPlayerToRight ()) { //right side
			transform.Rotate (transform.up, -m_angleVision, Space.World);
			Debug.Log ("dentro der");
		}
		else if (m_dotForward > m_absLimit && m_playerMovement.isPlayerToLeft ()) { //left side
			transform.Rotate (transform.up, m_angleVision, Space.World);
			Debug.Log ("dentro izq");
		}




		m_preDotForward = m_dotForward;

	}
}