using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData : MonoBehaviour
{
	public string cellName = "unnamed cell";
	public Sprite cellSprite;
	public int damage = 1;
	public int health = 1;
	public string description = "this cell f_cks";
	public CellAbility[] abilityArray;
	public int saveID = 0;

    void Start()
    {
        
    }
}
