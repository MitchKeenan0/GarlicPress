using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataVinegar : CellData
{
	public int damageGainPerRound = 5;

	private BattleUI battleUI = null;

    void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();
    }

	public override void RoundEndAbility(CombatantCell myCell)
	{
		base.RoundEndAbility(myCell);

		if (myCell != null)
		{
			myCell.ModifyDamage(damageGainPerRound);
			if (!battleUI)
				battleUI = FindObjectOfType<BattleUI>();
			if (battleUI != null)
				battleUI.ToastInteraction(myCell.transform.position, damageGainPerRound, 1, "DMG +");
		}
	}
}
