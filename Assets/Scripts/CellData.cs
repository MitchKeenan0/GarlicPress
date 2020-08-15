using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData : MonoBehaviour
{
	public string cellName = "unnamed cell";
	public Sprite cellSprite;
	public int damage = 1;
	public int health = 1;
	public int armour = 0;
	public int cellValue = 1;
	public string description = "this cell f_cks";
	public bool bAttackAbility = false;
	public bool bRoundEndAbility = false;
	public bool bOnMoveAbility = false;
	public bool bOnCellDiedAbility = false;
	public int saveID = 0;
	

    void Start()
    {
		
    }

	//-- Abilities
	public virtual void AttackAbility(CellSlot targetSlot)
	{

	}

	public virtual void RoundEndAbility(CombatantCell myCell)
	{

	}

	public virtual void OnMoveAbility(CellSlot mySlot)
	{

	}

	public virtual void OnCellDiedAbility(CombatantCell myCell)
	{

	}
}
