using UnityEngine;
using System.Collections;

public class CameraOrientation : MonoBehaviour {

	public Vector3 endPosition;
	public Camera mainCamera;
	public float speed = 3.0f;
	
	private GameObject m_world;
	private FollowCharacter m_follow;
	private bool m_centerCamera;

	void Start () {
		m_follow = mainCamera.GetComponent<FollowCharacter>();
		m_world = GameObject.FindGameObjectWithTag(Tags.world);
		m_centerCamera = false;
	}

	void Update () {
		if (m_centerCamera) {
			if (mainCamera.transform.position == endPosition) {
				mainCamera.transform.position = endPosition;
			} else {
				mainCamera.transform.position = Vector3.Slerp (mainCamera.transform.position, endPosition, speed * Time.deltaTime);
				mainCamera.transform.LookAt (m_world.transform.position);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == Tags.player) {
			m_follow.enable = false;
			m_centerCamera = true;
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.tag == Tags.player) {
			m_follow.enable = true;
			m_centerCamera = false;
		}
	}
}
