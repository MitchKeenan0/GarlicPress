using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEditor : MonoBehaviour
{
	public Material slotMaterial;
	public Material manipulationMaterial;
	public CanvasGroup cancelButtonCanvasGroup;

	private List<CellSlot> slotList;
	private CellData hotCellData = null;
	private bool bEditing = false;

    void Awake()
    {
		slotList = new List<CellSlot>();
		EnableCancelButton(false);
    }

	public void EnableCancelButton(bool value)
	{
		cancelButtonCanvasGroup.alpha = value ? 1f : 0f;
		cancelButtonCanvasGroup.blocksRaycasts = value;
		cancelButtonCanvasGroup.interactable = value;
	}

	public void LoadSlots()
	{
		CellSlot[] slots = FindObjectsOfType<CellSlot>();
		foreach (CellSlot cs in slots)
			slotList.Add(cs);
	}

    public void EnableSlotManipulation(bool value)
	{
		Material enabledMaterial = value ? manipulationMaterial : slotMaterial;
		foreach (CellSlot cs in slotList)
		{
			cs.SetMaterial(enabledMaterial);
			cs.SetBlinking(value);
			cs.SetInteractible(value);
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

	public void PlaceCell(CellSlot cs)
	{
		if (bEditing && (hotCellData != null))
		{
			cs.LoadCell(hotCellData);
			bEditing = true;
			hotCellData = null;
		}
	}

	public void CancelBoardEdit()
	{
		EnableSlotManipulation(false);
	}
}
