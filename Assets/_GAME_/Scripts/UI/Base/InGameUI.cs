using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : BaseUI
{
	[SerializeField] Slider destroyingSlider;
	[SerializeField] TextMeshProUGUI destroyingTxt;

    private void Update()
    {
		HandleDestroyingSlider();
    }

    private void OnEnable()
	{
		GameEvents.LevelStarted += OnLevelStarted;
		GameEvents.LevelFinished += OnLevelFinished;
	}

	private void OnDisable()
	{
		GameEvents.LevelStarted -= OnLevelStarted;
		GameEvents.LevelFinished -= OnLevelFinished;
	}


	private void OnLevelStarted()
	{
		ResetDestroyingSlider();

		SetShown();
	}

	void OnLevelFinished(bool successed)
	{
		ResetDestroyingSlider();
		SetHidden();
	}

	void OnLevelRestarted()
	{
		//progressBar.gameObject.SetActive(false);
	}

	void OnLevelEndStatus(bool _mode)
	{

	}

	private void OnLevelFail()
	{
		SetHidden();
	}

	private void OnLevelSuccess()
	{
		SetHidden();
	}

    #region DESTROYING SLIDER

    float loadingAmount = 0;
	bool destryongSliderEnabled = false;
	float sliderDuration = 0;

	public void InitDestroyingSlider(float duration)
    {
		ResetDestroyingSlider();

		sliderDuration = duration;
		destryongSliderEnabled = true;
	}

	void HandleDestroyingSlider()
	{
		if (!destryongSliderEnabled || sliderDuration <= 0)
			return;

		loadingAmount += Time.deltaTime / sliderDuration;

		loadingAmount = Mathf.Clamp(loadingAmount, 0,1);
		destroyingSlider.value = loadingAmount;

		if (loadingAmount >= 1)
        {
			destroyingTxt.gameObject.SetActive(true);

			ResetDestroyingSlider();
		}

	}

	void ResetDestroyingSlider()
    {
		destryongSliderEnabled = false;
		sliderDuration = 0;
		loadingAmount = 0;
		destroyingSlider.value = 0;
	}



    #endregion
}
