using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public TextMeshProUGUI gemScore;

	private int gemNum = 0;
	[SerializeField]
	public int gemNumCondition = 5;

	private LevelManager levelManager;

	private void Awake()
	{
		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	private void Start()
	{
		UpdateScore();
	}

	public void AddGems(int amount)
	{
		gemNum += amount;
		UpdateScore();

		if (gemNum >= gemNumCondition)
			levelManager.LoadScene(2);
	}

	private void UpdateScore()
	{
		gemScore.text = gemNum + "";
	}
}
