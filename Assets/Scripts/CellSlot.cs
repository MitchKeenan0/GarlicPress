using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public CanvasGroup highlightCanvasGroup;
	public ParticleSystem slotDamageParticles;

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
	private Camera cam;
	private int teamID = -1;

	public CombatantCell GetCell() { return combatCell; }
	public CombatantBoard GetBoard() { return board; }

	public int GetTeamID() { return teamID; }
	public void SetTeamID(int value)
	{
		teamID = value;
		CellArsenal myArsenal = GetComponentInChildren<CellArsenal>();
		if (myArsenal != null)
			myArsenal.InitCellArsenal(this);
	}
	

    void Awake()
    {
		image = GetComponent<Image>();
		colorBlinker = GetComponent<ColorBlinker>();
		boardEditor = FindObjectOfType<BoardEditor>();
		cam = Camera.main;
		button = GetComponent<Button>();
		slotSprite = image.sprite;
		slotColor = image.color;
		ShowHighlight(false);
		board = transform.parent.parent.GetComponent<CombatantBoard>();
		SetInteractible(false);
    }

	public void LoadCell(CombatantCell cc, CellData data, bool bEraseCell)
	{
		if (cc == null)
		{
			if (bEraseCell && (combatCell != null))
				combatCell.LoadCellData(null);
		}

		combatCell = cc;
		cellData = data;
		if ((combatCell != null) && (cellData != null))
		{
			combatCell.LoadCellData(cellData);
		}
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
			slotDamageParticles.Play();
		}
	}

	public void SetInteractible(bool value)
	{
		button.interactable = value;
		image.color = value ? Color.white : Color.black;
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
		LoadCell(null, null, bEraseCell);
		image.sprite = slotSprite;
		//image.color = slotColor;
		//ColorBlock cb = button.colors;
		//cb.normalColor = slotColor;
		//button.colors = cb;
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (boardEditor != null)
		{
			boardEditor.UpdateRemovingSlot(this);
		}
		else if ((combatCell != null) && (combatCell.GetCellData() != null))
		{
			CombatCellMover cellMover = combatCell.GetComponent<CombatCellMover>();
			if ((cellMover != null) && (cellMover.SetMoving(true, this)))
			{
				RecordMovingCell(combatCell, combatCell.GetCellData());
				combatCell.ShowCanvasGroup(true);
				ClearCell(false);
			}
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if ((movingCell != null) && (movingCell.GetCellData() != null))
		{
			movingCell.LoadCellData(movingCell.GetCellData());
			combatCell = movingCell;
			LoadCell(movingCell, movingCell.GetCellData(), false);
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

	public List<CellSlot> GetNeighborSlots()
	{
		List<CellSlot> neighborSlots = new List<CellSlot>();
		int myIndex = transform.GetSiblingIndex();
		List<CellSlot> myBoardSlots = new List<CellSlot>();
		myBoardSlots = board.GetSlots();

		/// using raycasts to cardinal directions for neighbor gets
		float slotWidth = 0.6f;
		Vector3 myPosition = transform.position;

		Vector3 northSlotPosition = myPosition + (Vector3.up * slotWidth);
		CheckCardinalNeighbor(northSlotPosition, myBoardSlots, neighborSlots);

		Vector3 eastSlotPosition = myPosition + (Vector3.right * slotWidth);
		CheckCardinalNeighbor(eastSlotPosition, myBoardSlots, neighborSlots);

		Vector3 southSlotPosition = myPosition + (Vector3.down * slotWidth);
		CheckCardinalNeighbor(southSlotPosition, myBoardSlots, neighborSlots);

		Vector3 westSlotPosition = myPosition + (Vector3.left * slotWidth);
		CheckCardinalNeighbor(westSlotPosition, myBoardSlots, neighborSlots);

		return neighborSlots;
	}

	void CheckCardinalNeighbor(Vector3 pos, List<CellSlot> boardSlots, List<CellSlot> addToList)
	{
		RaycastHit[] hits;
		Vector3 origin = cam.transform.position;
		Vector3 direction = (pos - origin) * 1.1f;

		hits = Physics.RaycastAll(origin, direction);
		if (hits.Length > 0)
		{
			for(int i = 0; i < hits.Length; i++)
			{
				RaycastHit hit = hits[i];
				Transform objectHit = hit.transform;
				CellSlot hitSlot = hit.transform.GetComponent<CellSlot>();
				if ((hitSlot != null) && (boardSlots.Contains(hitSlot)))
				{
					addToList.Add(hitSlot);
					break;
				}
			}
		}
	}
}
