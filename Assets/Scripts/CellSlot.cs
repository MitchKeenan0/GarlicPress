using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSlot : MonoBehaviour
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
	private CombatantCell combatCell;

	public CellData GetCell() { return cellData; }
	public void LoadCombatCell(CombatantCell cc) { combatCell = cc; }

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

	public void LoadCell(CellData cd)
	{
		cellData = cd;
		if (cellData != null)
		{
			image.sprite = cd.cellSprite;
			image.color = Color.white;
		}
		else
		{
			ClearCell();
		}
	}

	public void TakeDamage(int value)
	{
		damageParticles.Play();
		if (combatCell != null)
		{
			combatCell.TakeDamage(value);
			Debug.Log("hit cell");
		}
		else
		{
			board.TakeDamage(value);
			Debug.Log("hit board");
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
		cellData = null;
		image.sprite = slotSprite;
		image.color = slotColor;
	}
}
