using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameLoader : MonoBehaviour
{
    public Slider loadingSlider;
    public TextMeshProUGUI loadingTxt;

    float loadingAmount = 0;
    Coroutine loginCoroutine;
    int totalTask;

    private void Start()
    {
        StartLogin();
    }

    void StartLogin()
    {
        totalTask = 2;
        loginCoroutine = StartCoroutine(doStartLogin());
    }

    IEnumerator doStartLogin()
    {
        // LOAD DATA
        LevelDataController.Instance.LoadData(() => IncreaseProgress("Level Data Loaded."), () => StopLogin("Data Loading Error! : Level Data"));
        PlayerDataController.Instance.LoadData(() => IncreaseProgress("Player Data Loaded."), () => StopLogin("Data Loading Error! : Player Data"));

        while (loadingAmount < 1)
            yield return null;

        // CHECK PLAYER NAME
        bool checkName = false;
        CheckPlayerName((res)=> {
            Debug.Log("CHECKED!");
            checkName = true;
        });

        yield return new WaitUntil(()=> checkName);

        // LOAD GAMESCENE
        var sceneAsync = SceneManager.LoadSceneAsync("GameScene");

        sceneAsync.allowSceneActivation = false;

        while (sceneAsync.progress < 0.9f)
            yield return null;

        // FAKE WAITING
        yield return new WaitForSeconds(2f);

        sceneAsync.allowSceneActivation = true;
    }

    void StopLogin(string _errorMsg = null)
    {
        if (loginCoroutine != null)
        {
            if (_errorMsg == "NoData")
            {
                IncreaseProgress("StopLogin");
                return;
            }

            StopCoroutine(loginCoroutine);

            //TODO Loading Alert Kapat
            //DialogController.CloseLoadingAlertDialog();

            Debug.LogError("Stop Error: " + _errorMsg);
        }
    }

    void IncreaseProgress(string increasedFrom)
    {
        float val = (float)(1 / (float)totalTask);

        loadingAmount += val;

        if (loadingAmount > 0.99)
            loadingAmount = 1;

        loadingSlider.value = loadingAmount;
    }

    void CheckPlayerName(System.Action<bool> onComplete)
    {
        var hasName = PlayerDataController.Instance.HasName();
        var hasCountry = PlayerDataController.Instance.HasCountry();

        if (hasName && hasCountry)
        {
            onComplete?.Invoke(true);
        }
        else
        {
            DialogController.Show("PlayerDetailsDialog", null, ()=> {
                onComplete?.Invoke(true);
            });
        }
    }
}
