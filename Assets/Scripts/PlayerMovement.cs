using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_isJumping = false;

	enum state{stop, right, left};
	private state m_status = state.stop;
	
	void Start () {
		m_RG = GetComponent<Rigidbody>();
	}

	void Update () {
		if (!m_isJumping && ((Input.GetKey (KeyCode.RightArrow) && Input.GetKey (KeyCode.LeftArrow)) /*|| (mobile)*/)) {
			m_RG.velocity = (transform.up * jumpForce);
			m_isJumping = true;
		}


		if(Input.GetKey (KeyCode.DownArrow))
			m_isJumping = false;
	}

	void FixedUpdate(){
		if ((Input.GetKey (KeyCode.RightArrow) /*|| (mobile)*/) && m_status != state.left) {
			if ( m_status == state.stop)
				transform.Rotate (transform.up, -90);

			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);

			m_status = state.right;
		}
		else if ((Input.GetKey (KeyCode.LeftArrow) /*|| (mobile)*/) && m_status != state.right) {
			if ( m_status == state.stop)
				transform.Rotate (transform.up, 90);

			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);

			m_status = state.left;
		}
		else {
			if ( m_status == state.right)
				transform.Rotate (transform.up, 90);
			if ( m_status == state.left)
				transform.Rotate (transform.up, -90);

			m_status = state.stop;
		}

	}
}
