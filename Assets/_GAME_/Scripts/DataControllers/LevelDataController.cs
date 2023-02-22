using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class LevelDataController : DataControler<LevelDataController>
{
	const string DATAKEY = Consts.FileNames.LEVELDATA;

	[SerializeField] [ReadOnly] GameEntries.LevelSaveData levelData = null;
	public GameEntries.LevelSaveData LevelData => levelData;

	#region DATA FUNCTIONS

	public override void LoadData(System.Action onComplete = null, System.Action onFail = null)
	{
		levelData = DataUtils.LoadData<GameEntries.LevelSaveData>(DATAKEY);

		if (levelData != null)
			onComplete?.Invoke();

		else
			onFail?.Invoke();
	}

	protected override void SaveData()
	{
		DataUtils.SaveData(DATAKEY, Master.SerializeObject(levelData));
	}

	protected override void ClearData()
	{
		levelData = new GameEntries.LevelSaveData();
		
		DataUtils.ClearData(DATAKEY, Master.SerializeObject(levelData));
	}
    #endregion

    [Button("Set CurrentLevelNo", ButtonSizes.Medium)]
	public void SetCurrentLevelNo(int currentLevelNo, bool save = true)
	{
		levelData.currentLevelNo = currentLevelNo;

		if (save)
		{
			SaveData();
		}
	}


	/// <summary>
	/// Increases and saves LevelNo.
	/// </summary>
	private void IncreaseLevelNo()
	{
		levelData.currentLevelNo++;
		SaveData();
	}
}