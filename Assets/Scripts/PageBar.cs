using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageBar : MonoBehaviour
{
	public PlayerProfile profile;
	public CellLibrary library;
	public Button battleButton;
	public Button libraryButton;

	private List<Button> allButtons;
	private List<CanvasGroup> allCGs;
	private CanvasGroup battleCG;
	private CanvasGroup libraryCG;

    void Start()
    {
		allButtons = new List<Button>();
		allButtons.Add(battleButton);
		allButtons.Add(libraryButton);
		allCGs = new List<CanvasGroup>();
		battleCG = profile.GetComponent<CanvasGroup>();
		allCGs.Add(battleCG);
		libraryCG = library.GetComponent<CanvasGroup>();
		allCGs.Add(libraryCG);
		IsolateButton(battleButton);
	}

	void SetCanvasGroupEnabled(CanvasGroup cg, bool value)
	{
		cg.alpha = value ? 1f : 0f;
		cg.blocksRaycasts = value;
		cg.interactable = value;
	}

	void IsolateCanvasGroup(CanvasGroup cg)
	{
		foreach (CanvasGroup canvas in allCGs)
		{
			if (canvas != cg)
			{
				SetCanvasGroupEnabled(canvas, false);
			}
		}
	}

	void IsolateButton(Button bt)
	{
		SetButtonColorMultiplier(bt, 1f);
		foreach(Button button in allButtons)
		{
			if (button != bt)
			{
				SetButtonColorMultiplier(button, 0.6f);
			}
		}

		EventSystem.current.SetSelectedGameObject(null);
	}

	void SetButtonColorMultiplier(Button bt, float value)
	{
		ColorBlock colorBlock = bt.colors;
		colorBlock.colorMultiplier = value;
		bt.colors = colorBlock;
	}

	public void BattlePage()
	{
		SetCanvasGroupEnabled(battleCG, true);
		IsolateCanvasGroup(battleCG);
		IsolateButton(battleButton);
	}

	public void LibraryPage()
	{
		SetCanvasGroupEnabled(libraryCG, true);
		IsolateCanvasGroup(libraryCG);
		IsolateButton(libraryButton);
	}
}
