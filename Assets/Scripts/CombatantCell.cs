using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantCell : MonoBehaviour
{
	private Image cellImage;
	private Health health;
	private CellData cellData;
	private CellSlot mySlot;
	private CanvasGroup canvasGroup;

	public CellData GetCellData() { return cellData; }

    void Awake()
    {
		cellImage = GetComponent<Image>();
		health = GetComponent<Health>();
		canvasGroup = GetComponent<CanvasGroup>();
		ShowCanvasGroup(false);
		mySlot = transform.parent.GetComponent<CellSlot>();
	}

    public void LoadCellData(CellData cd)
	{
		cellData = cd;
		if (cellData != null)
		{
			cellImage.sprite = cellData.cellSprite;
			health.InitHealth(cellData.health);
			ShowCanvasGroup(true);
		}
	}

	public void TakeDamage(int value)
	{
		health.TakeDamage(value);
		if (health.GetHP() <= 0)
			CellDied();
	}

	public void ShowCanvasGroup(bool value)
	{
		canvasGroup.alpha = value ? 1f : 0f;
		canvasGroup.interactable = value;
		canvasGroup.blocksRaycasts = value;
	}

	void CellDied()
	{
		mySlot.ClearCell();
		ShowCanvasGroup(false);
		Destroy(gameObject, 0.5f);
	}
}
