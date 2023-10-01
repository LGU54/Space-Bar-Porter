using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class MessageManager : MonoBehaviour
{
    private CanvasGroup mCanvasGroup;
    private TMP_Text mContent;
    private Button mCancel;
    private Button mConfirm;
    void Awake()
    {
        mCanvasGroup = this.GetComponent<CanvasGroup>();
        mContent = transform.Find("Body/Content").GetComponent<TMP_Text>();
        mCancel = transform.Find("Cancel").GetComponent<Button>();
        mConfirm = transform.Find("Confirm").GetComponent<Button>();

        mCancel.onClick.AddListener(() => StartCoroutine(FadeMessage(0)));
    }
}

public partial class MessageManager
{
    public void Show()
    {
        StartCoroutine(FadeMessage(1));
    }

    public void Show(string content)
    {
        SetMessage(content);
        StartCoroutine(FadeMessage(1));
    }

    public void Show(string content, Action action)
    {
        SetMessage(content);
        BindActionOnConfirm(action);

        StartCoroutine(FadeMessage(1));
    }

    public void SetMessage(string content)
    {
        mContent.SetText(content);
    }

    public void BindActionOnConfirm(Action action)
    {
        mConfirm.onClick.RemoveAllListeners();
        mConfirm.onClick.AddListener(() => action());
    }

    IEnumerator FadeMessage(float targetAlpha)
    {
        mCanvasGroup.blocksRaycasts = targetAlpha == 1;

        while (!Mathf.Approximately(mCanvasGroup.alpha, targetAlpha))
        {
            mCanvasGroup.alpha = Mathf.MoveTowards(mCanvasGroup.alpha, targetAlpha, 0.3f);
            yield return null;
        }
        mCanvasGroup.alpha = targetAlpha;
    }
}