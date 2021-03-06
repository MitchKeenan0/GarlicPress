﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
	public string characterName = "unnamed character";
	public int characterLevel = 1;
	public CellData[] cells;

	private int characterXp = 0;

	void Awake()
	{
		DontDestroyChildOnLoad(gameObject);
	}

	public virtual void AddXP(int value)
	{
		characterXp += value;
	}

	public virtual void ResetXP(int value)
	{
		characterXp = value;
	}

	public static void DontDestroyChildOnLoad(GameObject child)
	{
		Transform parentTransform = child.transform;

		// If this object doesn't have a parent then its the root transform.
		while (parentTransform.parent != null)
		{
			// Keep going up the chain.
			parentTransform = parentTransform.parent;
		}
		GameObject.DontDestroyOnLoad(parentTransform.gameObject);
	}
}
