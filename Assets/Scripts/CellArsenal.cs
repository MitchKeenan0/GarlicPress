using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArsenal : MonoBehaviour
{
	public ParticleSystem attackParticles;
	public ParticleSystem defenceParticles;
	public float hitDelay = 0.1f;

	private CellAttack cellAttack = null;
	private BattleUI battleUI = null;
	private Battle battle = null;
	private IEnumerator attackCoroutine;

    void Start()
    {
		cellAttack = GetComponentInChildren<CellAttack>();
		battleUI = FindObjectOfType<BattleUI>();
		battle = FindObjectOfType<Battle>();
    }

	public virtual void AttackCell(Transform attackerTransform, Transform targetTransform, int damage)
	{
		if (cellAttack != null)
		{
			cellAttack.StartAttack(attackerTransform, targetTransform);
			attackCoroutine = AttackDuration(cellAttack.attackTravelDuration, attackerTransform, targetTransform, damage);
			StartCoroutine(attackCoroutine);
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

	public virtual void Ability(ParticleSystem ps)
	{

	}

	private IEnumerator AttackDuration(float duration, Transform attackerTransform, Transform targetTransform, int damage)
	{
		yield return new WaitForSeconds(duration + hitDelay);

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
				CellData cellData = targetCell.GetCellData();
				if ((cellData != null) && cellData.bOnAttackedAbility)
				{
					CellSlot slotA = attackerTransform.GetComponent<CellSlot>();
					CellSlot slotB = targetTransform.GetComponent<CellSlot>();
					bAttackThrough = cellData.OnAttackedAbility(slotB, slotA);
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
}
