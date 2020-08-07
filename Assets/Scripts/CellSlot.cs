using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSlot : MonoBehaviour
{
	private Image image;
	private BoardEditor boardEditor;
	private ColorBlinker colorBlinker;
	private CellData cellData;
	private Button button;

    void Awake()
    {
		image = GetComponent<Image>();
		colorBlinker = GetComponent<ColorBlinker>();
		boardEditor = FindObjectOfType<BoardEditor>();
		button = GetComponent<Button>();
    }

	public void LoadCell(CellData cd)
	{
		cellData = cd;
		image.sprite = cd.cellSprite;
	}

	public void SetInteractible(bool value)
	{
		button.interactable = value;
	}

	public void SetMaterial(Material value)
	{
		image.material = value;
	}

	public void SelectSlot()
	{
		boardEditor.EnableSlotManipulation(false);
		boardEditor.PlaceCell(this);
	}

	public void SetBlinking(bool value)
	{
		colorBlinker.SetEnabled(value);
	}
}
