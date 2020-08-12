using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellArsenal : MonoBehaviour
{
	public ParticleSystem attackParticles;
	public ParticleSystem defenceParticles;

    void Start()
    {
        
    }

	public virtual void AttackCell(Transform targetTransform)
	{
		attackParticles.transform.SetParent(targetTransform);
		attackParticles.transform.localPosition = Vector3.zero;
		attackParticles.Play();
	}

	public virtual void DefendCell()
	{
		defenceParticles.Play();
	}
}
