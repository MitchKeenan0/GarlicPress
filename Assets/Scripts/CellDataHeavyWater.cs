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

	/// HeavyWater 'decays' into regular Water on death
	public override void OnCellDiedAbility(CombatantCell myCell)
	{
		base.OnCellDiedAbility(myCell);

		myCombatCell = myCell;
		if (myCombatCell != null)
		{
			CellSlot mySlot = myCombatCell.GetSlot();
			if (mySlot != null)
			{
				myCombatCell.LoadCellData(waterCellData);
				mySlot.LoadCell(myCombatCell, false);
				Debug.Log("water cell replaced :)");
			}
			else
			{
				Debug.Log("no cell slot");
			}
		}
		else
		{
			Debug.Log("no combat cell");
		}
	}
}
