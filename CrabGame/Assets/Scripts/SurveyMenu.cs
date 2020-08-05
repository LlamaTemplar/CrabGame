using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyMenu : MonoBehaviour
{
	private LevelManager levelManager;

	private void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	public void RestartGame()
	{
		levelManager.LoadScene(1);
	}

	public void MainMenu()
	{
		levelManager.LoadScene(0);
	}
}
