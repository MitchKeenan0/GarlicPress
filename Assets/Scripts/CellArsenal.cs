using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArsenal : MonoBehaviour
{
	public ParticleSystem attackParticles;
	public ParticleSystem defenceParticles;
	public float hitDelay = 0.1f;

	private CellAttack cellAttack = null;
	private CellSlot mySlot = null;
	private BattleUI battleUI = null;
	private Battle battle = null;
	private Transform mySlotTransform = null;
	private int myDamage = 0;
	private IEnumerator hitDelayCoroutine;

    void Start()
    {
		cellAttack = GetComponentInChildren<CellAttack>();
		mySlot = transform.GetComponentInParent<CellSlot>();
		battleUI = FindObjectOfType<BattleUI>();
		battle = FindObjectOfType<Battle>();
    }

	public void InitCellArsenal(CellSlot slot)
	{
		mySlot = slot;
		cellAttack.InitCellAttack(this, mySlot.GetTeamID());
	}

	public virtual void StartHitAfterDelay(Transform attackerTransform, Transform targetTransform, int damage)
	{
		if (cellAttack != null)
		{
			mySlotTransform = attackerTransform;
			myDamage = damage;
			cellAttack.StartAttack(attackerTransform, targetTransform);
		}
	}

	public virtual void DefendCell(int defenceValue)
	{
		if (defenceParticles != null)
		{
			defenceParticles.Play();
			battleUI.ToastInteraction(transform.position, defenceValue, 1, "");
		}
	}

	public virtual void AttackColliderHit(Transform hitTransform)
	{
		hitDelayCoroutine = HitAfterDelay(hitDelay, mySlotTransform, hitTransform, myDamage);
		StartCoroutine(hitDelayCoroutine);
		cellAttack.EndAttack();
	}

	private void HitCell(Transform attackerTransform, Transform targetTransform, int damage)
	{
		attackParticles.transform.SetParent(targetTransform);
		attackParticles.transform.localPosition = Vector3.zero;
		int particleDamageScale = Mathf.Clamp(damage, 1, 20);
		attackParticles.transform.localScale = Vector3.one * particleDamageScale;
		attackParticles.Play();

		bool bAttackThrough = true;

		CellSlot targetCellSlot = targetTransform.GetComponent<CellSlot>();
		if (targetCellSlot != null)
		{
			CombatantCell targetCell = targetCellSlot.GetCell();
			if (targetCell != null)
			{
				CellData targetCellData = targetCell.GetCellData();
				if ((targetCellData != null) && targetCellData.bOnAttackedAbility)
				{
					CellSlot slotA = attackerTransform.GetComponent<CellSlot>();
					CellSlot slotB = targetTransform.GetComponent<CellSlot>();
					bAttackThrough = targetCellData.OnAttackedAbility(slotB, slotA);
				}
			}

			if (bAttackThrough)
				targetCellSlot.TakeDamage(damage);
		}

		if (bAttackThrough)
		{
			battleUI.ToastInteraction(targetCellSlot.transform.position, damage, 0, "--");
			battle.TestBattleOver();
		}
	}

	private IEnumerator HitAfterDelay(float duration, Transform attackerTransform, Transform targetTransform, int damage)
	{
		yield return new WaitForSeconds(duration);
		HitCell(attackerTransform, targetTransform, damage);
	}
}
