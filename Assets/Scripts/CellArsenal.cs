using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArsenal : MonoBehaviour
{
	public ParticleSystem attackParticles;
	public ParticleSystem defenceParticles;

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

	public virtual void DefendCell()
	{
		defenceParticles.Play();
	}

	public virtual void Ability(ParticleSystem ps)
	{

	}

	private IEnumerator AttackDuration(float duration, Transform attackerTransform, Transform targetTransform, int damage)
	{
		yield return new WaitForSeconds(duration);

		attackParticles.transform.SetParent(targetTransform);
		attackParticles.transform.localPosition = Vector3.zero;
		int particleDamageScale = Mathf.Clamp(damage, 1, 20);
		attackParticles.transform.localScale = Vector3.one * particleDamageScale;
		attackParticles.Play();

		CellSlot receivingCellSlot = targetTransform.GetComponent<CellSlot>();
		if (receivingCellSlot != null)
			receivingCellSlot.TakeDamage(damage);

		battleUI.ToastInteraction(receivingCellSlot.transform.position, damage, 0, "-");

		battle.TestBattleOver();
	}
}
