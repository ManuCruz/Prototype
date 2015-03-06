using UnityEngine;
using System.Collections;

public class FollowCharacter : MonoBehaviour {

	public float width = 5f;
	public float height = 5f;
	public float padding = 4f;
	public string direction = "X";

	private float midWidth;
	private float midHeight;
	private float paddingWidth;
	private float paddingHeight;

	private float anglesToRotate;
	private GameObject player;

	private GameObject world;
	
	
	// Use this for initialization
	void Start () {
		midWidth = width / 2f;
		midHeight = height / 2f;
		paddingWidth = width * padding;
		paddingHeight = height * padding;
	
		player = GameObject.FindGameObjectWithTag(Tags.player);
		world = GameObject.FindGameObjectWithTag(Tags.world);
	}
	
	// Update is called once per frame
	void Update () {
		anglesToRotate = -45;
		transform.RotateAround(world.transform.position, transform.up, anglesToRotate * Time.deltaTime);
	}
}
