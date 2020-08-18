using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryOptionSort : MonoBehaviour
{
	void Start()
    {
		
    }

	public List<CellOption> GetSortedList(List<CellOption> inputOptionList)
	{
		List<CellOption> myCellOptions = new List<CellOption>();

		int numOptions = inputOptionList.Count;
		for(int i = 0; i < numOptions; i++)
		{
			int highestValue = 99;
			CellOption lowestValueOption = null;
			foreach (CellOption co in inputOptionList)
			{
				CellData cd = co.GetCellData();
				if (cd != null)
				{
					if ((cd.cellValue < highestValue) && !myCellOptions.Contains(co))
					{
						lowestValueOption = co;
						highestValue = cd.cellValue;
					}
				}
			}

			myCellOptions.Add(lowestValueOption);
		}

		return myCellOptions;
	}
}
