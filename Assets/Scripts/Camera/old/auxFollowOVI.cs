﻿using UnityEngine;
using System.Collections;

public class auxFollowOVI : MonoBehaviour {

	public float distance = 15f;

	private GameObject m_player;
	
	void Start () {
		m_player = GameObject.FindGameObjectWithTag(Tags.player);
	}

	void Update () {
		Vector3 pos = m_player.transform.position.normalized;
		Camera.main.transform.position = pos * distance;
		Camera.main.transform.forward = m_player.transform.position - Camera.main.transform.position;
		Camera.main.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, m_player.transform.up);
	}
}