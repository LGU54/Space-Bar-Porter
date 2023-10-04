using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class StartSceneUIController : MonoBehaviour
{
    private Button mStart;
    private Button mContinue;
    private Button mSettings;
    private Button mQuit;

    private MessageManager mMessage;

    private Button mCredit;
    private CanvasGroup mCreditPanel;
    private Button mCreditClose;

    private void Awake()
    {
        mStart = transform.Find("Menu/Start").GetComponent<Button>();
        mContinue = transform.Find("Menu/Continue").GetComponent<Button>();
        mSettings = transform.Find("Menu/Settings").GetComponent<Button>();
        mQuit = transform.Find("Menu/Quit").GetComponent<Button>();

        mMessage = transform.Find("Message").GetComponent<MessageManager>();

        mCredit = transform.Find("Credit").GetComponent<Button>();
        mCreditPanel = transform.Find("CreditPanel").GetComponent<CanvasGroup>();
        mCreditClose = transform.Find("CreditPanel/Close").GetComponent<Button>();

        mCredit.onClick.AddListener(() => {
            StartCoroutine(FadeCreditCoroutine(1));
        });
        mCreditClose.onClick.AddListener(() => {
            StartCoroutine(FadeCreditCoroutine(0));
        });

        mStart.onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX("Click");
            ControlSceneManager.SwitchSceneWithoutConfirm("no0", () =>
            {
                DialogFSM.GetInstance().Context = new PlotReader(0);
                DialogFSM.GetInstance().Context.ReadBeforeLines();
                DialogFSM.NextLine();
            });
        });

        mContinue.onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX("Click");
            if (UserPreference.Read<string>("save") == null)
            {
                mMessage.Show(Localization.Get("NoSave"), () => {
                    DialogFSM.GetInstance().Context = new PlotReader(0);
                    DialogFSM.GetInstance().Context.ReadBeforeLines();
                    DialogFSM.NextLine();
                });
            }
            else
            {
                ControlSceneManager.SwitchSceneWithoutConfirm(UserPreference.Read<string>("save"), () =>
                {
                    // Debug.Log(DialogFSM.GetInstance().Context);
                    DialogFSM.SetContext(Int32.Parse(UserPreference.Read<string>("save").Replace("no", "")));
                    DialogFSM.GetInstance().Context.ReadBeforeLines();
                    DialogFSM.NextLine();
                });
            }
        });

        mSettings.onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX("Click");
            mMessage.Show(Localization.Get("OnSettingsClick"), () => {
                ControlSceneManager.SwitchSceneWithoutConfirm("SettingsScene");
            });
        });

        mQuit.onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX("Click");
            mMessage.Show(Localization.Get("OnApplicationQuit"), () => Application.Quit());
        });

        AudioManager.ChangeBgVolume(0.1f);
        UserPreference.Save("BackgroundMusicVolume", 10);
        AudioManager.ChangeSFXVolume(0.8f);
        UserPreference.Save("EffectVolume", 80);
    }
}

public partial class StartSceneUIController
{
    IEnumerator FadeCreditCoroutine(float targetAlpha)
    {
        mCreditPanel.blocksRaycasts = targetAlpha == 1;

        while(!Mathf.Approximately(mCreditPanel.alpha, targetAlpha))
        {
            mCreditPanel.alpha = Mathf.MoveTowards(mCreditPanel.alpha, targetAlpha, 0.3f);
            yield return null;
        }
        mCreditPanel.alpha = targetAlpha;
    }
}