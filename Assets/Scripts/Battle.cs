﻿using System.Collections;
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

			/// grant Move tokens
			MoveCounter playerMoveCounter = playerBoard.GetComponentInChildren<MoveCounter>();
			if (playerMoveCounter != null)
				playerMoveCounter.AddMoveToken(1);
			MoveCounter opponentMoveCounter = opponentBoard.GetComponentInChildren<MoveCounter>();
			if (opponentMoveCounter != null)
				opponentMoveCounter.AddMoveToken(1);

			/// end-of-round Abilities
			CombatantCell[] playerCells = playerBoard.GetComponentsInChildren<CombatantCell>();
			foreach(CombatantCell pc in playerCells)
			{
				if ((pc.GetCellData() != null) && (pc.GetCellData().bRoundEndAbility))
					pc.GetCellData().RoundEndAbility(pc);
			}
			CombatantCell[] opponentCells = opponentBoard.GetComponentsInChildren<CombatantCell>();
			foreach (CombatantCell oc in opponentCells)
			{
				if ((oc.GetCellData() != null) && (oc.GetCellData().bRoundEndAbility))
					oc.GetCellData().RoundEndAbility(oc);
			}
		}
		else
		{
			StopAllCoroutines();
			bool bPlayerWon = (playerHealth.GetHP() >= 1) && (playerBoard.GetNumCells() > 0);
			battleUI.GameOver(bPlayerWon);
		}
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

		if (cellA != null)
		{
			int cellADamage = combatCellA.GetDamage();
			if (cellADamage > 0)
			{
				combatCellA.GetComponent<CellArsenal>().AttackCell(cellSlotB.transform);
				battleUI.ToastInteraction(cellSlotB.transform.position, cellADamage);
				cellSlotB.TakeDamage(cellADamage);
			}

			if (cellA.bAttackAbility)
				cellA.AttackAbility(cellSlotB);
		}

		if (cellB != null)
		{
			int cellBDamage = combatCellB.GetDamage();
			if (cellBDamage > 0)
			{
				combatCellB.GetComponent<CellArsenal>().AttackCell(cellSlotA.transform);
				battleUI.ToastInteraction(cellSlotA.transform.position, cellBDamage);
				cellSlotA.TakeDamage(cellBDamage);
			}

			if (cellB.bAttackAbility)
				cellB.AttackAbility(cellSlotA);
		}

		if (!BothCharactersAlive())
		{
			StopAllCoroutines();
			bool bPlayerWon = (playerHealth.GetHP() >= 1) && (playerBoard.GetNumCells() > 0);
			battleUI.GameOver(bPlayerWon);
		}
	}

	bool BothCharactersAlive()
	{
		bool alive = (playerHealth.GetHP() >= 1) && (opponentHealth.GetHP() >= 1);
		bool well = (playerBoard.GetNumCells() > 0) && (opponentBoard.GetNumCells() > 0);
		return (alive && well);
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
