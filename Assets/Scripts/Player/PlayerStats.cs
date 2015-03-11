using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	bool alive = true;

	public void PlayerDead(){
		alive = false;
		Debug.Log ("PJ muerto");
	}
}
