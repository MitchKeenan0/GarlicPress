using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDataSoap : CellData
{
	public int potentialFragmentsOnCellDied = 2;
	public float fragmentDropRate = 0.5f;

	private int numFragmentsDropped = 0;

    void Start()
    {
        
    }

	public override void OnCellDiedAbility(CombatantCell myCell)
	{
		base.OnCellDiedAbility(myCell);

		if (myCell != null)
		{
			CellSlot cellSlot = myCell.GetSlot();
			if (cellSlot != null)
			{
				List<CellSlot> neighborSlots = new List<CellSlot>();
				neighborSlots = cellSlot.GetNeighbors();
				foreach(CellSlot cs in neighborSlots)
				{
					if ((cs.GetCell() == null) 
						&& (Random.Range(0f, 1f) >= fragmentDropRate))
					{
						CombatantBoard board = cs.GetBoard();
						board.SpawnCombatCell(this, cs);
						numFragmentsDropped++;
					}

					if (numFragmentsDropped >= potentialFragmentsOnCellDied)
						break;
				}
			}

			bOnCellDiedAbility = false;
			myCell.CellDied();
		}
	}
}
