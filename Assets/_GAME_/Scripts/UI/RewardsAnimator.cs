using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class RewardsAnimator : Singleton<RewardsAnimator>
{

    [System.Serializable]
    public class Assigns
    {
        //public Camera UICamera;
        public GameObject screenBlock;
        public Transform parentTransform;
        public RectTransform startingTransform;
        public RectTransform firstPointTransform;

        public GameObject animatingPF;
        public Transform targetTransform;
    }
    public Assigns assigns = new Assigns();


    private void Awake()
    {
        assigns.screenBlock.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartAnimating(50);
        }
    }

    public void StartAnimating(int goldAmount, System.Action _OnComplete = null)
    {
        StartCoroutine(AnimateRewards(goldAmount,_OnComplete));
    }

    IEnumerator AnimateRewards(int goldAmount, System.Action _OnComplete)
    {
        assigns.screenBlock.SetActive(true);

        var spawnPos = assigns.startingTransform.position;
        var obj = Instantiate(assigns.animatingPF, spawnPos, Quaternion.identity, assigns.parentTransform);

        var firstPos = assigns.firstPointTransform.position;
        obj.transform.DOMove(firstPos + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f), 1f)
            .SetEase(Ease.InOutCirc).OnComplete(() => {

                obj.transform.DOMove(assigns.targetTransform.position, .5f).SetDelay(.3f).OnComplete(() => {

                    PlayerDataController.Instance.IncreaseCoins(goldAmount);

                    assigns.targetTransform.DOScale(1f, .1f).From(0.9f).SetEase(Ease.OutBack);

                    Destroy(obj);
                });

            });

        yield return new WaitForSeconds(1f);

        assigns.screenBlock.SetActive(false);

        _OnComplete?.Invoke();
    }

}
