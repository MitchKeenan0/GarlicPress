using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
	public Text conclusionText;

    void Start()
    {
        
    }

    public void SetConclusionText(bool value)
	{
		if (value)
			conclusionText.text = "you won!";
		else
			conclusionText.text = "you lost";
	}
}
