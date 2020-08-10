using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantCell : MonoBehaviour
{
	private Image cellImage;
	private Health health;

    void Awake()
    {
		cellImage = GetComponent<Image>();
		health = GetComponent<Health>();
    }

    public void LoadCell(CellData cd)
	{
		if (cd != null)
		{
			cellImage.sprite = cd.cellSprite;
			health.InitHealth(cd.health);
		}
	}

	public void TakeDamage(int value)
	{
		health.TakeDamage(value);
		if (health.GetHP() <= 0)
			CellDied();
	}

	void CellDied()
	{
		cellImage.color = Color.black;
		CellSlot mySlot = transform.parent.GetComponent<CellSlot>();
		mySlot.LoadCombatCell(null);
		mySlot.LoadCell(null);
		mySlot.ClearCell();
		Destroy(gameObject, 0.2f);
	}
}
