using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public CanvasGroup highlightCanvasGroup;
	public ParticleSystem damageParticles;

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

	public void LoadCell(CombatantCell cc)
	{
		if (cc == null)
			cellData = null;
		combatCell = cc;
		if (combatCell != null)
			cellData = cc.GetCellData();
		else
			cellData = null;
	}

	public void TakeDamage(int value)
	{
		damageParticles.Play();
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

	public void ClearCell()
	{
		LoadCell(null);
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
			RecordMovingCell(combatCell, combatCell.GetCellData());
			combatCell.GetComponent<CombatCellMover>().SetMoving(true, this);
			combatCell.ShowCanvasGroup(true);
			LoadCell(null);
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if ((movingCell != null) && (movingCell.GetCellData() != null))
		{
			movingCell.LoadCellData(movingCell.GetCellData());
			LoadCell(movingCell);
			combatCell.GetComponent<CombatCellMover>().SetMoving(false, this);
			combatCell.ShowCanvasGroup(true);
			combatCell.GetComponent<CombatCellMover>().FinishMoveToSlot();
		}
	}

	void RecordMovingCell(CombatantCell cc, CellData cd)
	{
		movingCell = cc;
		movingCell.LoadCellData(cd);
	}
}
