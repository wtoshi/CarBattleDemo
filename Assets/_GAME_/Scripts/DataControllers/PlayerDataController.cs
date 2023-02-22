using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class PlayerDataController : DataControler<PlayerDataController>
{
	const string DATAKEY = Consts.FileNames.PLAYERDATA;

	[SerializeField] [ReadOnly] GameEntries.PlayerData playerData = null;
	public GameEntries.PlayerData PlayerData => playerData;

	#region DATA FUNCTIONS

	public override void LoadData(System.Action onComplete = null, System.Action onFail = null)
	{
		playerData = DataUtils.LoadData<GameEntries.PlayerData>(DATAKEY);

		if (playerData == null)
        {
			onFail?.Invoke();
			return;
		}	

		if (!HasCar())
        {
			var carList = GameUtils.GetCars();

			var defaultCar = carList.Find(x=> x.isDefaultCar);

            if (defaultCar != null)
            {
				OwnCar(defaultCar.carID, true);
            }
            else
            {
				var addingCar = carList[0];

				if (addingCar == null)
					Debug.LogError("LogError: No Car!");

				OwnCar(addingCar.carID, true);
			}
        }

		onComplete?.Invoke();
	}

	protected override void SaveData()
	{
		DataUtils.SaveData(DATAKEY, Master.SerializeObject(playerData));
	}

	protected override void ClearData()
	{
		playerData = new GameEntries.PlayerData();

		DataUtils.ClearData(DATAKEY, Master.SerializeObject(playerData));
	}
    #endregion

    #region CHECKS

	public bool HasCar()
    {
		return playerData.ownedCars.Count > 0;
	}

	public bool HasName()
    {
		var playerName = playerData.playerName;

		return !string.IsNullOrEmpty(playerName);
    }

	public bool HasCountry()
	{
		var countryCode = playerData.playerCountry.CountryCode;

		return !string.IsNullOrEmpty(countryCode);
	}

    #endregion

    #region GETS

	public GameEntries.CarDataEntry GetOwnedCarData(int carID)
    {
		var ownedCar = playerData.ownedCars.Find(x=>x.carID == carID);

		return ownedCar;
    }

    #endregion

    #region DATA ACTIONS
    [Button("Set Coins", ButtonSizes.Medium)]
	public void SetCoins(int gold, bool save = true)
	{
		playerData.golds = gold;

		if (playerData.golds < 0)
			playerData.golds = 0;

		GameEvents.Instance.OnCurrencyUpdated();

		if (save)
		{
			SaveData();
		}
	}

	[Button("Set Player Name", ButtonSizes.Medium)]
	public void SetName(string playerName, bool save = true)
	{
		playerData.playerName = playerName;

		if (save)
		{
			SaveData();
		}
	}

	[Button("Reset Player Country", ButtonSizes.Medium)]
	public void ResetPlayerCountry()
	{
		playerData.playerCountry = new GameEntries.Country();

		SaveData();
	}

	public void SetCountry(GameEntries.Country playerCountry, bool save = true)
	{
		playerData.playerCountry = playerCountry;

		if (save)
		{
			SaveData();
		}
	}


	/// <summary>
	/// Increases and saves Player Level.
	/// </summary>
	public void IncreasePlayerLevel()
	{
		playerData.playerLevel++;
		SaveData();
	}

	/// <summary>
	/// Increases and saves coins.
	/// </summary>
	public void IncreaseCoins(int amount)
	{
		playerData.golds += amount;

		GameEvents.Instance.OnCurrencyUpdated();
		SaveData();
	}

	/// <summary>
	/// Increases and saves coins.
	/// </summary>
	public void DecreaseCoins(int amount)
	{
		playerData.golds -= amount;

		if (playerData.golds < 0)
			playerData.golds = 0;

		GameEvents.Instance.OnCurrencyUpdated();

		SaveData();
	}

	public void OwnCar(int _carID, bool setSelected = false)
    {
		playerData.ownedCars.Add(new GameEntries.CarDataEntry() { carID = _carID, currentCarPaintID = 0 }); ;

		if (setSelected)
			playerData.currentCarID = _carID;

		SaveData();
    }
    #endregion
}