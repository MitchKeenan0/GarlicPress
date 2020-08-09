using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Matchmaker : MonoBehaviour
{
	public float minimumLoadTime = 0.6f;
	public CanvasGroup statusCanvasGroup;
	public Text statusText;
	public Text searchTimeText;
	public string searchTimeClassifierText = "";

	private bool bActive = false;
	private bool bLaunched = false;
	private float searchTime = 0f;

    void Start()
    {
		ShowStatusCG(false);
    }

	void Update()
	{
		if (bActive)
		{
			searchTime += Time.deltaTime;
			searchTimeText.text = searchTime.ToString("F2");
			/// Temp code until Looking For Game
			if (!bLaunched && (searchTime >= minimumLoadTime))
			{
				Launch();
			}
		}
	}

	void Launch()
	{
		SceneManager.LoadScene("BattleScene");
	}

	void ShowStatusCG(bool value)
	{
		statusCanvasGroup.alpha = value ? 1f : 0f;
		statusCanvasGroup.blocksRaycasts = value;
		statusCanvasGroup.interactable = value;
	}

	public void PlayBattle()
	{
		ShowStatusCG(true);
		bActive = true;
		/// temp code until LFG
		minimumLoadTime = Random.Range(0.8f, 2.6f);
	}

	public void PlayCoop()
	{

	}
}
