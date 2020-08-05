using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEditor : MonoBehaviour
{
	public Material slotMaterial;
	public Material manipulationMaterial;

	private List<CellSlot> slotList;

    void Start()
    {
		slotList = new List<CellSlot>();
    }

	public void InitSlots()
	{
		CellSlot[] slots = GetComponentsInChildren<CellSlot>();
		foreach (CellSlot cs in slots)
			slotList.Add(cs);
	}

    public void EnableSlotManipulation(bool value)
	{
		Material enabledMaterial = value ? manipulationMaterial : slotMaterial;
		foreach (CellSlot cs in slotList)
			cs.SetMaterial(enabledMaterial);
	}
}
