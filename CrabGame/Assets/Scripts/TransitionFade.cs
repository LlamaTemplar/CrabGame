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
	private Image image;
	private AudioSource soundPlayer;

	public float transitionTime = 0.5f; // How long it takes to full fade in/out

	private TransitionState transitionState;

	public float opacity = 1;

	public AudioClip fadeInSound;
	public AudioClip fadeOutSound;

	private void Awake()
	{
		image = GetComponent<Image>();
		soundPlayer = GetComponent<AudioSource>();
	}

	public void Update()
	{
		// Should opacity increase or decrease this Update()?
		float increment = Time.deltaTime * (1 / transitionTime) * (transitionState == TransitionState.FadingIn ? -1 : 1);
		opacity = Mathf.Clamp01(opacity + increment);

		// Update alpha channel of image
		Color newColor = new Color(image.color.r, image.color.g, image.color.b, opacity);
		image.color = newColor;
	}

	// Set transition state and other related fields
	public void SetTransitionState(TransitionState newState)
	{
		transitionState = newState;
		soundPlayer.clip = transitionState == TransitionState.FadingIn ? fadeInSound : fadeOutSound;
	}

	public void PlayTransitionSound()
	{
		soundPlayer.Stop();
		soundPlayer.Play();
	}
}
