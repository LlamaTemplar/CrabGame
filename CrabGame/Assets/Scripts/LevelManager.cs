using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// This has to be a root object in the scene! Don't parent it to other GameObjects!
public class LevelManager : MonoBehaviour
{
	private int sceneToLoad = -1;
	
	[SerializeField]
	private TransitionFade faderPrefab = null; // Used for transitions
	private TransitionFade fader;

	private static LevelManager Instance; // The only LevelManager

	// Save a static reference to the global LevelManager
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
			Destroy(this);
	}

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SpawnTransitionObjects(true);
	}

	private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
	{
		Debug.Log("Scene loaded");
		SpawnTransitionObjects(false);
	}

	private void SpawnTransitionObjects(bool firstTime)
	{
		// No transition object to spawn
		if (!faderPrefab)
			return;

		// Set a reference to this scene's transition object
		fader = Instantiate(faderPrefab);
		// Position it to cover the screen
		fader.transform.SetParent(FindObjectOfType<Canvas>().transform);
		fader.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		fader.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		fader.transform.localScale = Vector2.one;

		// Since we are calling this at the OnSceneLoaded event, we must be fading in to the scene
		fader.SetTransitionState(TransitionState.FadingIn);
		if (!firstTime)
		{
			fader.opacity = 1;

			fader.PlayTransitionSound();			
		}
		else // No need to play a transition when we press the play button
			fader.opacity = 0;
	}

	public void LoadScene(int index)
	{
		// Resets any time effects from the previous scene
		Time.timeScale = 1;

		sceneToLoad = index;
		StartCoroutine("LoadSceneCoroutine");
	}

	public IEnumerator LoadSceneCoroutine()
	{
		// Setup transition object
		fader.SetTransitionState(TransitionState.FadingOut);
		fader.PlayTransitionSound();

		// Wait for transition to finish
		yield return new WaitForSeconds(fader.transitionTime);

		SceneManager.LoadScene(sceneToLoad);
	}
}
