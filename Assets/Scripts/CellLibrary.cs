using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellLibrary : MonoBehaviour
{
	public Transform libraryTransform;
	public Transform informationTransform;
	public CellOption optionPrefab;
	public CellData[] allCells;

	private CanvasGroup libraryCanvasGroup;
	private CanvasGroup informationCanvasGroup;
	private InformationPanel informationPanel;
	private BoardEditor boardEditor;

	void Start()
    {
		if (FindObjectOfType<BoardEditor>() != null)
		{
			libraryCanvasGroup = libraryTransform.GetComponent<CanvasGroup>();
			informationCanvasGroup = informationTransform.GetComponent<CanvasGroup>();
			informationPanel = GetComponentInChildren<InformationPanel>();
			boardEditor = GetComponent<BoardEditor>();
			EnableInformation(false);

			int numCells = allCells.Length;
			for (int i = 0; i < numCells; i++)
			{
				CellOption cellOption = Instantiate(optionPrefab, libraryTransform);
				if ((allCells.Length > i) && (allCells[i] != null))
				{
					CellData cellData = allCells[i];
					cellOption.LoadCell(cellData);
				}
			}
		}
    }

	public void ShowInfo(bool value, CellData cd)
	{
		EnableInformation(value);
		if (cd != null)
			informationPanel.UpdateInformationCell(cd);
		if (value)
			boardEditor.CancelBoardEdit();
	}

	void EnableLibrary(bool value)
	{
		libraryCanvasGroup.alpha = value ? 1f : 0f;
		libraryCanvasGroup.blocksRaycasts = value;
		libraryCanvasGroup.interactable = value;
	}

	void EnableInformation(bool value)
	{
		informationCanvasGroup.alpha = value ? 1f : 0f;
		informationCanvasGroup.blocksRaycasts = value;
		informationCanvasGroup.interactable = value;
	}
}
