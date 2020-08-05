using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
	public Text nameText;
	public Image cellImage;
	public Text descriptionText;

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
		descriptionText.text = cd.description;
	}

	public void UseCell()
	{
		cellLibrary.ShowInfo(false);
		boardEditor.EnableSlotManipulation(true);
	}

	public void CloseInformationPanel()
	{
		cellLibrary.ShowInfo(false);
	}
}
