using UnityEngine;
using System.Collections;

public class LandingScript : MonoBehaviour {

	private PlayerMovement playerMovement;
	
	void Start () {
		playerMovement = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMovement>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == Tags.world)
			playerMovement.ResetJump ();
	}
}
