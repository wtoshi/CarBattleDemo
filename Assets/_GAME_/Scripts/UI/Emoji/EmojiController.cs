using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DanielLochner.Assets.SimpleScrollSnap;

public class EmojiController : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] Transform emojiHolder;
    [SerializeField] GameObject emojiItemPF;

    [SerializeField] Image cooldownFill;
    [SerializeField] TextMeshProUGUI cooldownTxt;
    [SerializeField] SimpleScrollSnap scrollSnap;

    Dictionary<int, GameEntries.EmojiData> emojiList = new Dictionary<int, GameEntries.EmojiData>();

    
    int selectedEmojiID = 0;

    private void Start()
    {
        GetEmojies();
        SetEmojies();
    }

    private void FixedUpdate()
    {
        HandleCD();
    }

    private void OnEnable()
    {
        ResetGCD();
    }

    #region COOLDOWN

    bool cdActive = false;
    float currentCD = 0;
    float cdDuration = 3;
    void InitCD()
    {
        if (cdDuration <= 0)
            return;

        currentCD = cdDuration;
        cdActive = true;
    }

    void ResetGCD()
    {
        currentCD = 0;
        cdActive = false;
        cooldownTxt.text = "";
        cooldownFill.fillAmount = 0;
    }

    void HandleCD()
    {
        if (!cdActive || cdDuration <= 0)
            return;

        cooldownFill.fillAmount = currentCD / cdDuration;
        cooldownTxt.text = currentCD.ToString("F0");

        if (currentCD > 0)
            currentCD -= Time.fixedDeltaTime;
        else
            ResetGCD();
    }

    #endregion

    void GetEmojies()
    {
        var emojies = GameUtils.GetEmojies();

        int id = 0;
        foreach (var emoji in emojies)
        {
            emojiList.Add(id, emoji);

            id++;
        }

        currentCD = 0;
    }

    void SetEmojies()
    {
        foreach (var kv in emojiList)
        {
            //var emojiObj = Instantiate(emojiItemPF, emojiHolder);

            scrollSnap.Add(emojiItemPF, kv.Key);

            //EmojiItem emojiItem = scrollSnap.Panels[kv.Key] emojiObj.GetComponent<EmojiItem>();
            EmojiItem emojiItem = scrollSnap.Panels[kv.Key].GetComponent<EmojiItem>();
            emojiItem.Init(kv.Key, kv.Value);

            
        }
    }

    public void SelectEmoji()
    {
        selectedEmojiID = scrollSnap.CurrentPanel;

        if (cdActive)
            return;

        SendEmoji(selectedEmojiID);
    }

    public void SendEmoji(int emojiID)
    {
        var playerConroller = LevelController.Instance.Player;

        if (playerConroller == null)
            return;

        var emojiPF = emojiList[emojiID].emojiPF;

        if (emojiPF == null)
            return;

        playerConroller.SendEmoji(emojiPF);

        InitCD();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cdActive)
            return;

        SendEmoji(selectedEmojiID);
    }
}
