using UnityEngine;
using System.Collections;

public class RotateWorld : MonoBehaviour {


	public float paddingLimit = 0.3f;
	public float vision = 45;
	private Transform m_player;
	private PlayerMovement movement;
	private Quaternion m_orientation;
	private float anglesToRotate;
	void Start () {
		m_player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		movement = m_player.gameObject.GetComponent<PlayerMovement> ();
		m_orientation = Quaternion.Euler (0, 0, 0);
		anglesToRotate = vision;
	}

	void Update () {
		actualAngles ();
		LookWorld();
		transform.localRotation = Quaternion.Lerp(transform.localRotation, m_orientation , Time.deltaTime);
	}


	void actualAngles() {
		float absLimit = Mathf.Max (Mathf.Abs (m_player.localPosition.x), Mathf.Abs (m_player.localPosition.y), Mathf.Abs (m_player.localPosition.z));
		if (absLimit == Mathf.Abs (transform.localPosition.z))
			anglesToRotate = (transform.localPosition.z < 0) ? vision : vision + 180;

		if (absLimit == Mathf.Abs (m_player.localPosition.x))
			anglesToRotate = (transform.localPosition.x > 0) ? vision + 90 : vision - 90;
	}

	void LookWorld(){
		//VERTICAL
		/*float dotUp = Vector3.Dot (m_player.localPosition, transform.up);
		
		if (dotUp > paddingLimit || dotUp < -paddingLimit) { //up and down sides
			bool toUp = dotUp > paddingLimit;
			m_orientation = Quaternion.Euler (toUp ? -vision : vision, 0, 0);
		} else if (dotUp < paddingLimit && dotUp > -paddingLimit) {
			m_orientation = Quaternion.Euler (0, 0, 0);
		}*/

		//HORIZONTAL
		float dotForward = Vector3.Dot(m_player.localPosition, m_player.forward);
		
		if (dotForward > paddingLimit && movement.isPlayerToRight()) {
			m_orientation = Quaternion.Euler (0, anglesToRotate, 0);
		} else if (dotForward < paddingLimit && movement.isPlayerToRight()) {
			m_orientation = Quaternion.Euler (0, 0, 0);
		}

		if (dotForward > paddingLimit && movement.isPlayerToLeft()) {
			m_orientation = Quaternion.Euler (0, -anglesToRotate, 0);
		} else if (dotForward < paddingLimit && movement.isPlayerToLeft()) {
			m_orientation = Quaternion.Euler (0, 0, 0);
		}


	}
}
