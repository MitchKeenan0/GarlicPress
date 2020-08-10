using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public int maxHealth = 5;

	private HealthUI healthUI;
	private int currentHealth = 0;

	public int GetHP() { return currentHealth; }

    void Start()
    {
		currentHealth = maxHealth;
		healthUI = GetComponentInChildren<HealthUI>();
		UpdateHealthUI();
	}

	public void InitHealth(int value)
	{
		maxHealth = value;
		currentHealth = value;
	}

	public virtual void TakeDamage(int damage)
	{
		currentHealth -= damage;
		UpdateHealthUI();
	}

	void UpdateHealthUI()
	{
		if (healthUI != null)
			healthUI.UpdateHealthValueText(currentHealth);
	}
}
