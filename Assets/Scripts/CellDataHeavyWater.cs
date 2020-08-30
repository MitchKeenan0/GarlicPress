using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataHeavyWater : CellData
{
	public CellData waterCellData;

	private CombatantCell myCombatCell = null;

    void Start()
    {
        
    }

	/// HeavyWater 'decays' into regular Water when eliminated
	public override void OnCellDiedAbility(CombatantCell myCell)
	{
		base.OnCellDiedAbility(myCell);

		myCombatCell = myCell;
		if (myCombatCell != null)
		{
			CellSlot mySlot = myCombatCell.GetSlot();
			if (mySlot != null)
			{
				mySlot.LoadCell(myCombatCell, waterCellData, false);
			}
		}
	}
}
