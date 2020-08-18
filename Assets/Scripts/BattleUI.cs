using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleUI : MonoBehaviour
{
	public GameOverPanel gameOverPanel;
	public InteractionToast interactionTextPrefab;
	public Sprite[] interactionSpriteArray;
	public Color[] interactionColorArray;
	public int toastPoolStartSize = 3;

	private CanvasGroup gameOverPanelCanvasGroup;
	private List<InteractionToast> interactionToastList;

    void Start()
    {
		gameOverPanelCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
		SetGameOverCanvasGroup(false);
		interactionToastList = new List<InteractionToast>();
        for(int i = 0; i < toastPoolStartSize; i++)
		{
			SpawnInteractionToast();
		}
    }

	/// interaction types	0-damage	1-modifyStats
	public void ToastInteraction(Vector3 screenPosition, float value, int interactionType, string prefix)
	{
		InteractionToast toast = GetInteractionToast();
		if (toast == null)
		{
			toast = SpawnInteractionToast();
		}

		Sprite interationSprite = interactionSpriteArray[interactionType];
		Color interactionColor = interactionColorArray[interactionType];
		toast.SetInteractionDetails(interationSprite, prefix + value.ToString("F0"), interactionColor);
		toast.transform.position = screenPosition;
		toast.SetToastActive(true);
	}

	InteractionToast GetInteractionToast()
	{
		InteractionToast it = null;
		foreach(InteractionToast toast in interactionToastList)
		{
			if (!toast.IsActive())
			{
				it = toast;
				break;
			}
		}
		return it;
	}

	InteractionToast SpawnInteractionToast()
	{
		InteractionToast it = Instantiate(interactionTextPrefab, transform);
		it.SetToastActive(false);
		interactionToastList.Add(it);
		return it;
	}

	public void GameOver(bool bPlayerWon)
	{
		SetGameOverCanvasGroup(true);
		gameOverPanel.SetConclusionText(bPlayerWon);
		Time.timeScale = 0.2f;
	}

	void SetGameOverCanvasGroup(bool value)
	{
		gameOverPanelCanvasGroup.alpha = value ? 1f : 0f;
		gameOverPanelCanvasGroup.interactable = value;
		gameOverPanelCanvasGroup.blocksRaycasts = value;
	}

	public void ReturnHome()
	{
		SceneManager.LoadScene("HomeScene");
	}
}
