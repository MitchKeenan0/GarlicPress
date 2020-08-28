using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantCell : MonoBehaviour
{
	private Image cellImage;
	private Health health;
	private CellData cellData;
	private CellSlot mySlot;
	private CanvasGroup canvasGroup;
	private CellArsenal arsenal;

	private int m_Damage = 0;
	private int m_Health = 0;
	private int m_Armour = 0;

	public CellData GetCellData() { return cellData; }
	public int GetDamage() { return m_Damage; }
	public int GetHealth() { return m_Health; }
	public int GetArmour() { return m_Armour; }

	public CellSlot GetSlot() { return mySlot; }
	public void SetSlot(CellSlot value) { mySlot = value; }

	void Awake()
    {
		cellImage = GetComponent<Image>();
		health = GetComponent<Health>();
		canvasGroup = GetComponent<CanvasGroup>();
		arsenal = GetComponent<CellArsenal>();
		ShowCanvasGroup(false);
		mySlot = transform.parent.GetComponent<CellSlot>();
	}

    public void LoadCellData(CellData cd)
	{
		cellData = cd;
		if (cellData != null)
		{
			m_Damage = cellData.damage;
			m_Health = cellData.health;
			m_Armour = cellData.armour;
			health.InitHealth(m_Health);
			cellImage.sprite = cellData.cellSprite;
			ShowCanvasGroup(true);
		}
		else
		{
			cellImage.sprite = null;
			ShowCanvasGroup(false);
		}
	}

	public void TakeDamage(int value)
	{
		int damageMinusArmour = value;
		if ((cellData != null) && (cellData.armour != 0))
		{
			damageMinusArmour -= cellData.armour;
			arsenal.DefendCell(cellData.armour);
		}
		health.TakeDamage(damageMinusArmour);
		if (health.GetHP() <= 0)
			CellDied();
	}

	public void ShowCanvasGroup(bool value)
	{
		canvasGroup.alpha = value ? 1f : 0f;
		canvasGroup.interactable = value;
		canvasGroup.blocksRaycasts = value;
	}

	//-- Stat affects
	public virtual void ModifyDamage(int value)
	{
		m_Damage += value;
	}
	public void SetDamage(int value) { m_Damage = value; }

	public virtual void ModifyHealth(int value)
	{
		m_Health += value;
		if (m_Health > health.maxHealth)
			health.InitHealth(m_Health);
	}
	public void SetHealth(int value)
	{
		m_Health = value;
		health.InitHealth(m_Health);
	}

	public virtual void ModifyArmour(int value)
	{
		m_Armour += value;
	}

	public void CellDied()
	{
		if (GetCellData() != null)
		{
			if (GetCellData().bOnCellDiedAbility)
			{
				cellData.OnCellDiedAbility(this);
			}
			else
			{
				mySlot.ClearCell(true);
				ShowCanvasGroup(false);
				Destroy(gameObject, 0.5f);
			}
		}
	}
}
