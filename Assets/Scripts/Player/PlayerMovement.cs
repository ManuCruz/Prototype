using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speedMovement = 3f;
	public float jumpForce = 5f;

	private Rigidbody m_RG;

	private bool m_canJump = true;
	private bool m_hasLanded = true;

	enum state{stop, right, left};
	private state m_status = state.stop;

	private float m_semiWidthScreen;
	private bool m_toR = false;
	private bool m_toL = false;

	private float m_absLimit;

	void Start () {
		m_RG = GetComponent<Rigidbody>();

		m_semiWidthScreen = Camera.main.pixelWidth/2;

		Vector3 inverseTransform = transform.InverseTransformVector (transform.position);
		m_absLimit = Mathf.Max (Mathf.Abs (inverseTransform.x), Mathf.Abs (inverseTransform.y), Mathf.Abs (inverseTransform.z));
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
			m_RG.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
		}
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate (Vector3.up * speedMovement * Time.deltaTime);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate (Vector3.up * -speedMovement * Time.deltaTime);
	}

	void DoMovement(){
		if (m_canJump && m_toR && m_toL) {  //Jump
			m_RG.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
			m_canJump = false;
			m_hasLanded = false;
		} 

		if(m_hasLanded && ((m_toR && !m_toL) || (!m_toR && m_toL)))
			m_canJump = true;

		if (m_toR && !m_toL) {  //Right
			if (m_status == state.stop)
				transform.Rotate (Vector3.up, -90);
			if (m_status == state.left)
				transform.Rotate (Vector3.up, 180);
			m_status = state.right;
		} else if (m_toL && !m_toR) {  //Left
			if (m_status == state.stop)
				transform.Rotate (Vector3.up, 90);
			if (m_status == state.right)
				transform.Rotate (Vector3.up, 180);
			m_status = state.left;
		} else if (!m_toL && !m_toR){  //Stop
			if (m_status == state.right)
				transform.Rotate (Vector3.up, 90);
			if (m_status == state.left)
				transform.Rotate (Vector3.up, -90);
			m_status = state.stop;
		}

		if (m_toR && m_status == state.right)
			transform.Translate (Vector3.forward * speedMovement * Time.deltaTime);

		if (m_toL && m_status == state.left)
			transform.Translate (Vector3.forward * speedMovement * Time.deltaTime);
	}
		
	void DoTransition(){
		float dotForward = Mathf.Abs (Vector3.Dot (transform.InverseTransformVector (transform.position), Vector3.forward));

		if (dotForward > m_absLimit && m_toR) { //right side
			transform.Rotate (Vector3.up, -90);
			AjustPosition();
		}
		else if (dotForward > m_absLimit && m_toL) { //left side
			transform.Rotate (Vector3.up, 90);
			AjustPosition();
		}

		float dotUp = Vector3.Dot (transform.InverseTransformVector (transform.position), Vector3.up);

		if (dotUp > m_absLimit || dotUp < -m_absLimit) { //up and down sides
			bool toUp = dotUp > m_absLimit;

			Vector3 vel = m_RG.velocity;  //get the velocity
	//		Vector3 vel = transform.InverseTransformVector (m_RG.velocity);
			m_RG.velocity = new Vector3(0,0,0);

			Debug.Log ("vel" + vel);

			switch (m_status){
			case state.stop: 
				transform.Rotate (Vector3.right, toUp? -90 : 90);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.right) * vel;
				break;
			case state.right:
				transform.Rotate (Vector3.forward, toUp? 90 : -90);
				vel = Quaternion.AngleAxis(toUp? 90 : -90, transform.forward) * vel;
				break;
			case state.left:
				transform.Rotate (Vector3.forward, toUp? -90 : 90);
				vel = Quaternion.AngleAxis(toUp? -90 : 90, transform.forward) * vel;
				break;
			}

			AjustPosition();
	
			Debug.Log ("rotated vel " + vel);
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
		Vector3 inverseTransform = transform.InverseTransformVector (transform.position);

		if (inverseTransform.x > m_absLimit)
			inverseTransform = new Vector3(m_absLimit, inverseTransform.y, inverseTransform.z);
		else if (inverseTransform.x < -m_absLimit)
			inverseTransform = new Vector3(-m_absLimit, inverseTransform.y, inverseTransform.z);
		else if (inverseTransform.y > m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, m_absLimit, inverseTransform.z);
		else if (inverseTransform.y < -m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, -m_absLimit, inverseTransform.z);
		else if (inverseTransform.z > m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, inverseTransform.y, m_absLimit);
		else if (inverseTransform.z < -m_absLimit)
			inverseTransform = new Vector3(inverseTransform.x, inverseTransform.y, -m_absLimit);

		transform.TransformVector (inverseTransform);
	}

	public void ResetJump(){
		m_hasLanded = true;
	}
	
	public bool isPlayerToRight(){
		return m_toR;
	}

	public bool isPlayerToLeft(){
		return m_toL;
	}
}
