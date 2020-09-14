using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
	public Vector3 raycastDirection = Vector3.forward;
	public Vector3 raycastOriginOffset = Vector3.forward * -1;

	private CellArsenal myArsenal = null;
	private Vector3 previousOrigin = Vector3.zero;
	private bool bUpdating = false;
	private int teamID = -1;
	private int damage = -1;

	void Awake()
    {
		SetEnabled(false);
    }

	void Update()
	{
		if (bUpdating)
		{
			RaycastForHit();
		}
	}

	void RaycastForHit()
	{
		Vector3 rayOrigin = transform.position + raycastOriginOffset;
		Vector3 rayDirection = rayOrigin + raycastDirection;
		RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection);
		int numHits = hits.Length;
		if (numHits > 0)
		{
			for (int i = 0; i < numHits; i++)
			{
				RaycastHit hit = hits[i];
				if (CheckHitForCell(hit))
					break;
			}
		}

		/// second raycast to check for fast movement gaps
		Vector3 deltaPosition = rayOrigin - previousOrigin;
		Vector3 deltaDirection = deltaPosition = raycastDirection;
		RaycastHit[] deltaHits = Physics.RaycastAll(deltaPosition, deltaDirection);
		int numDeltaHits = deltaHits.Length;
		if (numDeltaHits > 0)
		{
			for (int i = 0; i < numDeltaHits; i++)
			{
				RaycastHit hit = hits[i];
				if (CheckHitForCell(hit))
					break;
			}
		}

		previousOrigin = rayOrigin;
	}

	bool CheckHitForCell(RaycastHit hit)
	{
		bool bHitCell = false;
		CombatantCell hitCombatCell = hit.transform.GetComponentInChildren<CombatantCell>();
		if (hitCombatCell != null)
		{
			if (hitCombatCell.GetTeamID() != teamID)
			{
				if (myArsenal != null)
				{
					myArsenal.AttackColliderHit(hit.transform);
					SetEnabled(false);
					bHitCell = true;
				}
			}
		}
		CharacterCollider hitCharacter = hit.transform.GetComponent<CharacterCollider>();
		if (!hitCharacter)
			hitCharacter = hit.transform.GetComponentInChildren<CharacterCollider>();
		if (hitCharacter != null)
		{
			hitCharacter.TakeHit(damage);
			myArsenal.AttackColliderHit(hit.transform);
			SetEnabled(false);
			bHitCell = true;
		}
		return bHitCell;
	}

	public void InitAttackCollider(CellArsenal owningArsenal, int teamNumber, int dmg)
	{
		myArsenal = owningArsenal;
		teamID = teamNumber;
		damage = dmg;
	}

	public void SetEnabled(bool value)
	{
		bUpdating = value;
		previousOrigin = transform.position + raycastOriginOffset;
	}
}
