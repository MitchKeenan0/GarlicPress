using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAttack : MonoBehaviour
{
	public float attackTravelDuration = 0.2f;

	private CellArsenal myArsenal = null;
	private AttackCollider attackCollider = null;

    void Awake()
    {
		attackCollider = GetComponent<AttackCollider>();
		if (!attackCollider)
			attackCollider = GetComponentInChildren<AttackCollider>();
    }

	public void InitCellAttack(CellArsenal owningArsenal, int teamID)
	{
		myArsenal = owningArsenal;
		attackCollider.InitAttackCollider(owningArsenal, teamID);
	} 

    public virtual void StartAttack(Transform slotAttacker, Transform slotDefender)
	{
		attackCollider.SetEnabled(true);
	}

	public virtual void EndAttack()
	{
		attackCollider.SetEnabled(false);
	}
}
