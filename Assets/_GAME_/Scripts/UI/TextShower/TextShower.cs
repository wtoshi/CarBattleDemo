using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShower : MonoBehaviour
{
    public static TextShower Instance { get; private set; }

    [SerializeField] Transform effectParent;
    [SerializeField] GameObject textItemPF;

    GameObject currentEffect;

    public enum TextShowerType
    {
        DefeatedEntity, HitEntity
    }

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void ShowText(string _text,TextShowerType textType)
    {
        var showingText = _text;

        if (currentEffect != null)
            Destroy(currentEffect);

        //var pf = Master.Instance.GetResourcesByName<GameObject>("TextEffectPF");

        //if (pf == null) return;

        currentEffect = Master.Instance.AddChild(effectParent.gameObject, textItemPF);

        var effect = currentEffect.GetComponent<TextShowerItem>();

        effect.Set(showingText, textType);
    }
}
