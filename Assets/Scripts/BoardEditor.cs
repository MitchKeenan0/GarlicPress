using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardEditor : MonoBehaviour
{
	public float maxCells = 5;
	public Material slotMaterial;
	public Material manipulationMaterial;
	public CanvasGroup cancelButtonCanvasGroup;
	public Text cellCountText;

	private List<CellSlot> slotList;
	private List<CellData> cellDataList;
	private CombatantBoard playerBoard;
	private CellData hotCellData = null;
	private Game game;
	private bool bEditing = false;
	private int numCells = 0;

	void Awake()
	{
		slotList = new List<CellSlot>();
		cellDataList = new List<CellData>();
	}

    void Start()
    {
		playerBoard = GetComponentInChildren<CombatantBoard>();
		EnableCancelButton(false);
		game = FindObjectOfType<Game>();
		game.LoadGame();
    }

	void UpdateCellCount()
	{
		int count = 0;
		foreach(CellSlot cs in slotList)
		{
			if ((cs.GetCell() != null) && (cs.GetCell().GetCellData() != null))
				count++;
		}
		numCells = count;
		cellCountText.text = "00" + numCells + "/" + "00" + maxCells;
	} 

	public void EnableCancelButton(bool value)
	{
		cancelButtonCanvasGroup.alpha = value ? 1f : 0f;
		cancelButtonCanvasGroup.blocksRaycasts = value;
		cancelButtonCanvasGroup.interactable = value;
	}

	public void LoadSlots()
	{
		if (slotList == null)
			slotList = new List<CellSlot>();
		CellSlot[] slots = FindObjectsOfType<CellSlot>();
		foreach (CellSlot cs in slots)
		{
			if (!slotList.Contains(cs))
				slotList.Add(cs);
			CellData slotCellData = null;
			if (cs.GetCell() != null)
				slotCellData = cs.GetCell().GetCellData();
			if (slotCellData != null)
			{
				if (!cellDataList.Contains(slotCellData))
				{
					cellDataList.Add(slotCellData);
				}
			}
		}
		UpdateCellCount();
	}

    public void EnableSlotManipulation(bool value)
	{
		Material enabledMaterial = value ? manipulationMaterial : slotMaterial;
		foreach (CellSlot cs in slotList)
		{
			if ((cs.GetCell() != null) && (numCells < maxCells))
			{
				cs.SetMaterial(enabledMaterial);
				cs.SetBlinking(value);
				cs.SetInteractible(value);
			}
		}
		EnableCancelButton(value);
	}

	public void UpdateHotCell(CellData cell)
	{
		if (cell != null)
		{
			bEditing = true;
			hotCellData = cell;
		}
		else { bEditing = false; hotCellData = null; }
	}

	public void PlaceCell(CellSlot intoSlot)
	{
		if (bEditing && (hotCellData != null))
		{
			int slotIndex = intoSlot.transform.GetSiblingIndex();
			/// placed to empty slot...
			if (((intoSlot.GetCell() != null) && (intoSlot.GetCell().GetCellData() == null)) 
				|| (numCells < maxCells))
			{
				CombatantCell combatCell = playerBoard.SpawnCombatCell(hotCellData, intoSlot);
				intoSlot.LoadCell(combatCell);
				if (cellDataList.Count > slotIndex)
				{
					cellDataList[slotIndex] = hotCellData;
				}
				else
				{
					int difference = (slotIndex - cellDataList.Count) + 1;
					for(int i = 0; i < difference; i++)
					{
						cellDataList.Add(null);
					}
					cellDataList[slotIndex] = hotCellData;
				}
			}
			else if (intoSlot.GetCell() != null)
			{
				/// or overwrite existing cell
				CombatantCell existingCell = intoSlot.GetCell();
				existingCell.LoadCellData(hotCellData);
				intoSlot.LoadCell(existingCell);
				if (cellDataList.Count > slotIndex)
					cellDataList[slotIndex] = hotCellData;
				else
					cellDataList.Add(hotCellData);
			}
			hotCellData = null;
			UpdateCellCount();
			game.SaveGame();
		}
	}

	public void CancelBoardEdit()
	{
		EnableSlotManipulation(false);
	}

	public void ClearBoard()
	{
		foreach (CellSlot cs in slotList)
			cs.ClearCell();
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
		numCells = 0;
		UpdateCellCount();
		game.SaveGame();
	}
}
