using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuUI : BaseUI,  IPointerDownHandler
{
	[SerializeField] private GameObject tutorialHand;

	public bool hasRewards;

	private void OnEnable()
	{
		GameEvents.LevelLoaded += OnLevelLoaded;
		GameEvents.LevelStarted += OnLevelStarted;
		GameEvents.LevelFinished += OnLevelFinished;
	}

	private void OnDisable()
	{
		GameEvents.LevelLoaded -= OnLevelLoaded;
		GameEvents.LevelStarted -= OnLevelStarted;
		GameEvents.LevelFinished -= OnLevelFinished;
	}

	private void OnLevelLoaded()
	{
		SetShown();

		//PlayerDataController.Instance.IncreaseCoins(50);

        if (hasRewards)
            RewardsAnimator.Instance.StartAnimating(50, () => { hasRewards = false; });

        if (!tutorialHand.activeSelf)
			tutorialHand.SetActive(true);
	}

	void OnLevelStarted()
	{
		SetHidden();
	}

	void OnLevelFinished(bool successed)
    {
		SetHidden();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.GameState == GameEntries.GameState.InGame || hasRewards)
            return;

        tutorialHand.SetActive(false);

		GameManager.Instance.StartLevel();		
	}
}
