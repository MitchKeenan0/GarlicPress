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
	private float attackTime = 0f;
	private float tailDuration = 0f;
	private IEnumerator attackCoroutine;
	private bool bUpdating = false;
	private Vector3 myPos = Vector3.zero;
	private Vector3 opPos = Vector3.zero;
	private Transform attackHeadTransfom = null;
	private ParticleSystem attackHeadParticles = null;

	void Start()
    {
		lineRender = GetComponent<LineRenderer>();
		lineRender.enabled = false;
		bUpdating = false;
		attackHeadTransfom = GetComponentInChildren<Transform>();
		attackHeadParticles = attackHeadTransfom.GetComponent<ParticleSystem>();
		if (!attackHeadParticles)
			attackHeadParticles = GetComponentInChildren<ParticleSystem>();
		if (attackHeadParticles != null)
		{
			var em = attackHeadParticles.emission;
			em.enabled = false;
		}
	}

	void Update()
	{
		if (bUpdating)
		{
			float deltaT = Time.deltaTime;
			attackTime += deltaT;

			/// line head &..
			lineTravelPosition = Vector3.Lerp(myPos, opPos, attackTime * (1f / attackTravelDuration));
			lineRender.SetPosition(1, lineTravelPosition);
			attackHeadTransfom.position = lineTravelPosition;

			/// ..tail starts halfway to full duration
			if ((attackTime / attackTravelDuration) > lineTailLength)
			{
				tailDuration += deltaT;
				lineTailPosition = Vector3.Lerp(myPos, opPos, tailDuration * (1f / attackTravelDuration));
				lineRender.SetPosition(0, lineTailPosition);
			}

			if (attackTime > attackTravelDuration)
			{
				bUpdating = false;
				this.EndAttack();
			}
		}
	}

	public override void StartAttack(Transform attacker, Transform defender)
	{
		base.StartAttack(attacker, defender);

		mySlot = attacker.GetComponent<CellSlot>();
		opponentSlot = defender.GetComponent<CellSlot>();

		StartLineAttack();
	}

	public override void EndAttack()
	{
		base.EndAttack();

		if (lineRender != null)
		{
			lineRender.SetPosition(0, mySlot.transform.position);
			lineRender.SetPosition(1, mySlot.transform.position);
			lineRender.enabled = false;
		}

		if (attackHeadParticles != null)
		{
			var em = attackHeadParticles.emission;
			em.enabled = false;
		}

		bUpdating = false;
	}

	private void StartLineAttack()
	{
		myPos = mySlot.transform.position;
		opPos = opponentSlot.transform.position;
		lineTravelPosition = myPos;
		lineTailPosition = myPos;
		attackTime = 0f;
		tailDuration = 0f;

		if (lineRender != null)
		{
			lineRender.SetPosition(0, myPos);
			lineRender.SetPosition(1, myPos);
			lineRender.enabled = true;
		}
		
		if (attackHeadTransfom != null)
		{
			attackHeadTransfom.position = myPos;
			if (attackHeadParticles != null)
			{
				var em = attackHeadParticles.emission;
				em.enabled = true;
			}
		}

		bUpdating = true;
	}
}
