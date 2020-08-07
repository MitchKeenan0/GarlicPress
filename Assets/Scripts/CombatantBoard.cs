using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantBoard : MonoBehaviour
{
	public int width = 3;
	public int height = 3;
	public RectTransform boardRect;
	private GridLayoutGroup grid;
	public CellSlot cellSlotPrefab;
	public CellData firstCellPrefab;

	private float boardWidth = -1f;
	private float boardHeight = -1f;

    void Start()
    {
		InitBoard();
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
			cellSlot.LoadCell(firstCellPrefab);
		}

		BoardEditor be = FindObjectOfType<BoardEditor>();
		if (be != null)
			be.LoadSlots();
	}
}
