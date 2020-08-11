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
				if (cells[i] != null)
				{
					CombatantCell combatCell = SpawnCombatCell(cells[i], slotList[i]);
					slotList[i].LoadCell(combatCell, false);
				}
			}
		}

		if (health != null)
		{
			int cellCount = GetComponentsInChildren<CombatantCell>().Length;
			health.InitHealth(cellCount);
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();
	}

	public void TakeDamage(int value)
	{
		health.TakeDamage(value);
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

		BoardGenerator bg = GetComponent<BoardGenerator>();
		if (bg != null)
			bg.GenerateBoard();
	}

	public CombatantCell SpawnCombatCell(CellData cellData, CellSlot parentSlot)
	{
		CombatantCell cc = Instantiate(combatCellPrefab, parentSlot.transform);
		cc.LoadCellData(cellData);
		combatCellList.Add(cc);
		parentSlot.LoadCell(cc, false);
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
}
