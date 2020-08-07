using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlinker : MonoBehaviour
{
	public float FadeDuration = 1f;
	public Color Color1 = Color.gray;
	public Color Color2 = Color.white;

	private Color startColor;
	private Color endColor;
	private float lastColorChangeTime;
	private bool bEnabled = false;

	private Material material;

	void Start()
	{
		material = this.transform.GetComponent<Image>().material;
		startColor = Color1;
		endColor = Color2;
		material.color = Color.white;
	}

	void Update()
	{
		if (bEnabled)
		{
			var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
			ratio = Mathf.Clamp01(ratio);
			material.color = Color.Lerp(startColor, endColor, ratio * ratio);

			if (ratio == 1f)
			{
				lastColorChangeTime = Time.time;

				// Switch colors
				var temp = startColor;
				startColor = endColor;
				endColor = temp;
			}
		}
	}

	public void SetEnabled(bool value)
	{
		bEnabled = value;

		if (!bEnabled)
			material.color = Color1;
	}
}
