using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class DialogController : MonoBehaviour
{

    // Use this for initialization
    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }

    [HideInInspector]
    public Animator animator;
    GameObject dialogRoot;
    [HideInInspector]
    public string[] parameters;
    [HideInInspector]
    public System.Action onCloseComplete;
    [HideInInspector]
    public System.Action onOpenComplete;
    [HideInInspector]
    public System.Action customAction;
    [HideInInspector]
    public bool isOpenCompleted;
    [HideInInspector]
    public float showingTime = 0.4f;
    [HideInInspector]
    public float closingTime = 0.2f;

    bool isClosing;

    [System.Serializable]
    public class DialogOption
    {
        public bool playShowSound = true;
        public bool playCloseSound = true;
        public float ShowingSpeed = 1;
        public float ClosingSpeed = 1;
        public enum AnimationType
        {
            Scale
        }

        public AnimationType showAnimationType = AnimationType.Scale;
        public AnimationType closeAnimationType = AnimationType.Scale;

        public float delayTimeToShow = 0;

    }
    public DialogOption dialogOption = new DialogOption();

    public bool isShowing = false;


    void Awake()
    {
        if (GetComponent<Animator>() == null)
            animator = gameObject.AddComponent<Animator>();
        else
            animator = GetComponent<Animator>();

        showingTime = showingTime / dialogOption.ShowingSpeed;
        closingTime = closingTime / dialogOption.ClosingSpeed;

        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    void OnEnable()
    {
        //ShowDialog();
    }

    protected virtual void OnDestroy()
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    public static GameObject Show(string dialogName, string[] parameters = null, System.Action _customAction = null, bool showBlackPanel = true, float speedShow = 1)
    {

        GameObject dialogRoot = Master.Instance.GetChildByName(null, "DialogRoot", true);


        GameObject dialog = Master.Instance.GetChildByName(dialogRoot, dialogName, true);


        var obj = Master.Instance.GetResourcesByName<GameObject>(dialogName);
        dialog = Master.Instance.AddChild(dialogRoot, obj);

        if (dialog == null)
        {
            Debug.LogError("Can not find dialog <" + dialogName + ">!");
            return null;
        }

        dialog.name = dialogName;
        dialog.transform.SetAsLastSibling();
        dialog.GetComponent<DialogController>().parameters = parameters;

        if (dialog.GetComponent<DialogController>().dialogOption.delayTimeToShow > 0)
        {
            Master.Instance.WaitAndDo(dialog.GetComponent<DialogController>().dialogOption.delayTimeToShow, () =>
            {
                if (dialog != null)
                {
                    dialog.GetComponent<DialogController>().ShowDialog(_customAction);
                    dialog.GetComponent<DialogController>().ShowBlackPanel(showBlackPanel);
                }

            }, dialog.GetComponent<DialogController>());
        }
        else
        {
            dialog.GetComponent<DialogController>().ShowDialog(_customAction);
            dialog.GetComponent<DialogController>().ShowBlackPanel(showBlackPanel);
        }

        return dialog;
    }

    public virtual void ShowDialog(System.Action _customAction = null)
    {
        if (!transform.Find("Main").gameObject.activeSelf)
        {
            transform.Find("Main").gameObject.SetActive(true);
        }

        animator.enabled = true;
        animator.speed = dialogOption.ShowingSpeed;
        animator.Play("ShowDialog" + dialogOption.showAnimationType.ToString());

        customAction = _customAction;

        isShowing = true;

        OnShowDialog();

        Master.Instance.WaitAndDo(showingTime, () => { isOpenCompleted = true; OnOpenComplete(); }, this);

        if (dialogOption.playShowSound)
        {
            //TODO SOUND
            //AudioController.Instance.PlaySoundBySoundTemplate(15);
        }
    }

    public virtual void OnShowDialog()
    {

    }

    public static void ShowLoadingAlertDialog(string alert = "", bool isShowCancelButton = false, System.Action onCancel = null, float blackPanelAlpha = -1)
    {
        if (alert == "")
        {
            alert = "Loading";
        }

        //LoadingAlertDialog alertDialog = Show("LoadingAlertDialog").GetComponent<LoadingAlertDialog>();
        //if (alertDialog == null) Debug.Log("AlertDialog is null! ");
        //alertDialog.Set(alert, isShowCancelButton, blackPanelAlpha);
        //alertDialog.onCloseComplete = onCancel;
    }

    public static void CloseLoadingAlertDialog()
    {
        //if (FindObjectOfType<LoadingAlertDialog>() != null)
        //{
        //    FindObjectOfType<LoadingAlertDialog>().CloseImmediately();
        //}
    }


    public virtual void OnOpenComplete()
    {
        if (onOpenComplete != null)
        {
            onOpenComplete();
        }
    }

    public void ShowBlackPanel(bool isShow = true)
    {
        if (gameObject == null) return;

        GameObject blackPanel = Master.Instance.GetChildByName(gameObject, "BlackPanel", true);
        if (blackPanel != null)
        {
            blackPanel.SetActive(isShow);
        }
    }

    public void Close(System.Action onComplete)
    {
        if (isClosing || gameObject == null) return;

        isClosing = true;

        if (dialogOption.playCloseSound)
        {
            //TODO SOUND
            //AudioController.Instance.PlaySoundBySoundTemplate(16);
        }

        animator.speed = dialogOption.ClosingSpeed;
        animator.Play("CloseDialog" + dialogOption.closeAnimationType.ToString());
        isShowing = false;
        //        Debug.Log(closingTime);
        Master.Instance.WaitAndDo(closingTime, () =>
        {
            ShowBlackPanel(false);
            if (onComplete != null)
            {
                onComplete();
            }
            else
            {
                OnCloseComplete();
            }

            Destroy(gameObject);

        }, this);
    }

    public void Close()
    {
        if (isClosing || gameObject == null) return;

        isClosing = true;

        if (dialogOption.playCloseSound)
        {
            //TODO SOUND
            //AudioController.Instance.PlaySoundBySoundTemplate(16);
        }

        animator.speed = dialogOption.ClosingSpeed;
        animator.Play("CloseDialog" + dialogOption.closeAnimationType.ToString());
        isShowing = false;
        Master.Instance.WaitAndDo(closingTime, () =>
        {
            ShowBlackPanel(false);

            Destroy(gameObject);

            OnCloseComplete();
        }, this);
    }

    public void CloseImmediately()
    {
        if (isClosing || gameObject == null) return;

        isClosing = true;

        ShowBlackPanel(false);
        OnCloseComplete();
        isShowing = false;

        Destroy(gameObject);

    }

    public virtual void OnCloseComplete()
    {
        isClosing = false;
        onCloseComplete?.Invoke();
    }

    public static void CloseLastDialog(System.Action onCloseComplete = null)
    {
        FindObjectsOfType<DialogController>()[0].Close(onCloseComplete);
    }

    public static void CloseAllDialogs()
    {
        foreach (DialogController dialogController in FindObjectsOfType<DialogController>())
        {
            dialogController.Close();
        }
    }

}
