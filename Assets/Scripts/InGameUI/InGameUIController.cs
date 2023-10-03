using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class InGameUIController : MonoBehaviour
{
    public static InGameUIController Instance;
    public Sprite[] SkillSprites;

    private CanvasGroup mCanvasGroup;

    private Button mSkill;
    private TMP_Text mSpaceBarCount;

    private Button mMenu;
    private CanvasGroup mMenuPanelCG;
    private Button mQuit;
    private Button mRestart;
    private Button mResume;

    void Awake()
    {
        Instance = this;

        mCanvasGroup = this.GetComponent<CanvasGroup>();

        mSkill = transform.Find("Skill").GetComponent<Button>();
        mSpaceBarCount = transform.Find("SpaceBarCount/Value").GetComponent<TMP_Text>();

        mMenu = transform.Find("Menu").GetComponent<Button>();
        mMenu.onClick.AddListener(() => StartCoroutine(FadeMenuPanelCoroutine(1)));
        mMenuPanelCG = transform.Find("MenuPanel").GetComponent<CanvasGroup>();
        mMenuPanelCG.alpha = 0;

        mQuit = transform.Find("MenuPanel/Quit").GetComponent<Button>();
        mQuit.onClick.AddListener(() => {
            ControlSceneManager.SwitchSceneWithoutConfirm("StartScene");
        });

        mRestart = transform.Find("MenuPanel/Restart").GetComponent<Button>();
        mRestart.onClick.AddListener(() => {
            ControlSceneManager.ReloadSceneWithoutConfirm(ControlSceneManager.CurrentScene);
        });

        mResume = transform.Find("MenuPanel/Resume").GetComponent<Button>();
        mResume.onClick.AddListener(() => StartCoroutine(FadeMenuPanelCoroutine(0)));
    }
}

public partial class InGameUIController
{
    public static void SetSkillActive(bool state)
    {
        Instance.mSkill.interactable = state;
    }

    public static void SetSkill(AreaType type)
    {
        switch(type)
        {
            case AreaType.JumpArea:
                Instance.mSkill.image.sprite = Instance.SkillSprites[0];
                break;
            case AreaType.ReverseGravityArea:
                Instance.mSkill.image.sprite = Instance.SkillSprites[1];
                break;
            case AreaType.FlashArea:
                Instance.mSkill.image.sprite = Instance.SkillSprites[2];
                break;
        }
    }

    public static void UpdateSpaceBarCount(int count)
    {
        Instance.mSpaceBarCount.SetText($"× {count}");
    }

    IEnumerator FadeMenuPanelCoroutine(float targetAlpha)
    {
        mMenuPanelCG.blocksRaycasts = targetAlpha == 1;

        while (!Mathf.Approximately(mMenuPanelCG.alpha, targetAlpha))
        {
            mMenuPanelCG.alpha = Mathf.MoveTowards(mMenuPanelCG.alpha, targetAlpha, 0.3f);
            yield return null;
        }
        mMenuPanelCG.alpha = targetAlpha;
    }
}