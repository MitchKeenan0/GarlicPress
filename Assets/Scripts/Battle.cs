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
	private Health playerHealth;
	private Health opponentHealth;

	private IEnumerator warmupCoroutine;
	private IEnumerator battleCoroutine;

    void Start()
    {
		battleUI = FindObjectOfType<BattleUI>();
    }

	public void InitBattle(CombatantBoard player, CombatantBoard opponent)
	{
		playerBoard = player;
		playerHealth = playerBoard.GetComponent<Health>();
		opponentBoard = opponent;
		opponentHealth = opponentBoard.GetComponent<Health>();

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

		if (BothCharactersAlive())
		{
			/// start next round
			warmupCoroutine = Warmup();
			StartCoroutine(warmupCoroutine);

			/// grant move tokens
			MoveCounter playerMoveCounter = playerBoard.GetComponentInChildren<MoveCounter>();
			if (playerMoveCounter != null)
				playerMoveCounter.AddMoveToken(1);
			MoveCounter opponentMoveCounter = opponentBoard.GetComponentInChildren<MoveCounter>();
			if (opponentMoveCounter != null)
				opponentMoveCounter.AddMoveToken(1);
		}
	}

	bool BothCharactersAlive()
	{
		bool result = !((playerHealth.GetHP() <= 0) || (opponentHealth.GetHP() <= 0));
		return result;
	}

	void ResolveCellPair(CellSlot cellSlotA, CellSlot cellSlotB)
	{
		cellSlotA.HighlightForDuration(0.5f);
		cellSlotB.HighlightForDuration(0.5f);

		CellData cellA = null;
		CombatantCell combatCellA = null;
		if (cellSlotA.GetCell() != null)
		{
			combatCellA = cellSlotA.GetCell();
			cellA = combatCellA.GetCellData();
		}
		CellData cellB = null;
		CombatantCell combatCellB = null;
		if (cellSlotB.GetCell() != null)
		{
			combatCellB = cellSlotB.GetCell();
			cellB = combatCellB.GetCellData();
		}

		if ((cellA != null) && (cellA.damage > 0))
		{
			combatCellA.GetComponent<CellArsenal>().AttackCell(cellSlotB.transform);
			battleUI.ToastInteraction(cellSlotB.transform.position, cellA.damage);
			cellSlotB.TakeDamage(cellA.damage);
		}

		if ((cellB != null) && (cellB.damage > 0))
		{
			combatCellB.GetComponent<CellArsenal>().AttackCell(cellSlotA.transform);
			battleUI.ToastInteraction(cellSlotA.transform.position, cellB.damage);
			cellSlotA.TakeDamage(cellB.damage);
		}

		if (!BothCharactersAlive())
		{
			StopAllCoroutines();
			bool bPlayerWon = playerHealth.GetHP() > 0;
			battleUI.GameOver(bPlayerWon);
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
