using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionToast : MonoBehaviour
{
	public Image interactionImage;
	public Text interactionText;

	private CanvasGroup canvasGroup;
	private IEnumerator durationCoroutine;
	private bool bActive = false;

	public bool IsActive() { return bActive; }

    void Start()
    {
		canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetInteractionDetails(Sprite sprite, string value, Color col)
	{
		interactionImage.sprite = sprite;
		interactionImage.color = col;
		interactionText.text = value;
		interactionText.color = col;
	}

	public void SetToastActive(bool value)
	{
		if (canvasGroup == null)
			canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = value ? 1f : 0f;
		if (value)
		{
			durationCoroutine = Duration();
			StartCoroutine(durationCoroutine);
		}
		bActive = value;
	}

	private IEnumerator Duration()
	{
		yield return new WaitForSeconds(1f);
		SetToastActive(false);
	}
}
