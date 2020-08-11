using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Game : MonoBehaviour
{
	public CombatantBoard playerBoard;
	public CombatantBoard opponentBoard;
	public List<CellData> playerCells;

    void Start()
    {
		playerCells = new List<CellData>();
		//DeleteSave();
	}

	//public virtual void SavePlayerBoardAsJSON()
	//{
	//	Save save = CreatePlayerBoardSave();
	//	string json = JsonUtility.ToJson(save);
	//	Debug.Log("Saving as JSON: " + json);
	//}

	public virtual void SaveGame()
	{
		Save save = CreatePlayerBoardSave();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, save);
		file.Close();

		//Debug.Log("Game Saved");
	}

	private Save CreatePlayerBoardSave()
	{
		Save save = new Save();
		List<int> playerCellIDs = new List<int>();
		CombatantBoard playerBoard = null;
		CombatantBoard[] boards = FindObjectsOfType<CombatantBoard>();
		foreach(CombatantBoard cb in boards)
		{
			if (cb.tag == "Player")
			{
				playerBoard = cb;
				break;
			}
		}
		if (playerBoard != null)
		{
			List<CellSlot> boardSlots = new List<CellSlot>();
			boardSlots = playerBoard.GetSlots();
			int numLiveCells = 0;
			for(int i = 0; i < boardSlots.Count; i++)
			{
				CellData cd = null;
				if (boardSlots[i].GetCell() != null)
					cd = boardSlots[i].GetCell().GetCellData();
				if (cd != null)
				{
					playerCells.Add(cd);
					if (playerCellIDs.Count > i)
						playerCellIDs[i] = cd.saveID;
					else
						playerCellIDs.Add(cd.saveID);
					numLiveCells++;
				}
				else
				{
					if (playerCellIDs.Count > i)
						playerCellIDs[i] = -1;
					else
						playerCellIDs.Add(-1);
				}
			}
		}
		
		save.playerCellIDs = playerCellIDs;
		return save;
	}

	public virtual void LoadGame()
	{
		// 1
		if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
		{
			CharacterDataPlayer player = FindObjectOfType<CharacterDataPlayer>();

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			Save save = (Save)bf.Deserialize(file);
			file.Close();

			playerCells = new List<CellData>();
			CellLibrary cellLibrary = FindObjectOfType<CellLibrary>();
			int numLiveCells = 0;
			for(int i = 0; i < save.playerCellIDs.Count; i++)
			{
				int cdID = save.playerCellIDs[i];
				if (cdID >= 0)
				{
					CellData IDCellData = cellLibrary.allCells[cdID];
					playerCells.Add(IDCellData);
					numLiveCells++;
				}
				else
				{
					playerCells.Add(null);
				}
			}

			playerBoard.LoadBoard(playerCells);

			//Debug.Log("Game Loaded");
		}
		//else
		//{
		//	Debug.Log("No game saved!");
		//}
	}

	public virtual void DeleteSave()
	{
		try
		{
			File.Delete(Application.persistentDataPath + "/gamesave.save");
			Debug.Log("deleted save");
		}
		catch (System.Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
