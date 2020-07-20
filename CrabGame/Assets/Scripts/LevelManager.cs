using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
	//public TextMeshProUGUI gemScore;
	[HideInInspector]
	public int gemNum = 0;

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

	private void FixedUpdate()
	{
		//gemScore.text = gemNum.ToString();
		
	}

	public void LoadScene(int index)
	{
		Time.timeScale = 1;
		UnityEngine.SceneManagement.SceneManager.LoadScene(index);
	}
}
