using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public TextMeshProUGUI algaeScore;

	private int algaeNum = 0;
	//[SerializeField]
	//public int gemNumCondition = 0;
	public GameObject AlgaeRootObject;

	private LevelManager levelManager;

	private void Awake()
	{
		if (algaeScore == null) algaeScore = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();

		levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
	}

	private void Start()
	{

		if(AlgaeRootObject != null)
		{
			CountAlgae();
		}

		UpdateScore();
	}

	public void RemoveAlgae(int amount)
	{
		algaeNum -= amount;
		UpdateScore();

		if (algaeNum <= 0)
			levelManager.LoadScene(2);
	}

	private void UpdateScore()
	{
		algaeScore.text = algaeNum + "";
	}

	private void CountAlgae()
	{
		algaeNum = AlgaeRootObject.transform.childCount;
	}
}
