using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarMover : MonoBehaviour
{
	public HealthBar hpBarPrefab;
	private HealthBar hpBar;
	public Unit parent;

	void Awake()
	{
		hpBar = Instantiate(hpBarPrefab);
		hpBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
		if (parent.GetHealthBar() == null)
			parent.SetHealthBar(hpBar);
	}

    // Update is called once per frame
    void Update()
    {
		UpdateHPBarPosAndVis();
	}

	// Update HPBar position and enemy/ally state
	private void UpdateHPBarPosAndVis()
	{
		// Position bar
		Vector3 barPosition = new Vector3(transform.position.x, transform.position.y + 1, 0);

		if (!hpBar.gameObject.activeSelf)
			hpBar.gameObject.SetActive(true);

		RectTransform rect = hpBar.GetComponent<RectTransform>();

		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, barPosition);

		rect.position = new Vector2(screenPoint.x, screenPoint.y);
	}
}
