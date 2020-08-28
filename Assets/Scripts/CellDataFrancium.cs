using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataFrancium : CellData
{
	private BattleUI battleUI = null;

    void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();
    }

	/// Francium absorbs health from damage
	public override bool OnAttackedAbility(CellSlot mySlot, CellSlot targetSlot)
	{
		base.OnAttackedAbility(mySlot, targetSlot);

		bool bAttackThrough = true;

		CombatantCell targetCell = targetSlot.GetComponentInChildren<CombatantCell>();
		if (targetCell != null)
		{
			int targetCellDamage = targetCell.GetDamage();

			CombatantCell myCell = mySlot.GetCell();
			if (myCell != null)
			{
				if (!battleUI)
					battleUI = FindObjectOfType<BattleUI>();
				battleUI.ToastInteraction(myCell.transform.position, targetCellDamage, 3, "hp +");

				myCell.ModifyHealth(targetCellDamage);

				bAttackThrough = false;
			}
		}

		return bAttackThrough;
	}
}
