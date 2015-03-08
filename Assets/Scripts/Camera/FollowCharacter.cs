using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour {

	public bool enable = true;
	public float distance = 15f;
	public float speed = 1.0f;
	
	private GameObject m_player;
	private GameObject m_world;
	void Start () {
		m_player = GameObject.FindGameObjectWithTag(Tags.player);
		m_world = GameObject.FindGameObjectWithTag(Tags.world);
	}
	
	void Update () {
		if (enable) {
			Vector3 pos = m_player.transform.position.normalized;
			Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, pos * distance, speed * Time.deltaTime);
			Camera.main.transform.LookAt(m_world.transform.position);
		}
	}
}
