using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject root;
	public GameObject controlRoot;

	private LevelManager levelManager;

	private void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	private void Start()
	{
		root.SetActive(false);
		controlRoot.SetActive(false);
	}

	void Update()
	{
		// Show/hide menu when user presses the Pause button
		if (Input.GetButtonDown("Pause"))
		{
			SetState(!root.activeSelf);
		}
	}

	public void ResumeGame()
	{
		SetState(false);
	}

	public void RestartGame()
	{
		levelManager.ReLoadScene();
	}

	public void LoadSurvey()
	{
		levelManager.LoadScene(0);
	}

	public void ShowControls(bool controlState)
	{
		controlRoot.SetActive(controlState);
	}

	private void SetState(bool state)
	{
		root.SetActive(state);
		controlRoot.SetActive(controlRoot.activeSelf && state);
		if (state)
			Time.timeScale = 0.1f;
		else
			Time.timeScale = 1.0f;
	}
}
