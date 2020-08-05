using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellOption : MonoBehaviour
{
	public Text nameText;
	public Text damageText;
	public Text healthText;

	private CellLibrary cellLibrary;

    void Start()
    {
		cellLibrary = FindObjectOfType<CellLibrary>();
    }

	public void CellSelected()
	{
		cellLibrary.ShowInfo(true);
	}
}
