using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public CanvasGroup highlightCanvasGroup;

	private Image image;
	private BoardEditor boardEditor;
	private ColorBlinker colorBlinker;
	private CellData cellData;
	private Button button;
	private Sprite slotSprite;
	private Color slotColor;
	private CombatantBoard board;
	private CombatantCell combatCell = null;
	private CombatantCell movingCell = null;

	public CombatantCell GetCell() { return combatCell; }

    void Awake()
    {
		image = GetComponent<Image>();
		colorBlinker = GetComponent<ColorBlinker>();
		boardEditor = FindObjectOfType<BoardEditor>();
		button = GetComponent<Button>();
		slotSprite = image.sprite;
		slotColor = image.color;
		ShowHighlight(false);
		board = transform.parent.parent.GetComponent<CombatantBoard>();
    }

	public void LoadCell(CombatantCell cc, bool bEraseCell)
	{
		if (cc == null)
		{
			if (bEraseCell && (combatCell != null))
				combatCell.LoadCellData(null);
			cellData = null;
		}
		combatCell = cc;
		if (combatCell != null)
			cellData = cc.GetCellData();
		else
			cellData = null;
	}

	public void TakeDamage(int value)
	{
		if ((combatCell != null) && (combatCell.GetCellData() != null))
		{
			combatCell.TakeDamage(value);
		}
		else
		{
			board.TakeDamage(value);
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
		if (boardEditor != null)
		{
			boardEditor.EnableSlotManipulation(false);
			boardEditor.PlaceCell(this);
		}
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

	public void HighlightForDuration(float duration)
	{
		highlightCoroutine = HighlightForTime(duration);
		StartCoroutine(highlightCoroutine);
	}

	private IEnumerator highlightCoroutine;
	private IEnumerator HighlightForTime(float value)
	{
		ShowHighlight(true);
		yield return new WaitForSeconds(value);
		ShowHighlight(false);
	}

	public void ClearCell(bool bEraseCell)
	{
		LoadCell(null, bEraseCell);
		image.sprite = slotSprite;
		image.color = slotColor;
		ColorBlock cb = button.colors;
		cb.normalColor = slotColor;
		button.colors = cb;
	}

	public void OnPointerDown(PointerEventData data)
	{
		if ((combatCell != null) && (combatCell.GetCellData() != null))
		{
			if (boardEditor != null)
			{
				boardEditor.UpdateRemovingSlot(this);
			}
			else
			{
				CombatCellMover cellMover = combatCell.GetComponent<CombatCellMover>();
				if ((cellMover != null) && (cellMover.SetMoving(true, this)))
				{
					RecordMovingCell(combatCell, combatCell.GetCellData());
					combatCell.ShowCanvasGroup(true);
					LoadCell(null, false);
				}
			}
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if ((movingCell != null) && (movingCell.GetCellData() != null))
		{
			movingCell.LoadCellData(movingCell.GetCellData());
			LoadCell(movingCell, false);
			combatCell.GetComponent<CombatCellMover>().SetMoving(false, this);
			combatCell.ShowCanvasGroup(true);
			combatCell.GetComponent<CombatCellMover>().FinishMoveToSlot();
			RecordMovingCell(null, null);
		}
	}

	void RecordMovingCell(CombatantCell cc, CellData cd)
	{
		movingCell = cc;
		if (movingCell != null)
			movingCell.LoadCellData(cd);
	}
}
