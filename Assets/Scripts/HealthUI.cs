using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	public Text healthValueText;

    void Start()
    {
        
    }

	public void UpdateHealthValueText(int value)
	{
		int safeValue = Mathf.Clamp(value, 0, 999);
		healthValueText.text = safeValue.ToString();
	}
}
