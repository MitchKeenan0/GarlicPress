﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBattle : Game
{
	public CanvasGroup loadPageCanvasGroup;
	public float loadDuration = 1f;

	private Battle battle;
	private IEnumerator loadCoroutine;

    void Start()
    {
		battle = FindObjectOfType<Battle>();
		loadCoroutine = LoadDelay();
		StartCoroutine(loadCoroutine);
    }

	void ShowLoadPage(bool value)
	{
		loadPageCanvasGroup.alpha = value ? 1f : 0f;
		loadPageCanvasGroup.interactable = value;
		loadPageCanvasGroup.blocksRaycasts = value;
	}

	public override void SaveGame()
	{
		base.SaveGame();
	}

	public override void LoadGame()
	{
		base.LoadGame();
	}

	private IEnumerator LoadDelay()
	{
		ShowLoadPage(true);
		LoadGame();
		yield return new WaitForSeconds(loadDuration);
		battle.InitBattle(playerBoard, opponentBoard);
		ShowLoadPage(false);
	}
}
