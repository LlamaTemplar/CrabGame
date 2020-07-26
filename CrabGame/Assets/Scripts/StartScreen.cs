using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject controls;

	private LevelManager levelManager;

	private void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	// For OnClick Event for Start Button
	public void StartGame()
    {
		// Load the next Scene in the Build settings
		levelManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // For OnClick Event for Controls Button
    public void ShowControls()
    {
        // Hide Start Menu
        startMenu.SetActive(false);
        // Show Controls
        controls.SetActive(true);
    }

    // For OnClick Event for Back Button (After Controls are shown)
    public void HideControls()
    {
        // Hide Controls
        controls.SetActive(false);
        // Show Start Menu
        startMenu.SetActive(true);
    }

    // For OnClick Event for Exit Button
    public void ExitGame()
    {
        // Closes application (Note: will only work if the Application is built and exported, so it will NOT work in the Unity Project)
        Application.Quit();
    }
}
