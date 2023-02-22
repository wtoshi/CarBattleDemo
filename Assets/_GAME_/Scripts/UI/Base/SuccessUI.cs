using UnityEngine;

public class SuccessUI : BaseUI
{
	
	private void OnEnable()
	{
		GameEvents.LevelLoaded += OnLevelLoaded;
		GameEvents.LevelFinished += OnLevelFinished;
	}

	private void OnDisable()
	{
		GameEvents.LevelLoaded -= OnLevelLoaded;
		GameEvents.LevelFinished -= OnLevelFinished;
	}

	private void OnLevelLoaded()
	{
		SetHidden();
	}

	private void OnLevelSuccess()
	{
		SetShown();
	}

	void OnLevelFinished(bool successed)
	{
		if (successed)
		{
			OnLevelSuccess();
		}
	}

}
