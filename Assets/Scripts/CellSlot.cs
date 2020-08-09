using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSlot : MonoBehaviour
{
	public CanvasGroup highlightCanvasGroup;
	private Image image;
	private BoardEditor boardEditor;
	private ColorBlinker colorBlinker;
	private CellData cellData;
	private Button button;
	private Sprite slotSprite;
	private Color slotColor;

	public CellData GetCell() { return cellData; }

    void Awake()
    {
		image = GetComponent<Image>();
		colorBlinker = GetComponent<ColorBlinker>();
		boardEditor = FindObjectOfType<BoardEditor>();
		button = GetComponent<Button>();
		slotSprite = image.sprite;
		slotColor = image.color;
		ShowHighlight(false);
    }

	public void LoadCell(CellData cd)
	{
		cellData = cd;
		if (cellData != null)
		{
			image.sprite = cd.cellSprite;
			image.color = Color.white;
		}
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
		ShowHighlight(value);
		//colorBlinker.SetEnabled(value);
	}

	public void ShowHighlight(bool value)
	{
		highlightCanvasGroup.alpha = value ? 1f : 0f;
	}

	public void ClearCell()
	{
		cellData = null;
		image.sprite = slotSprite;
		image.color = slotColor;
	}
}
