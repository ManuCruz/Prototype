using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public Vector3 position;
	public Camera mainCamera;

	private GameObject world;
	void Start(){
		world = GameObject.FindGameObjectWithTag (Tags.world);
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == Tags.player) {
			mainCamera.transform.position = position;
			mainCamera.transform.LookAt(world.transform.position);
		}
	}
}
