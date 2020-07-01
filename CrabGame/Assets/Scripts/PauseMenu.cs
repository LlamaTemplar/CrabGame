using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject root;

	private LevelManager levelManager;

	private void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	private void Start()
	{
		root.SetActive(false);
	}

	void Update()
	{
		if (Input.GetButtonDown("Pause"))
		{
			SetState(!root.activeSelf);
		}
	}

	public void ResumeGame()
	{
		SetState(false);
	}

	private void SetState(bool state)
	{
		root.SetActive(state);
		if (state)
			Time.timeScale = 0.1f;
		else
			Time.timeScale = 1.0f;
	}

	public void LoadSurvey()
	{
		levelManager.LoadScene(2);
	}
}
