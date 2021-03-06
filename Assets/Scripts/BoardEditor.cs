﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardEditor : MonoBehaviour
{
	public int maxBoardValue = 9;
	public Material slotMaterial;
	public Material manipulationMaterial;
	public CanvasGroup cancelButtonCanvasGroup;
	public Text cellCountText;
	public Transform removeCellPanel;

	private List<CellSlot> slotList;
	private List<CellData> boardCellDataList;
	private CombatantBoard playerBoard;
	private CellData hotCellData = null;
	private CellSlot removingSlot = null;
	private Game game;
	private CanvasGroup removeCellCanvasGroup;
	private bool bEditing = false;
	private int boardValue = 0;

	void Awake()
	{
		slotList = new List<CellSlot>();
		boardCellDataList = new List<CellData>();
	}

    void Start()
    {
		playerBoard = GetComponentInChildren<CombatantBoard>();
		removeCellCanvasGroup = removeCellPanel.GetComponent<CanvasGroup>();
		EnableCancelButton(false);
		EnableRemovePanel(false);
		game = FindObjectOfType<Game>();
		game.LoadGame();
    }

	void UpdateBoardValue()
	{
		int totalCellValue = 0;
		foreach(CellSlot cs in slotList)
		{
			if ((cs.GetCell() != null) && (cs.GetCell().GetCellData() != null))
				totalCellValue += cs.GetCell().GetCellData().cellValue;
		}
		boardValue = totalCellValue;
		cellCountText.text = boardValue + "/" + maxBoardValue;
	} 

	public void EnableCancelButton(bool value)
	{
		cancelButtonCanvasGroup.alpha = value ? 1f : 0f;
		cancelButtonCanvasGroup.blocksRaycasts = value;
		cancelButtonCanvasGroup.interactable = value;
	}

	public void EnableRemovePanel(bool value)
	{
		removeCellCanvasGroup.alpha = value ? 1f : 0f;
		removeCellCanvasGroup.blocksRaycasts = value;
		removeCellCanvasGroup.interactable = value;
	}

	public void UpdateRemovingSlot(CellSlot value)
	{
		removingSlot = value;
		removeCellPanel.transform.position = value.transform.position;
		if ((value != null) && (hotCellData == null))
			EnableRemovePanel(true);
	}

	public void RemoveCell()
	{
		if (removingSlot != null)
		{
			removingSlot.ClearCell(true);
			removingSlot = null;
			hotCellData = null;

			UpdateBoardValue();
			game.SaveGame();
			EnableRemovePanel(false);
		}
	}

	public void CancelRemoveCell()
	{
		removingSlot = null;
		EnableRemovePanel(false);
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
				if (!boardCellDataList.Contains(slotCellData))
				{
					boardCellDataList.Add(slotCellData);
				}
			}
		}
		maxBoardValue = slots.Length;
		UpdateBoardValue();
	}

    public void EnableSlotManipulation(bool value)
	{
		Material enabledMaterial = value ? manipulationMaterial : slotMaterial;

		foreach (CellSlot cs in slotList)
		{
			/// enable empty cells if we can afford it
			int hotCellValue = hotCellData.cellValue;
			bool bEmptie = (cs.GetCell() == null) && ((boardValue + hotCellValue) <= maxBoardValue);

			/// or replace occupied cells
			bool bOccupie = (cs.GetCell() != null) && (cs.GetCell().GetCellData() != null);
			if (bOccupie)
			{
				int occupiedCellValue = cs.GetCell().GetCellData().cellValue;
				if ((boardValue + hotCellValue - occupiedCellValue) > maxBoardValue)
					bOccupie = false;
			}

			if (bEmptie || bOccupie)
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
			int placingSlotIndex = intoSlot.transform.GetSiblingIndex();
			
			/// placed to empty slot...
			if (((intoSlot.GetCell() == null) || ((intoSlot.GetCell() != null) && (intoSlot.GetCell().GetCellData() == null)))
				&& ((boardValue + hotCellData.cellValue) <= maxBoardValue))
			{
				CombatantCell combatCell = playerBoard.SpawnCombatCell(hotCellData, intoSlot);
				intoSlot.LoadCell(combatCell, hotCellData, false);
				if (boardCellDataList.Count > placingSlotIndex)
				{
					boardCellDataList[placingSlotIndex] = hotCellData;
				}
				else
				{
					int difference = (placingSlotIndex - boardCellDataList.Count) + 1;
					for(int i = 0; i < difference; i++)
					{
						boardCellDataList.Add(null);
					}
					boardCellDataList[placingSlotIndex] = hotCellData;
				}
			}
			else if (intoSlot.GetCell() != null)
			{
				/// or overwrite existing cell
				CombatantCell existingCell = intoSlot.GetCell();
				existingCell.LoadCellData(hotCellData);
				intoSlot.LoadCell(existingCell, hotCellData, false);
				if (boardCellDataList.Count > placingSlotIndex)
					boardCellDataList[placingSlotIndex] = hotCellData;
				else
					boardCellDataList.Add(hotCellData);
			}

			hotCellData = null;
			bEditing = false;
			UpdateBoardValue();
			game.SaveGame();
		}
	}

	public void CancelBoardEdit()
	{
		EnableSlotManipulation(false);
	}

	public void ClearBoard()
	{
		if (removingSlot != null)
		{
			removingSlot.ClearCell(true);
			removingSlot = null;
		}

		if (hotCellData != null)
			UpdateHotCell(null);

		foreach (CellSlot cs in slotList)
			cs.ClearCell(true);

		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
		UpdateBoardValue();
		game.SaveGame();
	}
}
