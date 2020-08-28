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
		healthValueText.text = value.ToString();
	}
}
