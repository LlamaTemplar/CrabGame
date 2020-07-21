using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarMover : MonoBehaviour
{
	public HealthBar hpBarPrefab;
	private HealthBar hpBar;
	public Unit parentUnit;

	void Awake()
	{
		hpBar = Instantiate(hpBarPrefab);

		// Make it visible in the UI
		hpBar.transform.SetParent(FindObjectOfType<Canvas>().transform);

		// Connect to Parent Unit
		if (parentUnit.GetHealthBar() == null)
			parentUnit.SetHealthBar(hpBar);
	}

    // Update is called once per frame
    void Update()
    {
		UpdateHealthBarPosition();
	}

	// Update HPBar position
	private void UpdateHealthBarPosition()
	{
		Vector3 barPosition = new Vector3(transform.position.x, transform.position.y + 1, 0);

		RectTransform rect = hpBar.GetComponent<RectTransform>();

		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, barPosition);

		rect.position = new Vector2(screenPoint.x, screenPoint.y);
	}
}
