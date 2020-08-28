using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootPanel : MonoBehaviour
{
	public Text xpValueText;
	public float scoreCountSpeed = 15f;

	private IEnumerator lootCountCoroutine;
	private int playerScore = 0;

	void Start()
    {
		xpValueText.text = "0";
    }

	public void ActivateScoreCount()
	{
		lootCountCoroutine = CountLoot();
		StartCoroutine(lootCountCoroutine);
	}

	public void UpdateLoot(int score, bool bUpdateText)
	{
		playerScore = score;
		if (bUpdateText)
			xpValueText.text = playerScore.ToString();
	}

	private IEnumerator CountLoot()
	{
		float deltaTime = 0.05f;
		float scoreCount = 0;
		float t = 0f;
		while (scoreCount < playerScore)
		{
			float interpScore = Mathf.MoveTowards(scoreCount, playerScore, t);
			t += Time.deltaTime * scoreCountSpeed;
			scoreCount = interpScore;
			int intScore = Mathf.RoundToInt(scoreCount);
			xpValueText.text = intScore.ToString();
			int intSize = Mathf.RoundToInt(scoreCount * 5.6f);
			xpValueText.fontSize = Mathf.Clamp(intSize, 30, 85);

			yield return new WaitForSeconds(deltaTime);
		}
	}
}
