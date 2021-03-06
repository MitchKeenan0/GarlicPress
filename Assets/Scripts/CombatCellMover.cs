﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCellMover : MonoBehaviour
{
	private RectTransform myRectTransform;
	private RectTransform parentRectTransform;
	private RectTransform moveFieldTransform;
	private MoveCounter moveCounter;
	private Camera cameraMain;
	private CellSlot cellSlot;
	private CellSlot moveOriginSlot;
	private CombatantCell combatCell;
	private CellData movingCellData;
	private bool bMoveable = false;
	private bool bMoving = false;

	void Start()
	{
		myRectTransform = GetComponent<RectTransform>();
		parentRectTransform = transform.parent.GetComponent<RectTransform>();
		if (FindObjectOfType<MoveField>() != null)
			moveFieldTransform = FindObjectOfType<MoveField>().GetComponent<RectTransform>();
		cameraMain = Camera.main;
		cellSlot = transform.GetComponentInParent<CellSlot>();
		combatCell = GetComponent<CombatantCell>();
	}
    
    void Update()
    {
        if (bMoving && (Input.touchCount > 0))
		{
			Touch touch = Input.GetTouch(0);
			Vector3 movePosition = cameraMain.ScreenToWorldPoint(touch.position);
			movePosition.z = 0;
			transform.position = movePosition;
		}
    }

	public void FinishMoveToSlot()
	{
		CombatantBoard myBoard = parentRectTransform.parent.GetComponentInParent<CombatantBoard>();
		if (myBoard != null)
		{
			bool bGoodMove = false;
			Transform closestTransform = myBoard.GetClosestSlotTo(transform.position);
			if (closestTransform != null)
			{
				CellSlot closestSlot = closestTransform.GetComponent<CellSlot>();
				if (closestSlot != cellSlot)
				{
					transform.SetParent(closestTransform);
					transform.localPosition = Vector3.zero;
					transform.localScale = Vector3.one;

					//if ((moveOriginSlot != null) && (moveOriginSlot.GetCell() != null))
					//{
					//	CellData cellData = moveOriginSlot.GetCell().GetCellData();
					//	if (cellData != null)
					//		combatCell.LoadCellData(cellData);
					//}

					bGoodMove = true;
					cellSlot = closestSlot;
					cellSlot.LoadCell(combatCell, movingCellData, false);
					combatCell.SetSlot(cellSlot);

					/// On Move Ability
					if ((combatCell != null) && (combatCell.GetCellData() != null)
						&& (combatCell.GetCellData().bOnMoveAbility))
					{
						combatCell.GetCellData().OnMoveAbility(cellSlot);
					}
				}
				else
				{
					/// move ended up at origin
					transform.SetParent(cellSlot.transform);
					transform.localPosition = Vector3.zero;
					transform.localScale = Vector3.one;
				}
			}

			if (bGoodMove)
			{
				if (moveCounter != null)
					moveCounter.SpendMoveToken(1);

				moveOriginSlot.ClearCell(false);
			}
		}
	}

	public void SetMoveable(bool value, MoveCounter counter)
	{
		bMoveable = value;
		moveCounter = counter;
	}

	public bool SetMoving(bool value, CellSlot originSlot)
	{
		bool bMoveStarted = false;
		if (bMoveable)
		{
			if (value)
			{
				bMoving = true;
				transform.SetParent(moveFieldTransform);
				transform.localScale = Vector3.one;
				bMoveStarted = true;
				moveOriginSlot = originSlot;
				movingCellData = originSlot.GetCell().GetCellData();
			}
			else
			{
				bMoving = false;
				transform.SetParent(parentRectTransform);
				transform.localScale = Vector3.one;
			}
		}
		return bMoveStarted;
	}
}
