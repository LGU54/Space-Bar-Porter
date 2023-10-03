using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckStatus : MonoBehaviour
{
    Collider2D playerCollider;
    object area;
    bool isEnd = true;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Debug.Log(obj);
        BaseAreaCollider areaCollider = obj.GetComponent<BaseAreaCollider>();
        AreaType areaType = areaCollider.GetAreaType();
        check(areaCollider.GetAreaType());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Data.SearchListAndReplace("check", -1);
        //Debug.Log(Data.SearchListAndOutput("check"));
        Verify();
    }
    */
    private void Verify()
    {
        bool verify = Physics2D.OverlapBox(new Vector2(transform.position.x + 0.2F, transform.position.y), new Vector2(0.1F, 0.1F), 0, LayerMask.GetMask("area"));
        if (verify)
        {
            Collider2D aCollider = Physics2D.OverlapBox(new Vector2(transform.position.x + 0.2F, transform.position.y), new Vector2(0.1F, 0.1F), 0, LayerMask.GetMask("area"));
            BaseAreaCollider areaCollider = aCollider.gameObject.GetComponent<BaseAreaCollider>();
            check(areaCollider.GetAreaType());
        }
        else if(!verify && Data.SearchListAndOutput("check") != -1)
        {
            Data.SearchListAndReplace("check", -1);
            InGameUIController.SetSkillActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (isEnd)
        {
            Verify();
        }
    }
    public void check(AreaType type)
    {
        switch (type)
        {
            case AreaType.JumpArea:
                Data.SearchListAndReplace("check", 0);
                break;
            case AreaType.ReverseGravityArea:
                Data.SearchListAndReplace("check", 1);
                break;
            case AreaType.FlashArea:
                Data.SearchListAndReplace("check", 2);
                break;
            case AreaType.EndArea:
                isEnd = false;
                StartCoroutine(OnEnd());
                break;
            default:
                break;
        }
        InGameUIController.SetSkillActive(true);
        if (Data.SearchListAndOutput("check") <= 2)
        {
            InGameUIController.SetSkill(type);
        }
    }

    IEnumerator OnEnd()
    {
        DialogFSM.GetInstance().Context.ReadAfterLines();
        DialogFSM.NextLine();

        yield return new WaitUntil(() => !DialogFSM.IsDisplaying);

        SceneEnd.EnterNext();
    }
}
