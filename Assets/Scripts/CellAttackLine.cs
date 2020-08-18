using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAttackLine : CellAttack
{
	private LineRenderer lineRender = null;
	private CellSlot mySlot = null;
	private CellSlot opponentSlot = null;
	private Vector3 lineTravelPosition = Vector3.zero;
	private float attackDuration = 0f;
	private IEnumerator attackCoroutine;

    void Start()
    {
		lineRender = GetComponent<LineRenderer>();
		lineRender.enabled = false;
    }

	public override void StartAttack(Transform attacker, Transform defender)
	{
		base.StartAttack(attacker, defender);

		mySlot = attacker.GetComponent<CellSlot>();
		opponentSlot = defender.GetComponent<CellSlot>();

		attackCoroutine = AttackDuration();
		StartCoroutine(attackCoroutine);
	}

	public override void EndAttack()
	{
		base.EndAttack();

		lineRender.SetPosition(0, mySlot.transform.position);
		lineRender.enabled = false;
	}

	private IEnumerator AttackDuration()
	{
		Vector3 myPos = mySlot.transform.position;
		Vector3 opPos = opponentSlot.transform.position;
		lineRender.SetPosition(0, myPos);
		lineRender.SetPosition(1, myPos);
		lineRender.enabled = true;
		lineTravelPosition = myPos;
		attackDuration = 0f;

		while (true)
		{
			attackDuration += Time.deltaTime;
			lineTravelPosition = Vector3.Lerp(myPos, opPos, attackDuration * (1f / attackTravelDuration));

			lineRender.SetPosition(1, lineTravelPosition);

			if (attackDuration > attackTravelDuration)
				break;

			yield return new WaitForSeconds(Time.deltaTime);
		}

		this.EndAttack();
	}
}
