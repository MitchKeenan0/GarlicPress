using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellLibrary : MonoBehaviour
{
	public Transform libraryTransform;
	public Transform informationTransform;
	public GameObject optionPrefab;

	private CanvasGroup libraryCanvasGroup;
	private CanvasGroup informationCanvasGroup;

	void Start()
    {
		libraryCanvasGroup = libraryTransform.GetComponent<CanvasGroup>();
		informationCanvasGroup = informationTransform.GetComponent<CanvasGroup>();
		EnableInformation(false);

		int rando = Random.Range(3, 7);
		for (int i = 0; i < rando; i++)
		{
			GameObject gob = Instantiate(optionPrefab, libraryTransform);
		}
    }

	public void ShowInfo(bool value)
	{
		EnableInformation(value);
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
