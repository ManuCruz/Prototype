using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour {

	public float distance = 15f;

	public GameObject prota;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = prota.transform.position.normalized;
		Camera.main.transform.position = pos * distance;
		Camera.main.transform.LookAt (pos);
	}
}
