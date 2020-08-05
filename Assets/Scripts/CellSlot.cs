using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSlot : MonoBehaviour
{
	private Image image;
	private BoardEditor boardEditor;

    void Start()
    {
		image = GetComponent<Image>();
		boardEditor = transform.root.GetComponent<BoardEditor>();
    }

	public void SetMaterial(Material value)
	{
		image.material = value;
	}

	public void SelectSlot()
	{
		boardEditor.EnableSlotManipulation(false);
	}
}
