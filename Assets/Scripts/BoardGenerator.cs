using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
	public int cellCount = 9; /// replace this using CharacterData later
	private CombatantBoard board;
	private CellLibrary cellLibrary;
	   
    void Awake()
    {
		board = GetComponent<CombatantBoard>();
		cellLibrary = FindObjectOfType<CellLibrary>();
	}

    public void GenerateBoard()
	{
		List<CellData> cellList = new List<CellData>();
		List<CellSlot> occupiedSlots = new List<CellSlot>();
		if (cellLibrary == null)
			cellLibrary = FindObjectOfType<CellLibrary>();
		int numCellTypes = cellLibrary.allCells.Length;
		if (board == null)
			board = GetComponent<CombatantBoard>();
		int numBoardSlots = board.GetSlots().Count;

		int cells = 0;
		int cellValue = 0;
		int tries = 0;
		while ((cells < cellCount) && (cellValue < numBoardSlots) && (tries < 100))
		{
			foreach (CellSlot cs in board.GetSlots())
			{
				if ((cs.GetCell() == null) || 
					((cs.GetCell() != null) && (cs.GetCell().GetCellData() != null)))
				{
					if ((Random.Range(0f, 1f) > 0.3f) && (cells < cellCount))
					{
						int randomCell = Random.Range(0, numCellTypes);
						CellData cellData = cellLibrary.allCells[randomCell];
						if ((cellValue + cellData.cellValue) <= numBoardSlots)
						{
							cellList.Add(cellData);
							cells++;
							cellValue += cellData.cellValue;
						}
					}
					else
					{
						if (cells < cellCount)
							cellList.Add(null);
					}
				}
			}
			tries++;
		}

		board.LoadBoard(cellList);
	}
}
