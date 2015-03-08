using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public Vector3 position;
	public Camera mainCamera;

	private GameObject world;
	private bool rotating;
	private Vector3 translate;
	private Vector3 dif;
	void Start(){
		world = GameObject.FindGameObjectWithTag (Tags.world);
		rotating = false;
		//siempre da valores positivos. arreglar
		translate.x = (position.x == 0) ? 0 : position.x / position.x;
		translate.y = (position.y == 0) ? 0 : position.y / position.y;
		translate.z = (position.z == 0) ? 0 : position.z / position.z;
		

		Debug.Log (translate);
	}

	void Update(){
		if (rotating) {
			mainCamera.transform.position += translate * Time.deltaTime;
			mainCamera.transform.LookAt(world.transform.position);
			//la condicion de parada no es de momento fiable.
			dif = position - mainCamera.transform.position;
			if((Mathf.Abs(dif.x) <= 0.01) && (Mathf.Abs(dif.y) <= 0.01) && (Mathf.Abs(dif.z) <= 0.01))
				rotating = false;
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == Tags.player) {
			rotating = true;
			//mainCamera.transform.position = position;
			//mainCamera.transform.LookAt(world.transform.position);
		}
	}
}
