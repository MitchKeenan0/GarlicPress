using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public int maxHealth = 5;
	public int initScalar = 1;

	private HealthUI healthUI = null;
	private int currentHealth = 0;

	public int GetHP() { return currentHealth; }

    void Start()
    {
		currentHealth = maxHealth;
		healthUI = GetComponentInChildren<HealthUI>();
	}

	public void InitHealth(int value)
	{
		maxHealth = value * initScalar;
		currentHealth = maxHealth;
		UpdateHealthUI();
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		UpdateHealthUI();
	}

	void UpdateHealthUI()
	{
		if (!healthUI)
			healthUI = GetComponentInChildren<HealthUI>();
		if (healthUI != null)
			healthUI.UpdateHealthValueText(currentHealth);
	}
}
