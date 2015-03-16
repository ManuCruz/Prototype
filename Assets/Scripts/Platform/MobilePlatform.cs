using UnityEngine;
using System.Collections;

public class MobilePlatform : MonoBehaviour {

	//siempre se mueve en el right
	public float rightDistance = 1;
	public float leftDistance = 1;
	public float speed = 1;

	private float m_maxPosition = 0;
	private float m_minPosition = 0;
	private float m_direction = 1;

	void Start () {
		//posiciones iniciales y finales
		m_minPosition = Vector3.Dot(transform.position, transform.right) - leftDistance;
		m_maxPosition = Vector3.Dot(transform.position, transform.right) + rightDistance;	 
	}
	

	void Update () {
		movePlatform();
	}

	void movePlatform(){
		float actualPosition = Vector3.Dot(transform.position, transform.right);
		
		if (actualPosition > m_maxPosition)
			m_direction = -1;
		else if (actualPosition < m_minPosition)
			m_direction = 1;
		
		transform.position += transform.right * speed * Time.deltaTime * m_direction;
	}

	public float getActualDirection(){
		return m_direction;
	}
}
