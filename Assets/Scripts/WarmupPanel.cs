using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmupPanel : MonoBehaviour
{
	public Text warmupText;
	public Transform timerBarTransform;

	private float fullBarWidth = 0f;
	private float durationTime = 0f;
	private float duration = 0f;
	private bool bTimerActive = false;

	void Start()
    {
		fullBarWidth = timerBarTransform.localScale.x;
		ZeroBar();
	}

	void Update()
	{
		if (bTimerActive)
		{
			durationTime += Time.deltaTime;

			Vector3 timerBarScale = timerBarTransform.localScale;
			float timerWidth = fullBarWidth * (durationTime / duration);
			timerBarScale.x = timerWidth;
			timerBarTransform.localScale = timerBarScale;

			if (durationTime >= duration)
				bTimerActive = false;
		}
	}

	void ZeroBar()
	{
		Vector3 initBarScale = Vector3.one;
		initBarScale.x = 0f;
		timerBarTransform.localScale = initBarScale;
	}

	public void ActivateWarmupTimer(float value)
	{
		bTimerActive = true;
		duration = value;
		durationTime = 0f;
	}

    public void SetWarmupMessage(string value)
	{
		warmupText.text = value;
	}
}
