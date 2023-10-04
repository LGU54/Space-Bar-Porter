using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class SettingsManager : MonoBehaviour
{
    private Toggle mAudioButton;
    private Toggle mGameButton;
    private Toggle mSystemButton;
    private Button mBackButton;

    private CanvasGroup mAudioSettings;
    private CanvasGroup mGameSettings;
    private CanvasGroup mSystemSettings;

    private Slider mBackgroundMusicVolume;
    private Slider mEffectVolume;
    private TMP_Text mBackgroundMusicVolumeValue;
    private TMP_Text mEffectVolumeValue;

    private TMP_Dropdown mLanguageSelector;
    private readonly string[] mLanguageOptions = { "en", "zh_Hans" };

    private void Awake()
    {
        mAudioButton = transform.Find("Menu/Audio").GetComponent<Toggle>();
        mGameButton = transform.Find("Menu/Game").GetComponent<Toggle>();
        mSystemButton = transform.Find("Menu/System").GetComponent<Toggle>();
        mBackButton = transform.Find("Back").GetComponent<Button>();
        mBackButton.onClick.AddListener(() => {
            ControlSceneManager.SwitchSceneWithoutConfirm("StartScene");
        });

        mAudioSettings = transform.Find("AudioSettings").GetComponent<CanvasGroup>();
        mGameSettings = transform.Find("GameSettings").GetComponent<CanvasGroup>();
        mSystemSettings = transform.Find("SystemSettings").GetComponent<CanvasGroup>();

        mBackgroundMusicVolume = transform.Find("AudioSettings/Background/Slider").GetComponent<Slider>();
        mEffectVolume = transform.Find("AudioSettings/Effect/Slider").GetComponent<Slider>();
        mBackgroundMusicVolumeValue = transform.Find("AudioSettings/Background/Value").GetComponent<TMP_Text>();
        mEffectVolumeValue = transform.Find("AudioSettings/Effect/Value").GetComponent<TMP_Text>();

        Init();
    }

    private void Init()
    {
        mAudioButton.SetIsOnWithoutNotify(true);
        mAudioButton.onValueChanged.AddListener(value => {
            StartCoroutine(FadeGameSettings(0));
            StartCoroutine(FadeSystemSettings(0));
            StartCoroutine(FadeAudioSettings(1));
        });

        mGameButton.onValueChanged.AddListener(value => {
            StartCoroutine(FadeAudioSettings(0));
            StartCoroutine(FadeSystemSettings(0));
            StartCoroutine(FadeGameSettings(1));
        });

        mSystemButton.onValueChanged.AddListener(value => {
            StartCoroutine(FadeAudioSettings(0));
            StartCoroutine(FadeGameSettings(0));
            StartCoroutine(FadeSystemSettings(1));
        });
    
        mAudioSettings.alpha = 1;
        mAudioSettings.blocksRaycasts = true;

        mGameSettings.alpha = 0;
        mGameSettings.blocksRaycasts = false;

        mSystemSettings.alpha = 0;
        mSystemSettings.blocksRaycasts = false;

        mBackgroundMusicVolume.SetValueWithoutNotify(UserPreference.Contains("BackgroundMusicVolume") ? UserPreference.Read<int>("BackgroundMusicVolume") : 10);
        mEffectVolume.SetValueWithoutNotify(UserPreference.Contains("EffectVolume") ? UserPreference.Read<int>("EffectVolume") : 80);
        mBackgroundMusicVolumeValue.text = mBackgroundMusicVolume.value.ToString();
        mEffectVolumeValue.text = mEffectVolume.value.ToString();

        mBackgroundMusicVolume.onValueChanged.AddListener(value => {
            UserPreference.Save("BackgroundMusicVolume", (int) value);
            mBackgroundMusicVolume.SetValueWithoutNotify(value);
            mBackgroundMusicVolumeValue.text = value.ToString();

            AudioManager.ChangeBgVolume(value / 100f);
        });
        mEffectVolume.onValueChanged.AddListener(value => {
            UserPreference.Save("EffectVolume", (int) value);
            mEffectVolume.SetValueWithoutNotify(value);
            mEffectVolumeValue.text = value.ToString();

            AudioManager.ChangeSFXVolume(value / 100f);
        });
    }
}

public partial class SettingsManager
{
    IEnumerator FadeAudioSettings(float targetAlpha)
    {
        while (!Mathf.Approximately(mAudioSettings.alpha, targetAlpha))
        {
            mAudioSettings.alpha = Mathf.MoveTowards(mAudioSettings.alpha, targetAlpha, 0.2f);
            yield return null;
        }

        mAudioSettings.alpha = targetAlpha;
        mAudioSettings.blocksRaycasts = targetAlpha == 1;
    }

    IEnumerator FadeGameSettings(float targetAlpha)
    {
        while (!Mathf.Approximately(mGameSettings.alpha, targetAlpha))
        {
            mGameSettings.alpha = Mathf.MoveTowards(mGameSettings.alpha, targetAlpha, 0.2f);
            yield return null;
        }

        mGameSettings.alpha = targetAlpha;
        mGameSettings.blocksRaycasts = targetAlpha == 1;
    }

    IEnumerator FadeSystemSettings(float targetAlpha)
    {
        while (!Mathf.Approximately(mSystemSettings.alpha, targetAlpha))
        {
            mSystemSettings.alpha = Mathf.MoveTowards(mSystemSettings.alpha, targetAlpha, 0.2f);
            yield return null;
        }

        mSystemSettings.alpha = targetAlpha;
        mSystemSettings.blocksRaycasts = targetAlpha == 1;
    }
}
