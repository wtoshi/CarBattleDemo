using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextShowerItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI effectTxt;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] Color defeatedColor;
    [SerializeField] Color hitColor;

    RectTransform _myRect;

    Tween myTween;

    private void Awake()
    {
        _myRect = GetComponent<RectTransform>();

        ResetMe();
    }

    public void Set(string _txt, TextShower.TextShowerType textShowerType)
    {
        effectTxt.text = _txt;

        switch (textShowerType)
        {
            case TextShower.TextShowerType.DefeatedEntity:
                effectTxt.color = defeatedColor;
                break;
            default:
                effectTxt.color = Color.black;
                break;
        }

        Anim();
    }

    void Anim()
    {
        transform.DOScale(1f, .5f).SetEase(Ease.OutBack).From(.5f).OnComplete(() => {
            var toMoveY = transform.position.y;
            transform.DOMoveY(toMoveY + 2f, 3f).SetEase(Ease.Linear).OnComplete(DestroyMe);

            canvasGroup.DOFade(0, .5f).SetEase(Ease.Linear).SetDelay(1f);
        });
    }

    void ResetMe()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1;        
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //myTween.Kill();
    }
}
