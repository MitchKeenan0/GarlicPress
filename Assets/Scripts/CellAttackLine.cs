using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAttackLine : CellAttack
{
	public float lineTailLength = 0.5f;

	private LineRenderer lineRender = null;
	private CellSlot mySlot = null;
	private CellSlot opponentSlot = null;
	private Vector3 lineTravelPosition = Vector3.zero;
	private Vector3 lineTailPosition = Vector3.zero;
	private float attackDuration = 0f;
	private float tailDuration = 0f;
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

		attackCoroutine = DurationLineAttack();
		StartCoroutine(attackCoroutine);
	}

	public override void EndAttack()
	{
		base.EndAttack();

		lineRender.SetPosition(0, mySlot.transform.position);
		lineRender.SetPosition(1, mySlot.transform.position);
		lineRender.enabled = false;
	}

	private IEnumerator DurationLineAttack()
	{
		Vector3 myPos = mySlot.transform.position;
		Vector3 opPos = opponentSlot.transform.position;
		lineRender.SetPosition(0, myPos);
		lineRender.SetPosition(1, myPos);
		lineRender.enabled = true;
		lineTravelPosition = myPos;
		lineTailPosition = myPos;
		attackDuration = 0f;
		tailDuration = 0f;

		while (true)
		{
			float deltaT = Time.deltaTime;
			attackDuration += deltaT;

			/// line head &..
			lineTravelPosition = Vector3.Lerp(myPos, opPos, attackDuration * (1f / attackTravelDuration));
			lineRender.SetPosition(1, lineTravelPosition);

			/// ..tail starts halfway to full duration
			if ((attackDuration / attackTravelDuration) > lineTailLength)
			{
				tailDuration += deltaT;
				lineTailPosition = Vector3.Lerp(myPos, opPos, tailDuration * (1f / attackTravelDuration));
				lineRender.SetPosition(0, lineTailPosition);
			}

			if (attackDuration > attackTravelDuration)
				break;

			yield return new WaitForSeconds(deltaT);
		}

		this.EndAttack();
	}
}
