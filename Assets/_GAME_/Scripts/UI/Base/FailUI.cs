using UnityEngine;

public class FailUI : BaseUI
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

	private void OnLevelFail()
	{
		SetShown();
	}

	void OnLevelFinished(bool successed)
	{
		if (!successed)
		{
			OnLevelFail();
		}
	}
}
