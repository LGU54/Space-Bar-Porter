using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpaceController: MonoBehaviour
{
    static bool jump = false;
    static bool reverseGravity = false;
    static bool flash = false;
    float jumpForce = 100;
    Flash flashAction;
    static int spaceCount;
    Animator animator;
    bool StartJump;
    private void Awake()
    {
        flashAction = GetComponent<Flash>();
        animator = GetComponent<Animator>();
    }
    public void SetSpaceCount(int count)
    {
        spaceCount = count;
    }

    public void SetReverseGravity(bool rg)
    {
        reverseGravity = rg;
    }

    public void SetFlash(bool f)
    {
        flash = f;
    }


    public int GetSpaceCount()
    {
        return spaceCount;
    }
    public bool GetReverseGravity()
    {
        return reverseGravity;
    }
    public bool GetFlash()
    {
        return flash;
    }

    private static playerSpaceController spaceController;
    public static playerSpaceController SpaceController
    {
        get
        {
            if (spaceController == null)
            {
                spaceController =  new playerSpaceController();
            }
            return spaceController;
        }
    }

    private enum Actions{
        jump, 
        reverseGravity,
        flash
    }

    public void checkStat(int input)
    {
        if (input == 0)
        {
            jump = true;
            reverseGravity = false;
            flash = false;
            //Debug.Log("J");
        }
        else if (input == 1)
        {
            jump = false;
            reverseGravity = true;
            flash = false;
            //Debug.Log("R");
        }
        else if (input == 2)
        {
            jump = false;
            reverseGravity = false;
            flash = true;
            //Debug.Log("F");
        }else if (input == -1)
        {
            jump = false;
            reverseGravity = false;
            flash = false;
        }
    }
    public void startJump()
    {
        animator.SetTrigger("startJump");
    }

    public void Fall()
    {
        animator.SetTrigger("isFall");
    }

    public void CheckFall()
    {
        if (StartJump && transform.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            animator.SetTrigger("isFall");
        }
    }

    public void SpaceAction(Rigidbody2D player)
    {
        //space = Data.SearchListAndOutput("SpaceData");
        Vector2 yVelocity = new Vector2(0, 100);
        if (spaceCount <= 0)
        {
            return;
        }
        if(reverseGravity == false && flash == false && jump == true)
        {
            if (player.GetComponent<Rigidbody2D>().gravityScale < 0) { player.AddForce(yVelocity * -jumpForce); }
            else { player.AddForce(yVelocity * jumpForce); }
            animator.SetTrigger("startJump");
            StartJump = true;
            // JumpSFX.Post(gameObject);
            //Debug.Log("jump");
            spaceCount -= 1;
        }else if (reverseGravity == true && flash == false && jump == false)
        {
            //Debug.Log("Reverse");
            player.gravityScale *= -1;
            spaceCount -= 1;
            transform.Rotate(new Vector3(0, 0, 1), 180);
            transform.Rotate(new Vector3(0, 1, 0), 180);
            // if (player.gravityScale  < 0)
            // {
            //     GravityUpSFX.Post(gameObject);
            // }
            // else if (player.gravityScale > 0)
            // {
            //     GravityDownSFX.Post(gameObject);
            // }
        }
        else if (reverseGravity == false && flash == true && jump == false)
        {
            //Debug.Log("flash");
            if (flashAction.flash(player)) 
            { spaceCount -= 1;}
            // DashSFX.Post(gameObject);
        }
        else
        {
            Debug.Log("error");
        }
        //SetSpaceCount(space - 1);
    }
    private void FixedUpdate()
    {
        // Debug.Log(spaceCount);
    }
}


