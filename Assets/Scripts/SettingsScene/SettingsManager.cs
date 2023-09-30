using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SettingsManager : MonoBehaviour
{
    private Button mAudioButton;
    private Button mGameButton;
    private Button mSystemButton;

    private CanvasGroup mAudioSettings;
    private CanvasGroup mGameSettings;
    private CanvasGroup mSystemSettings;

    private void Awake()
    {
        mAudioButton = transform.Find("Menu/AudioButton").GetComponent<Button>();
        mGameButton = transform.Find("Menu/GameButton").GetComponent<Button>();
        mSystemButton = transform.Find("Menu/SystemButton").GetComponent<Button>();

        mAudioSettings = transform.Find("AudioSettings").GetComponent<CanvasGroup>();
        mGameSettings = transform.Find("GameSettings").GetComponent<CanvasGroup>();
        mSystemSettings = transform.Find("SystemSettings").GetComponent<CanvasGroup>();

        Init();
    }

    private void Init()
    {
        mAudioSettings.alpha = 1;
        mAudioSettings.blocksRaycasts = true;

        mGameSettings.alpha = 0;
        mGameSettings.blocksRaycasts = false;

        mSystemSettings.alpha = 0;
        mSystemSettings.blocksRaycasts = false;
    }
}
