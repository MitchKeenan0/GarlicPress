using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
	public InteractionToast interactionTextPrefab;
	public int toastPoolStartSize = 3;

	private List<InteractionToast> interactionToastList;

    void Start()
    {
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
}
