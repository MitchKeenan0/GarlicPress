using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
	public Text nameText;
	public Image cellImage;
	public Text cellValueText;
	public Text descriptionText;
	public StatPanel damageStatPanel;
	public StatPanel healthStatPanel;
	public StatPanel armourStatPanel;

	private CellLibrary cellLibrary;
	private BoardEditor boardEditor;

    void Start()
    {
		cellLibrary = transform.root.GetComponent<CellLibrary>();
		boardEditor = FindObjectOfType<BoardEditor>();
    }

	public void UpdateInformationCell(CellData cd)
	{
		nameText.text = cd.cellName;
		cellImage.sprite = cd.cellSprite;
		cellValueText.text = cd.cellValue.ToString();
		descriptionText.text = cd.description;
		damageStatPanel.SetStatValue(cd.damage);
		healthStatPanel.SetStatValue(cd.health);
		armourStatPanel.SetStatValue(cd.armour);

		boardEditor.UpdateHotCell(cd);
	}

	public void UseCell()
	{
		cellLibrary.ShowInfo(false, null);
		boardEditor.EnableSlotManipulation(true);
	}

	public void CloseInformationPanel()
	{
		cellLibrary.ShowInfo(false, null);
	}
}
