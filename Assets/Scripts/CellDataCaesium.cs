using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataCaesium : CellData
{
	private BattleUI battleUI;

    void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();  
    }

	public override void OnAttackedAbility(CellSlot mySlot, CellSlot targetSlot)
	{
		base.OnAttackedAbility(mySlot, targetSlot);

		CombatantCell targetCell = targetSlot.GetComponentInChildren<CombatantCell>();
		if (targetCell != null)
		{
			int targetCellDamage = targetCell.GetDamage();

			CombatantCell myCell = mySlot.GetCell();
			if (myCell != null)
			{
				if (!battleUI)
					battleUI = FindObjectOfType<BattleUI>();
				battleUI.ToastInteraction(myCell.transform.position, targetCellDamage, 2, "DMG +");
				myCell.SetDamage(targetCellDamage);
				myCell.GetComponent<CellArsenal>().AttackCell(mySlot.transform, targetSlot.transform, myCell.GetDamage());
				myCell.SetDamage(0);
			}
		}
	}
}
