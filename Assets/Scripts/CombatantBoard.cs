using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantBoard : MonoBehaviour
{
	public int boardColumnCount = 3;
	public int boardRowCount = 3;
	public int startingCellCount = 5;
	public RectTransform boardRect;
	private GridLayoutGroup grid;
	public CellSlot cellSlotPrefab;
	public CombatantCell combatCellPrefab;

	private Health health;
	private Camera cameraMain;
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
		health = GetComponent<Health>();
		cameraMain = Camera.main;
		InitBoard();
    }

	public void LoadBoard(List<CellData> cells)
	{
		if (slotList != null)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if ((cells[i] != null) && (slotList.Count > i))
				{
					CombatantCell combatCell = SpawnCombatCell(cells[i], slotList[i]);
					slotList[i].LoadCell(combatCell, combatCell.GetCellData(), false);
				}
			}
		}

		if (health != null)
		{
			int cellCount = slotList.Count;
			health.InitHealth(cellCount);
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();
	}

	public void SetBoardTeamID(int value)
	{
		if (combatCellList != null)
		{
			for (int i = 0; i < combatCellList.Count; i++)
			{
				CombatantCell cc = combatCellList[i];
				cc.InitCombatantCell(value);
			}
		}

		if (slotList != null)
		{
			for (int j = 0; j < slotList.Count; j++)
			{
				CellSlot cs = slotList[j];
				cs.SetTeamID(value);
			}
		}
	}

	public void TakeDamage(int value)
	{
		health.TakeDamage(value);
	}

	void InitBoard()
	{
		grid = boardRect.GetComponent<GridLayoutGroup>();
		grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		grid.constraintCount = boardColumnCount;

		boardWidth = boardRect.sizeDelta.x;
		boardHeight = boardRect.sizeDelta.y;

		int numCells = boardColumnCount * boardRowCount;
		for(int i = 0; i < numCells; i++)
		{
			CellSlot cellSlot = Instantiate(cellSlotPrefab, boardRect);
			slotList.Add(cellSlot);
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();

		BoardGenerator bg = GetComponent<BoardGenerator>();
		if (bg != null)
			bg.GenerateBoard();
	}

	public CombatantCell SpawnCombatCell(CellData cellData, CellSlot parentSlot)
	{
		CombatantCell cc = Instantiate(combatCellPrefab, parentSlot.transform);
		combatCellList.Add(cc);
		parentSlot.LoadCell(cc, cellData, false);
		return cc;
	}

	public Transform GetClosestSlotTo(Vector3 position)
	{
		Transform slotTransform = null;
		float closestDistance = 99999f;
		foreach(CellSlot slot in slotList)
		{
			if ((slot.GetCell() == null) || ((slot.GetCell() != null && slot.GetCell().GetCellData() != null)))
			{
				Vector3 slotPosition = slot.transform.position;
				slotPosition.z = 0;
				float distToSlot = Vector3.Distance(position, slotPosition);
				if (distToSlot < closestDistance)
				{
					closestDistance = distToSlot;
					slotTransform = slot.transform;
				}
			}
		}
		return slotTransform;
	}

	public int GetNumCells()
	{
		int result = 0;
		foreach (CellSlot cs in slotList)
		{
			if ((cs.GetCell() != null) && (cs.GetCell().GetCellData() != null))
				result++;
		}
		return result;
	}

	public void MirrorBoard()
	{
		// 1 - store & clear the top and bottom rows
		/// top
		List<CombatantCell> topRowCells = new List<CombatantCell>();
		for(int i = 0; i < boardColumnCount; i++)
		{
			CellSlot slot = slotList[i];
			if (slot != null)
			{
				CombatantCell cell = slot.GetCell();
				if ((cell != null) && (cell.GetCellData() != null))
					topRowCells.Add(cell);
			}
		}
		/// bottom
		List<CombatantCell> bottomRowCells = new List<CombatantCell>();
		for (int i = 0; i < boardColumnCount; i++)
		{
			int mirrorIndex = i + (boardColumnCount * 2);
			CellSlot slot = slotList[mirrorIndex];
			if (slot != null)
			{
				CombatantCell cell = slot.GetCell();
				if ((cell != null) && (cell.GetCellData() != null))
					bottomRowCells.Add(cell);
			}
		}

		// 2 - load exchanged rows
		/// top
		for(int i = 0; i < boardColumnCount; i++)
		{
			if ((i < bottomRowCells.Count) && (bottomRowCells[i] != null))
			{
				CellSlot slot = slotList[i];
				CombatantCell cell = bottomRowCells[i];
				CellData data = cell.GetCellData();

				slot.LoadCell(cell, data, false);
				cell.LoadCellData(data);
				cell.transform.SetParent(slot.transform);
				cell.transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				cell.SetSlot(slot);
			}
		}
		/// bottom
		for(int i = 0; i < boardColumnCount; i++)
		{
			if ((i < topRowCells.Count) && (topRowCells[i] != null))
			{
				int mirrorIndex = i + (boardColumnCount * 2);
				CellSlot slot = slotList[mirrorIndex];
				CombatantCell cell = topRowCells[i];
				CellData data = cell.GetCellData();

				slot.LoadCell(cell, data, false);
				cell.LoadCellData(data);
				cell.transform.SetParent(slot.transform);
				cell.transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				cell.SetSlot(slot);
			}
		}
	}
}
