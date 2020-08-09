using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantBoard : MonoBehaviour
{
	public int width = 3;
	public int height = 3;
	public int startingCellCount = 5;
	public RectTransform boardRect;
	private GridLayoutGroup grid;
	public CellSlot cellSlotPrefab;
	public CellData firstCellPrefab;
	public bool bLiveCells = false;
	public CombatantCell combatCellPrefab;

	private float boardWidth = -1f;
	private float boardHeight = -1f;
	private List<CellSlot> slotList;
	private List<CellData> cellList;
	private List<CombatantCell> combatCellList;

	public List<CellSlot> GetSlots() { return slotList; }

    void Awake()
    {
		slotList = new List<CellSlot>();
		cellList = new List<CellData>();
		combatCellList = new List<CombatantCell>();
		InitBoard();
    }

	public void LoadBoard(List<CellData> cells)
	{
		if (slotList == null)
		{
			Debug.Log("no slot list");
		}
		if (slotList != null)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				slotList[i].LoadCell(cells[i]);
				if (bLiveCells)
					SpawnCombatCell(cells[i], slotList[i].transform);
			}
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();
	}

	void InitBoard()
	{
		grid = boardRect.GetComponent<GridLayoutGroup>();
		grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		grid.constraintCount = width;

		boardWidth = boardRect.sizeDelta.x;
		boardHeight = boardRect.sizeDelta.y;

		int numCells = width * height;
		for(int i = 0; i < numCells; i++)
		{
			CellSlot cellSlot = Instantiate(cellSlotPrefab, boardRect);
			slotList.Add(cellSlot);
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();
	}

	void SpawnCombatCell(CellData cellData, Transform parentTransform)
	{
		CombatantCell cc = Instantiate(combatCellPrefab, parentTransform);
		cc.LoadCell(cellData);
		combatCellList.Add(cc);
	}
}
