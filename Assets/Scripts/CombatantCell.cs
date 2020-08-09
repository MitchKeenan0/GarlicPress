using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatantCell : MonoBehaviour
{
	private Image cellImage;

    void Awake()
    {
		cellImage = GetComponent<Image>();
    }

    public void LoadCell(CellData cd)
	{
		if (cd != null)
		{
			cellImage.sprite = cd.cellSprite;
		}
	}
}
