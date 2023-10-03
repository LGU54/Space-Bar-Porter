using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    Animator animator;
    playerGenerate pg;
    private void Awake()
    {
        pg = GetComponent<playerGenerate>();
        //animator = GetComponent<Animator>();
    }
    private void Start()
    {
        pg.SetPlayer(GameObject.Find("Player"));
        pg.setSpaceBar();
    }

    public void localRG()
    {
        pg.Regenerate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //animator.SetTrigger("Die");
        pg.GetPlayer().GetComponent<Rigidbody2D>().simulated = false;
        Invoke(nameof(localRG), 0.5F);
    }
}
