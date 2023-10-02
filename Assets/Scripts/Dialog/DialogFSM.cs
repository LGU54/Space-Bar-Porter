using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class DialogFSM : MonoBehaviour
{
    private static DialogFSM mSelf;
    public IState CurrentState;
    public DialogState CurrentStateType;
    public DialogState LastStateType;

    public Dictionary<DialogState, IState> States;
    public Sprite[] TachieSprites;
    private readonly static Dictionary<string, int> mTachieSprites = new Dictionary<string, int>() {
        {"P", 0}, {"S", 1}, {"J", 2}, {"G", 3}, {"D", 4}
    };

    public PlotReader Context;

    private CanvasGroup mCanvasGroup;
    public Button Button;

    public Image TachieA;
    public Image TachieB;
    public CanvasGroup NameBoxA;
    public CanvasGroup NameBoxB;
    public TMP_Text NameLabelA;
    public TMP_Text NameLabelB;
    public TMP_Text Text;

    public static DialogFSM GetInstance()
    {
        return mSelf;
    }

    void Awake()
    {
        mSelf = this;

        States = new Dictionary<DialogState, IState>();

        mCanvasGroup = this.GetComponent<CanvasGroup>();
        Button = this.GetComponent<Button>();

        TachieA = transform.Find("TachieA").GetComponent<Image>();
        TachieB = transform.Find("TachieB").GetComponent<Image>();
        NameBoxA = transform.Find("NameBoxA").GetComponent<CanvasGroup>();
        NameBoxB = transform.Find("NameBoxB").GetComponent<CanvasGroup>();
        NameLabelA = transform.Find("NameBoxA/Text").GetComponent<TMP_Text>();
        NameLabelB = transform.Find("NameBoxB/Text").GetComponent<TMP_Text>();
        Text = transform.Find("DialogBox/Text").GetComponent<TMP_Text>();

        RegisterState(DialogState.None, new NoneState(this));
        RegisterState(DialogState.Narration, new NarrationState(this));
        RegisterState(DialogState.CharacterATalking, new CharacterATalkingState(this));
        RegisterState(DialogState.CharacterBTalking, new CharacterBTalkingState(this));

        SetInitialState(DialogState.None);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.A) && Context.CurrentLines.Count == 0)
        {
            Context.ReadGameLines();
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.B) && Context.CurrentLines.Count == 0)
        {
            Context.ReadAfterLines();
            NextLine();
        }
    }
    
}

public partial class DialogFSM
{
    public static void NextLine()
    {
        if (mSelf.Context.CurrentLines.Count > 0)
        {
            var line = mSelf.Context.CurrentLines[0];
            SetContent(line.Text);
            SetIdentity(line.Type, line.Id);

            mSelf.ChangeState((DialogState) line.Type);
            mSelf.Context.CurrentLines.RemoveAt(0);
        }
        else
        {
            mSelf.ChangeState(DialogState.None);
        }
    }

    public void RegisterState(DialogState state, IState iState)
    {
        States.Add(state, iState);
    }

    public void ChangeState(DialogState targetState)
    {
        if (States[targetState] == null || !States[targetState].OnCondition())
        {
            return;
        }

        CurrentState.OnExit();

        CurrentState = States[targetState];
        CurrentStateType = targetState;

        CurrentState.OnEnter();
    }

    public void SetInitialState(DialogState targetState)
    {
        CurrentState = States[targetState];
        CurrentStateType = targetState;
        LastStateType = targetState;
    }

    public static void SetIdentity(LineType target, string id)
    {
        switch (target)
        {
            case LineType.CharacterA:
                mSelf.NameLabelA.text = Localization.Get(id);
                mSelf.TachieA.sprite = mSelf.TachieSprites[mTachieSprites[id]];
                break;
            case LineType.CharacterB:
                mSelf.NameLabelB.text = Localization.Get(id);
                mSelf.TachieB.sprite = mSelf.TachieSprites[mTachieSprites[id]];
                break;
            case LineType.Narration:
                mSelf.TachieA.sprite = null;
                mSelf.TachieB.sprite = null;
                break;
            default:
                break;
        }
    }

    public static void SetContent(string content)
    {
        mSelf.Text.text = content;
    }

    public static void FadeCharacter(string target, float targetAlpha)
    {
        switch (target)
        {
            case "A":
                mSelf.StartCoroutine(mSelf.FadeCharacterCoroutine("A", targetAlpha));
                break;
            case "B":
                mSelf.StartCoroutine(mSelf.FadeCharacterCoroutine("B", targetAlpha));
                break;
        }
    }

    IEnumerator FadeCharacterCoroutine(string target, float targetAlpha)
    {
        switch (target)
        {
            case "A":
                TachieA.CrossFadeAlpha(targetAlpha == 1f ? 1f : 0f, 0.3f, true);
                while (!Mathf.Approximately(NameBoxA.alpha, targetAlpha))
                {
                    NameBoxA.alpha = Mathf.MoveTowards(NameBoxA.alpha, targetAlpha, 0.3f);
                    yield return null;
                }
                NameBoxA.alpha = targetAlpha;
                break;
            case "B":
                TachieB.CrossFadeAlpha(targetAlpha == 1f ? 1f : 0f, 0.3f, true);
                while (!Mathf.Approximately(NameBoxB.alpha, targetAlpha))
                {
                    NameBoxB.alpha = Mathf.MoveTowards(NameBoxB.alpha, targetAlpha, 0.3f);
                    yield return null;
                }
                NameBoxB.alpha = targetAlpha;
                break;
        }
    }

    public static void FadeLayer(float targetAlpha)
    {
        mSelf.StartCoroutine(mSelf.FadeLayerCoroutine(targetAlpha));
    }

    IEnumerator FadeLayerCoroutine(float targetAlpha)
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

