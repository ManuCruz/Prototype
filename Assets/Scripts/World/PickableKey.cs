using UnityEngine;
using System.Collections;

public class PickableKey : MonoBehaviour {

	private GameObject player;
	private PlayerInventory playerInventory;
	
	void Start(){
		player = GameObject.FindGameObjectWithTag (Tags.player);
		playerInventory = player.GetComponent<PlayerInventory>();
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject == player) {
			playerInventory.hasKey = true;
			Destroy(gameObject);
		}
	}
}
