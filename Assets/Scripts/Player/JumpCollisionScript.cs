using UnityEngine;
using System.Collections;

public class JumpCollisionScript : MonoBehaviour {

	private bool m_collisionWithWall;

	void Start () {
		m_collisionWithWall = false;
	}
	
	void OnTriggerEnter(Collider other) {
		m_collisionWithWall = true;
	}

	void OnTriggerStay(Collider other) {
		m_collisionWithWall = true;
	}

	void OnTriggerExit(Collider other) {
		m_collisionWithWall = false;
	}

	public bool IsCollidingWithWall(){
		return m_collisionWithWall;
	}

}