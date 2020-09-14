using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
	public CombatantBoard myBoard = null;
	public ParticleSystem myDamageReceivedParticles = null;

    void Start()
    {
		
	}

	public void TakeHit(int damage)
	{
		if (myBoard != null)
			myBoard.TakeDamage(damage);
		if (myDamageReceivedParticles != null)
			myDamageReceivedParticles.Play();
	}
}
