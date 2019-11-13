using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RetryScreen : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvas;
	[SerializeField] private float fadeInDuration = 1f;
	[SerializeField] private Health player;
	[SerializeField] private TextMeshProUGUI scoreText;

	private float startTimestamp;
	private float deathTimestamp;

	private void Start()
	{
		canvas.alpha = 0;
		startTimestamp = Time.time;
	}

	private void Update()
	{
		if (!player.IsAlive())
		{
			SetScoreText();
			ActivateRetryScreenCanvas();

			if (canvas.alpha < 1f)
			{
				canvas.alpha += Time.deltaTime * fadeInDuration;
			}
		}
	}

	private void ActivateRetryScreenCanvas()
	{
		if(canvas.gameObject.activeInHierarchy != true)
		{
			canvas.gameObject.SetActive(true);
		}
	}

	private void SetScoreText()
	{
		if (Mathf.Approximately(deathTimestamp, 0f))
		{
			deathTimestamp = Time.time;

			float timeSurvived = deathTimestamp - startTimestamp;
			string minutes = Mathf.Floor(timeSurvived / 60).ToString("00");
			string seconds = Mathf.Floor(timeSurvived % 60).ToString("00");

			scoreText.text = string.Format("{0}:{1}", minutes, seconds);
		}
	}

	public void ReloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
