using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellOption : MonoBehaviour
{
	public Image cellImage;

	private Text cellValueText;
	private CellLibrary cellLibrary;
	private CellData cellData;

	public CellData GetCellData() { return cellData; }

    void Awake()
    {
		cellLibrary = FindObjectOfType<CellLibrary>();
		cellValueText = GetComponentInChildren<Text>();
    }

	public void LoadCell(CellData cd)
	{
		cellData = cd;
		if (cellImage == null)
			cellImage = GetComponent<Image>();
		cellImage.sprite = cd.cellSprite;
		if (cellValueText == null)
			cellValueText = GetComponentInChildren<Text>();
		cellValueText.text = cd.cellValue.ToString();
	}

	public void CellSelected()
	{
		cellLibrary.ShowInfo(true, cellData);
	}
}
