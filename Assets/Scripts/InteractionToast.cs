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

    public void SetInteractionDetails(Sprite sprite, string value)
	{
		interactionImage.sprite = sprite;
		interactionText.text = value;
	}

	public void SetToastActive(bool value)
	{
		bActive = value;
		if (canvasGroup == null)
			canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = value ? 1f : 0f;
		if (value)
		{
			durationCoroutine = Duration();
			StartCoroutine(durationCoroutine);
		}
		else
		{
			StopAllCoroutines();
		}
	}

	private IEnumerator Duration()
	{
		yield return new WaitForSeconds(1f);
		SetToastActive(false);
	}
}
