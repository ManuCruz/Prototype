using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_isJumping = false;

	enum state{stop, right, left};
	private state m_status = state.stop;

	private float m_semiWidth;
	private bool m_toR = false;
	private bool m_toL = false;

	private float m_absLimit;

	void Start () {
		m_RG = GetComponent<Rigidbody>();

		m_semiWidth = Camera.main.pixelWidth/2;

		float worldSize = GameObject.FindGameObjectWithTag(Tags.world).GetComponent<SizeScript>().worldSize;
		m_absLimit = (worldSize + transform.localScale.z)/2;
	}

	void Update () {
		GetInput ();
		DoMovement ();
		DoTransition ();

		//QUIT
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();
	}

	void GetInput(){
		m_toR = false;
		m_toL = false;

		//movil
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary){
				if (Input.GetTouch(i).position.x < m_semiWidth)
					m_toL = true;
				else
					m_toR = true;
			}
		}

		//PC
		if (Input.GetKey (KeyCode.RightArrow))
			m_toR = true;
		if (Input.GetKey (KeyCode.LeftArrow))
			m_toL = true;


		//TEST
		if (Input.GetKeyDown (KeyCode.Space)){
			m_RG.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
//			m_RG.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
		}
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate (transform.up * speedMovement * Time.deltaTime, Space.World);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate (transform.up * -speedMovement * Time.deltaTime, Space.World);
	}

	void DoMovement(){
		if (!m_isJumping && m_toR && m_toL) {  //Jump
			m_RG.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
			m_isJumping = true;
		} else if (m_toR && !m_toL) {  //Right
			if (m_status == state.stop)
				transform.Rotate (transform.up, -90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, 180, Space.World);
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
			
			m_status = state.right;
		} else if (m_toL && !m_toR) {  //Left
			if (m_status == state.stop)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.right)
				transform.Rotate (transform.up, 180, Space.World);
			transform.Translate (transform.forward * speedMovement * Time.deltaTime, Space.World);
			
			m_status = state.left;
		} else if (!m_toL && !m_toR){  //Stop
			if (m_status == state.right)
				transform.Rotate (transform.up, 90, Space.World);
			if (m_status == state.left)
				transform.Rotate (transform.up, -90, Space.World);
			
			m_status = state.stop;
		}
	}
		
	void DoTransition(){
		float dotForward = Vector3.Dot (transform.position, transform.forward);
		
		if (dotForward > m_absLimit && m_toR) { //right side
			transform.Rotate (transform.up, -90, Space.World);
			AjustPosition();
		}
		else if (dotForward > m_absLimit && m_toL) { //left side
			transform.Rotate (transform.up, 90, Space.World);
			AjustPosition();
		}

		float dotUp = Vector3.Dot (transform.position, transform.up);

		if (dotUp > m_absLimit || dotUp < -m_absLimit) { //up and down sides
			bool toUp = dotUp > m_absLimit;

			Vector3 vel = m_RG.velocity;  //get the velocity
			m_RG.velocity = new Vector3(0,0,0);

			Debug.Log ("velocidad al empezar el cambio" + vel);
			Debug.Log ("velocidad tras el resetep" + m_RG.velocity);

			switch (m_status){
			case state.stop: 
				transform.Rotate (transform.right, toUp? -90 : 90, Space.World);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.right) * vel;
				break;
			case state.right:
				transform.Rotate (transform.forward, toUp? 90 : -90, Space.World);
				vel = Quaternion.AngleAxis(toUp? 90 : -90, transform.forward) * vel;
				break;
			case state.left:
				transform.Rotate (transform.forward, toUp? -90 : 90, Space.World);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.forward) * vel;
				break;
			}

			AjustPosition();

		//	vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.right) * vel;

			m_RG.AddForce(vel, ForceMode.VelocityChange);  //restore the velocity

			//problemas conocidos:
			//1.- en una caida sin fin, llega un momento donde se resetea la fuerza.
			//		posible causa: se llama a la funcion DoTransition() en dos frames consecutivos  y no se ha terminado de aplicar la fuerza calculada 
			//		en el primero de ellos. Esto produce que en el segundo, m_RG.velocity sea cero.

		
		}

		Debug.Log ("velocidad del objeto " + m_RG.velocity);
	}

	void AjustPosition(){
		if (transform.position.x > m_absLimit)
			transform.position = new Vector3(m_absLimit, transform.position.y, transform.position.z);
		else if (transform.position.x < -m_absLimit)
			transform.position = new Vector3(-m_absLimit, transform.position.y, transform.position.z);
		else if (transform.position.y > m_absLimit)
			transform.position = new Vector3(transform.position.x, m_absLimit, transform.position.z);
		else if (transform.position.y < -m_absLimit)
			transform.position = new Vector3(transform.position.x, -m_absLimit, transform.position.z);
		else if (transform.position.z > m_absLimit)
			transform.position = new Vector3(transform.position.x, transform.position.y, m_absLimit);
		else if (transform.position.z < -m_absLimit)
			transform.position = new Vector3(transform.position.x, transform.position.y, -m_absLimit);
	}

	public void ResetJump(){
		m_isJumping = false;
	}
}
