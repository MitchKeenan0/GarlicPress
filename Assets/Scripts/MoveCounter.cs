using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCounter : MonoBehaviour
{
	public Text moveCountText;

	private int moves = 0;

    void Start()
    {
		SetMoversMoveable(false);
		UpdateMoveCountText();
	}

	void UpdateMoveCountText()
	{
		moveCountText.text = moves.ToString();
	}

	void SetMoversMoveable(bool value)
	{
		CombatCellMover[] cellMovers = transform.parent.GetComponentsInChildren<CombatCellMover>();
		int count = 0;
		foreach (CombatCellMover cm in cellMovers)
		{
			if ((cm != null) && (cm.GetComponent<CombatantCell>() != null) && (cm.GetComponent<CombatantCell>().GetCellData() != null))
			{
				cm.SetMoveable(value, this);
				count++;
			}
		}
	}

	public void AddMoveToken(int value)
	{
		moves += value;
		moves = Mathf.Clamp(moves, 0, 999);
		if (moves > 0)
			SetMoversMoveable(true);
		UpdateMoveCountText();
	}

	public void SpendMoveToken(int value)
	{
		moves -= value;
		moves = Mathf.Clamp(moves, 0, 999);
		if (moves == 0)
			SetMoversMoveable(false);
		UpdateMoveCountText();
	}
}
