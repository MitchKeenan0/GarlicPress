using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
	public int cellCount = 9; /// replace this using CharacterData later

	private CombatantBoard board;
	private CellLibrary cellLibrary;
	private int cells = 0;
	private int cellValue = 0;
	private int tries = 0;
	private int numCellTypes = 0;
	private int numBoardSlots = 0;
	private List<CellData> cellList;
	private List<CellSlot> boardSlotList;
	private List<CellSlot> occupiedSlots;

	void Awake()
    {
		board = GetComponent<CombatantBoard>();
		cellLibrary = FindObjectOfType<CellLibrary>();
		cellList = new List<CellData>();
		boardSlotList = new List<CellSlot>();
		occupiedSlots = new List<CellSlot>();
	}

    public void GenerateBoard()
	{
		if (cellLibrary == null)
			cellLibrary = FindObjectOfType<CellLibrary>();
		if (board == null)
			board = GetComponent<CombatantBoard>();

		boardSlotList = board.GetSlots();
		numCellTypes = cellLibrary.allCells.Length;
		numBoardSlots = boardSlotList.Count;

		while ((cells < cellCount) && (cellValue < numBoardSlots) && (tries < 100))
		{
			/// first row guaranteed
			for(int i = 0; i < board.boardColumnCount; i++)
			{
				CellSlot cs = boardSlotList[i];
				NewCell();
			}

			/// remaining cells randomly
			foreach (CellSlot cs in boardSlotList)
			{
				if ((cs.GetCell() == null) ||
					((cs.GetCell() != null) && (cs.GetCell().GetCellData() != null)))
				{
					if ((Random.Range(0f, 1f) > 0.3f) && (cells < cellCount))
					{
						NewCell();
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

	void NewCell()
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
}
