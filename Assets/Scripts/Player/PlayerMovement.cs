using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_isJumping = false;

	enum state{stop, right, left};
	private state m_status = state.stop;

	private float m_semiWidthScreen;
	private bool m_toR = false;
	private bool m_toL = false;

	private float m_absLimit;

	void Start () {
		m_RG = GetComponent<Rigidbody>();

		m_semiWidthScreen = Camera.main.pixelWidth/2;

		m_absLimit = Mathf.Max (Mathf.Abs (transform.position.x), Mathf.Abs (transform.position.y), Mathf.Abs (transform.position.z));
	}

	void Update () {
		GetInput ();
		DoMovement ();
		DoTransition ();

		//QUIT
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit();
	}

	/* Escribo hablando de derecha, para la izquierda es lo mismo.
	 * Si se pulsa derecha, avanzar a la derecha.
	 * si pulsando derecha, se pulsa izquieda (se mantenga o no), saltar hacia la derecha.
	 * Si se pulsan los dos a la vez, se salta hacia arriba.
	 * si se pulsan los dos y no se levantan, solo se salta una vez.
	 * Si pulsando derecha se desliza el dedo, el personaje deja de avanzar.
	 */
	
	void GetInput(){
		m_toR = false;
		m_toL = false;

		//movil
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Stationary){
				if (Input.GetTouch(i).position.x < m_semiWidthScreen)
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

			m_RG.AddForce(vel, ForceMode.VelocityChange);  //restore the velocity

			//problemas conocidos:
			//1.- en una caida sin fin, llega un momento donde se resetea la fuerza.
			//		posible causa: se llama a la funcion DoTransition() en dos frames consecutivos  y no se ha terminado de aplicar la fuerza calculada 
			//		en el primero de ellos. Esto produce que en el segundo, m_RG.velocity sea cero.
			//2.- a veces el personaje se sale de los "limites"
			//3.- en los saltos se atasca contra los obstaculos
			//4.- saltos seguidos en móvil
		}
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
