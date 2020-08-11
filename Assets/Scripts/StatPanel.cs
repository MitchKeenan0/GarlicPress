using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour
{
	public Text statNameText;
	public Text statValueText;

    void Start()
    {
        
    }

	public void SetStatName(string value)
	{
		statNameText.text = value;
	}

	public void SetStatValue(int value)
	{
		statValueText.text = value.ToString();
	}
}
