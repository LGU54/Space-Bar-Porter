using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Flash : MonoBehaviour
{
    Vector2 positionPresent;
    Rigidbody2D player;
    static Vector2 targetPoint;
    SpriteRenderer child;
    playerSpaceController controller;
    bool flag = true;
    bool wallCheck, groundCheck = false;
    public float dist = 5;
    Vector3 position;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).GetComponent<SpriteRenderer>();
        controller = GetComponent<playerSpaceController>();
        position = transform.GetChild(0).localPosition;
        //signal = transform.GetChild(0).GetComponent<Signal>();
    }
    private void Start()
    {
        child.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        positionPresent = player.position;
        if (Data.SearchListAndOutput("isFacingRight") == 0)
        {
            targetPoint = new Vector2(positionPresent.x + dist/2, positionPresent.y);
        }
        if(Data.SearchListAndOutput("isFacingRight") == 1)
        {
            targetPoint = new Vector2(positionPresent.x - dist/2, positionPresent.y);
        }

        wallCheck = Physics2D.OverlapBox(targetPoint - new Vector2(0, 0.3F), new Vector2(0.8F, 0.8F), 0, LayerMask.GetMask("ground"));
        //DrawDebugBox(targetPoint - new Vector2(0, 0.3F), 0.8f, 0.8f, Color.yellow);
        if (player.gravityScale < 0)
        {
            groundCheck = Physics2D.OverlapBox(new Vector2(targetPoint.x, targetPoint.y + 0.3F), new Vector2(0.45F, 0.45F), 0, LayerMask.GetMask("ground"));
            //DrawDebugBox(new Vector2(targetPoint.x, targetPoint.y + 0.3F), 0.45f, 0.45f, Color.red);
        }
        else
        {
            groundCheck = Physics2D.OverlapBox(new Vector2(targetPoint.x, targetPoint.y - 1), new Vector2(0.45F, 0.45F), 0, LayerMask.GetMask("ground"));
            //DrawDebugBox(new Vector2(targetPoint.x, targetPoint.y - 1), 0.5f, 0.5f, Color.red);
        }
        // Debug.Log("GroundCheck: " + groundCheck);
        // Debug.Log("WallCheck:   " + wallCheck);
        // Debug.Log("GetFlash():  " + controller.GetFlash());
        //Debug.Log(controller);
        //Debug.Log(controller.GetFlash());
        //Debug.Log(targetPoint);
        //Collider2D ground = Physics2D.OverlapPoint(new Vector2(targetPoint.x, targetPoint.y - 2), LayerMask.GetMask("ground"));
        //Debug.Log(ground);
        if (!wallCheck && groundCheck && controller.GetFlash())
        {
            flag = true;
            child.color = new Color(1, 1, 1, 0.4F);
            
        }
        else
        {
            child.color = new Color(0, 0, 0, 0);
            flag = false;
        }
        ChildTurn(transform.GetChild(0));
    }
    public void ChildTurn(Transform child)
    {
        if (Data.SearchListAndOutput("isFacingRight") == 0)
        {
            child.localPosition = position;
        }
        else
        {
            child.localPosition = new Vector3(-position.x, position.y, position.z);
        }
    }

    public bool flash(Rigidbody2D player)
    {
        if (flag && player != null)
        {
            player.transform.position = targetPoint;
            Debug.Log(targetPoint);
            return true;
        }
        else if (player == null)
        {
            Debug.Log("no player");
            return false;
        }
        return false;
    }
    /*private void DrawDebugBox(Vector2 position, float width, float length, Color color)
    {
        Debug.DrawLine(transform.position, new Vector3(targetPoint.x, targetPoint.y, 0));
        Debug.DrawLine(
            new Vector3(position.x - width / 2, position.y - length / 2, 0),
            new Vector3(position.x - width / 2, position.y + length / 2, 0),
            color);
        Debug.DrawLine(
            new Vector3(position.x - width / 2, position.y + length / 2, 0),
            new Vector3(position.x + width / 2, position.y + length / 2, 0),
            color);
        Debug.DrawLine(
            new Vector3(position.x + width / 2, position.y + length / 2, 0),
            new Vector3(position.x + width / 2, position.y - length / 2, 0),
            color);
        Debug.DrawLine(
            new Vector3(position.x + width / 2, position.y - length / 2, 0),
            new Vector3(position.x - width / 2, position.y - length / 2, 0),
            color);
    }
    */
}
