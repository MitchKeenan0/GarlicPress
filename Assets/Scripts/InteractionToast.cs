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

	void Update()
	{
		if (bActive)
			transform.position += (Vector3.up * Time.deltaTime * 0.1f);
	}

	public void SetInteractionDetails(Sprite sprite, string value, Color col, Vector3 textOffset)
	{
		interactionImage.sprite = sprite;
		Color interactionImageColor = col * 0.6f;
		interactionImageColor.a = 1f;
		interactionImage.color = interactionImageColor;

		interactionText.text = value;
		interactionText.color = col;
		interactionText.transform.localPosition = textOffset;
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
