using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
	public CanvasGroup warmupCanvasGroup;
	public float warmupTime = 1f;
	public float cellResolveTime = 0.3f;

	private CombatantBoard playerBoard;
	private CombatantBoard opponentBoard;
	private BattleUI battleUI;

	private IEnumerator warmupCoroutine;
	private IEnumerator battleCoroutine;

    void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();
    }

	public void InitBattle(CombatantBoard player, CombatantBoard opponent)
	{
		playerBoard = player;
		opponentBoard = opponent;

		warmupCoroutine = Warmup();
		StartCoroutine(warmupCoroutine);
	}

	private IEnumerator ResolveCells()
	{
		int cellCount = playerBoard.GetSlots().Count;
		for(int i = 0; i < cellCount; i++)
		{
			CellSlot playerSlot = playerBoard.GetSlots()[i];
			CellSlot opponentSlot = opponentBoard.GetSlots()[i];
			ResolveCellPair(playerSlot, opponentSlot);
			yield return new WaitForSeconds(cellResolveTime);
		}
		Debug.Log("GG");
	}

	void ResolveCellPair(CellSlot cellSlotA, CellSlot cellSlotB)
	{
		cellSlotA.SetBlinking(true);
		cellSlotB.SetBlinking(true);

		CellData cellA = cellSlotA.GetCell();
		CellData cellB = cellSlotB.GetCell();

		if ((cellB != null) && (cellA == null))
		{
			battleUI.ToastInteraction(cellSlotA.transform.position, cellB.damage);
		}
		else if ((cellA != null) && (cellB == null))
		{
			battleUI.ToastInteraction(cellSlotB.transform.position, cellA.damage);
		}
		else if ((cellA != null) && (cellB != null))
		{
			battleUI.ToastInteraction(cellSlotA.transform.position, cellB.damage);
			battleUI.ToastInteraction(cellSlotB.transform.position, cellA.damage);
		}
	}

	private IEnumerator Warmup()
	{
		ShowWarmup(true);
		yield return new WaitForSeconds(warmupTime);
		ShowWarmup(false);
		battleCoroutine = ResolveCells();
		StartCoroutine(battleCoroutine);
	}

	void ShowWarmup(bool value)
	{
		warmupCanvasGroup.alpha = value ? 1f : 0f;
		warmupCanvasGroup.blocksRaycasts = value;
		warmupCanvasGroup.interactable = value;
	}
}
