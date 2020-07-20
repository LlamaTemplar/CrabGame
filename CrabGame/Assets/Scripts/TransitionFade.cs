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

	private TransitionState transitionState;

	public AudioClip fadeInSound;
	public AudioClip fadeOutSound;
	public AudioSource sound;

	public float opacity = 1;

	private void Awake()
	{
		sound = GetComponent<AudioSource>();
	}

	public void Update()
	{
		float increment = Time.deltaTime * (1 / transitionTime) * (transitionState == TransitionState.FadingIn ? -1 : 1);
		opacity = Mathf.Clamp01(opacity + increment);
		Color newColor = new Color(image.color.r, image.color.g, image.color.b, opacity);
		image.color = newColor;
	}

	public void SetTransitionState(TransitionState newState)
	{
		transitionState = newState;
		sound.clip = transitionState == TransitionState.FadingIn ? fadeInSound : fadeOutSound;
	}

	public void PlayTransitionSound()
	{
		sound.Stop();
		sound.Play();
	}
}
