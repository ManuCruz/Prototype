﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour {
	
	public float fadeSpeed = 2f;          
	private bool m_sceneStarting = true;  
	private bool m_sceneEnding = false;  
	private Image m_image;
	private Text m_text;
	
	void Start ()
	{
		m_image = GetComponent<Image>();
		m_image.color = Color.black;
		m_text = GetComponentInParent<Text> ();
		m_text.text = "New Game";
	}
	
	void Update ()
	{
		if(m_sceneStarting)
			StartScene();
		if(m_sceneEnding)
			EndScene();
	}
	
	
	void FadeToClear ()
	{
		m_image.color = Color.Lerp(m_image.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{
		m_image.color = Color.Lerp(m_image.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	
	
	void StartScene ()
	{

		FadeToClear();
		
		if(m_image.color.a <= 0.05f)
		{
			m_image.color = Color.clear;
			m_image.enabled = false;
			m_text.enabled = false;

			m_sceneStarting = false;
		}
	}
	
	public void End (string message){
		m_sceneEnding = true;
		m_text.text = message;
		m_text.enabled = true;
	}
	
	void EndScene ()
	{
		m_image.enabled = true;
		
		FadeToBlack();
		
		if(m_image.color.a >= 0.95f)
			Application.LoadLevel(Application.loadedLevel); 
	}
}