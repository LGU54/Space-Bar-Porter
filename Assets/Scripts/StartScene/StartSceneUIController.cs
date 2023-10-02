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

    private void Awake()
    {
        mStart = transform.Find("Menu/Start").GetComponent<Button>();
        mContinue = transform.Find("Menu/Continue").GetComponent<Button>();
        mSettings = transform.Find("Menu/Settings").GetComponent<Button>();
        mQuit = transform.Find("Menu/Quit").GetComponent<Button>();

        mMessage = transform.Find("Message").GetComponent<MessageManager>();

        mStart.onClick.AddListener(() => {
            ControlSceneManager.SwitchSceneWithoutConfirm("MainScene", () => {
                DialogFSM.GetInstance().Context = new PlotReader(5);
                DialogFSM.GetInstance().Context.ReadBeforeLines();
                DialogFSM.NextLine();
            });
        });

        mSettings.onClick.AddListener(() => {
            ControlSceneManager.SwitchSceneWithoutConfirm("SettingsScene");
        });

        mQuit.onClick.AddListener(() => {
            mMessage.Show(Localization.Get("OnApplicationQuit"), () => Application.Quit());
        });
    }
}
