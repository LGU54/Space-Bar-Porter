using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class ControlSceneManager : MonoBehaviour
{
    public static string CurrentScene;

    public static ControlSceneManager Instance;
    private CanvasGroup mCanvasGroup;
    private Slider mProgress;

    public static bool CanTransition;

    private void Awake()
    {
        Instance = this;
        CurrentScene = "ControlScene";

        mCanvasGroup = GetComponent<CanvasGroup>();
        mProgress = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            Debug.Log("Load");
            LoadSceneWithoutConfirm("MainScene");
        }
    }
}

public partial class ControlSceneManager
{
    public static void LoadScene(string targetScene, Action action = null)
    {
        if (targetScene == CurrentScene) return;

        CanTransition = false;
        Instance.StartCoroutine(Instance.LoadSceneCoroutine(targetScene, action));
    }
    
    public static void LoadSceneWithoutConfirm(string targetScene, Action action = null)
    {
        if (targetScene == CurrentScene) return;

        CanTransition = true;
        Instance.StartCoroutine(Instance.LoadSceneCoroutine(targetScene, action));
    }

    public static void SwitchScene(string targetScene, Action action = null)
    {
        if (targetScene == CurrentScene) return;

        CanTransition = false;
        Instance.StartCoroutine(Instance.SwitchSceneCoroutine(targetScene, action));
    }

    public static void SwitchSceneWithoutConfirm(string targetScene, Action action = null)
    {
        if (targetScene == CurrentScene) return;

        CanTransition = true;
        Instance.StartCoroutine(Instance.SwitchSceneCoroutine(targetScene, action));
    }

    IEnumerator LoadSceneCoroutine(string targetScene, Action action = null)
    {
        yield return Fade(1);
        yield return new WaitForSeconds(1f);

        var operation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (operation.progress <= 0.9f)
        {
            if (operation.progress == 0.9f && mProgress.value == 0.9f)
            {
                break;
            }

            mProgress.value = (mProgress.value + (operation.progress - mProgress.value) / 10) > 0.85f ? 0.9f : mProgress.value + (operation.progress - mProgress.value) / 10;
            yield return null;
        }

        mProgress.value = 1;

        operation.allowSceneActivation = true;
        CurrentScene = targetScene;

        action?.Invoke();

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => {return CanTransition;});
        CanTransition = false;

        yield return Fade(0);
    }

    IEnumerator SwitchSceneCoroutine(string targetScene, Action action = null)
    {
        yield return Fade(1);
        yield return new WaitForSeconds(1f);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        var operation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (operation.progress <= 0.9f)
        {
            if (operation.progress == 0.9f && mProgress.value == 0.9f)
            {
                break;
            }

            mProgress.value = (mProgress.value + (operation.progress - mProgress.value) / 10) > 0.85f ? 0.9f : mProgress.value + (operation.progress - mProgress.value) / 10;
            yield return null;
        }

        mProgress.value = 1;

        operation.allowSceneActivation = true;
        CurrentScene = targetScene;

        action?.Invoke();

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => {return CanTransition;});
        CanTransition = false;

        yield return Fade(0);
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (targetAlpha == 1)
        {
            mCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            mCanvasGroup.blocksRaycasts = false;
        }

        while (!Mathf.Approximately(mCanvasGroup.alpha, targetAlpha))
        {
            mCanvasGroup.alpha = Mathf.MoveTowards(mCanvasGroup.alpha, targetAlpha, 0.1f);
            yield return null;
        }
        mCanvasGroup.alpha = targetAlpha;
    }
}
