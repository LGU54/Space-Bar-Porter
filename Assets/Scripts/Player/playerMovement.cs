using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D player;
    public float moveSpeed;
    //float xVelocity;
    Vector2 moveinput;
    SpriteRenderer SpriteRenderer;
    playerSpaceController spaceController;
    CanvasGroup menu;
    Animator animator;
    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Data.SearchListAndReplace("SpaceCount", 100);
        spaceController = GetComponent<playerSpaceController>();
        menu = GameObject.Find("MenuPanel").GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
        if (moveinput.x != 0) { animator.SetBool("isWalking", true); }
        if(moveinput.x > 0)
        {
            Data.SearchListAndReplace("isFacingRight", 0);
            SpriteRenderer.flipX = false;
        }
        if(moveinput.x < 0)
        {
            Data.SearchListAndReplace("isFacingRight", 1);
            SpriteRenderer.flipX = true;
        }
        if(moveinput.x == 0)
        { animator.SetBool("isWalking", false); }
    }

    void Update()
    {
        if(player.velocity.y < 0) { animator.SetTrigger("Falling"); }
        if (player.gravityScale < 0)
        {
            SpriteRenderer.flipY = true;
        }
        else
        {
            SpriteRenderer.flipY = false;
        }
        spaceController.CheckFall();
        spaceController.checkStat(Data.SearchListAndOutput("check"));
        player.AddForce(moveinput * moveSpeed * Time.deltaTime * 200);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space is pressed");
            spaceController.SpaceAction(player);
        }
        CanControl();
    }
    

    private void FixedUpdate()
    {
        InGameUIController.UpdateSpaceBarCount(spaceController.GetSpaceCount());
        
        //int n = Data.SearchListAndOutput("check");
        //Debug.Log(n);
    }

    public void CanControl()
    {
        if (ControlSceneManager.IsTransiting || DialogFSM.IsDisplaying || menu.alpha == 1)
        {
            player.simulated = false;
        }
        else
        {
            player.simulated = true;
        }
        // Debug.Log(player.simulated);
        // Debug.Log(ControlSceneManager.IsTransiting);
        // Debug.Log(DialogFSM.IsDisplaying);
        // Debug.Log(menu.alpha == 1);
    }
}
