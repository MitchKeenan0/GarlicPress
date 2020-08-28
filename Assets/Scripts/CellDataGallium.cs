using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataGallium : CellData
{
	public int healPerRound = 5;

	private BattleUI battleUI;

	void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();
    }

	public override void RoundEndAbility(CombatantCell myCell)
	{
		base.RoundEndAbility(myCell);
		myCell.ModifyHealth(healPerRound);
		if (!battleUI)
			battleUI = FindObjectOfType<BattleUI>();
		battleUI.ToastInteraction(myCell.transform.position, healPerRound, 3, "hp +");
	}
}
