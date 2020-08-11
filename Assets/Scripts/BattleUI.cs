using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleUI : MonoBehaviour
{
	public GameOverPanel gameOverPanel;
	public InteractionToast interactionTextPrefab;
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

	public void ToastInteraction(Vector3 screenPosition, float value)
	{
		InteractionToast toast = GetInteractionToast();
		if (toast == null)
		{
			SpawnInteractionToast();
			toast = GetInteractionToast();
		}
		toast.SetInteractionDetails(null, value.ToString("F0"));
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

	void SpawnInteractionToast()
	{
		InteractionToast it = Instantiate(interactionTextPrefab, transform);
		it.SetToastActive(false);
		interactionToastList.Add(it);
	}

	public void GameOver(bool bPlayerWon)
	{
		SetGameOverCanvasGroup(true);
		gameOverPanel.SetConclusionText(bPlayerWon);
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
