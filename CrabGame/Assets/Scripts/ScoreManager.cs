using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public TextMeshProUGUI gemScore;
	[HideInInspector]
	private int gemNum = 0;

	private void Start()
	{
		UpdateScore();
	}

	public void AddGems(int amount)
	{
		gemNum += amount;
		UpdateScore();
	}

	private void UpdateScore()
	{
		gemScore.text = gemNum + "";
	}
}
