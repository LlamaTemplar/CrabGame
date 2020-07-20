using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
	//public TextMeshProUGUI gemScore;
	[HideInInspector]
	public int gemNum = 0;

	private int sceneToLoad = -1;

	[SerializeField]
	private TransitionFade faderPrefab;
	private TransitionFade fader;

	private static LevelManager Instance;

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
		if (!faderPrefab)
			return;

		fader = Instantiate(faderPrefab);
		fader.transform.SetParent(FindObjectOfType<Canvas>().transform);
		fader.GetComponent<RectTransform>().offsetMin = Vector2.zero;
		fader.GetComponent<RectTransform>().offsetMax = Vector2.zero;
		fader.transform.localScale = Vector2.one;

		fader.transitionState = TransitionState.FadingIn;
		fader.opacity = firstTime ? 0 : 1;
	}

	public void LoadScene(int index)
	{
		sceneToLoad = index;
		Time.timeScale = 1;
		StartCoroutine("LoadSceneCoroutine");
	}

	public IEnumerator LoadSceneCoroutine()
	{
		fader.transitionState = TransitionState.FadingOut;

		yield return new WaitForSeconds(fader.transitionTime);

		SceneManager.LoadScene(sceneToLoad);
	}
}
