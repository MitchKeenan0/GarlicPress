using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellOption : MonoBehaviour
{
	private Image cellImage;
	private CellLibrary cellLibrary;
	private CellData cellData;

    void Awake()
    {
		cellLibrary = FindObjectOfType<CellLibrary>();
		cellImage = GetComponent<Image>();
    }

	public void LoadCell(CellData cd)
	{
		cellData = cd;
		if (cellImage == null)
			cellImage = GetComponent<Image>();
		cellImage.sprite = cd.cellSprite;
	}

	public void CellSelected()
	{
		cellLibrary.ShowInfo(true, cellData);
	}
}
