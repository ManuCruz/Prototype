using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	private float m_velocity;
	private MovilePlatform m_movile;
	void Start(){
		m_movile = transform.parent.gameObject.GetComponent<MovilePlatform>();
		m_velocity = m_movile.speed;
	}
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == Tags.playerFeet) {
			other.transform.parent.transform.Translate(transform.right * m_velocity * Time.deltaTime * m_movile.getActualDirection(), Space.World);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == Tags.playerFeet) {
			other.transform.parent.transform.Translate(transform.right * m_velocity * Time.deltaTime * m_movile.getActualDirection(), Space.World);
		}

	}
}
