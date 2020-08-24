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
		bOnCellDiedAbility = true;
		numFragmentsDropped = 0;
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
				neighborSlots = cellSlot.GetNeighborSlots();
				numFragmentsDropped = 0;

				foreach (CellSlot cs in neighborSlots)
				{
					float randomFragmentShot = Random.Range(0f, 1f);

					if (((cs.GetCell() == null) || ((cs.GetCell() != null) && (cs.GetCell().GetCellData() == null)))
						&& (randomFragmentShot <= fragmentDropRate))
					{
						CombatantBoard board = cs.GetBoard();
						board.SpawnCombatCell(this, cs);
						numFragmentsDropped++;
					}

					if (numFragmentsDropped >= potentialFragmentsOnCellDied)
						break;
				}
			}

			myCell.ShowCanvasGroup(false);
			Destroy(myCell, 0.5f);
		}
	}
}
