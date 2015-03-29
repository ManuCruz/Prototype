using UnityEngine;
using System.Collections;

public class ChangeScene: MonoBehaviour 
{
	public string[] Escenas;
	
	private static int m_actualScene = 0;

	public string GetNextScene(){
		return Escenas[m_actualScene];
	}

	public void NextScene()
	{
		if(m_actualScene < Escenas.Length - 1)
			m_actualScene++;
	}
}
