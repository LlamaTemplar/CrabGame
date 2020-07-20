using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TransitionState
{
	FadingIn,
	FadingOut
}

public class TransitionFade : MonoBehaviour
{
	public Image image;

	public float transitionTime = 0.5f;

	public TransitionState transitionState;

	public float opacity = 1;

	// For OnClick Event for Start Button
	public void Update()
	{
		float increment = Time.deltaTime * (1 / transitionTime) * (transitionState == TransitionState.FadingIn ? -1 : 1);
		opacity = Mathf.Clamp01(opacity + increment);
		Color newColor = new Color(image.color.r, image.color.g, image.color.b, opacity);
		image.color = newColor;
	}
}
