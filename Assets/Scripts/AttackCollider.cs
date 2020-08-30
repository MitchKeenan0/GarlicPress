using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
	public Vector3 raycastDirection = Vector3.forward;
	public Vector3 raycastOriginOffset = Vector3.forward * -1;

	private CellArsenal myArsenal = null;
	private bool bUpdating = false;
	public int teamID = 0;

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
			for(int i = 0; i < numHits; i++)
			{
				RaycastHit hit = hits[i];
				CellSlot hitCellSlot = hit.transform.GetComponent<CellSlot>();
				if (hitCellSlot != null)
				{
					if (hitCellSlot.GetTeamID() != teamID)
					{
						myArsenal.AttackColliderHit(hit.transform);
						SetEnabled(false);
						break;
					}
				}
			}
		}
	}

	public void InitAttackCollider(CellArsenal owningArsenal, int teamNumber)
	{
		myArsenal = owningArsenal;
		teamID = teamNumber;
	}

	public void SetEnabled(bool value)
	{
		bUpdating = value;
	}
}
