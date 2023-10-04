using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGenerate : MonoBehaviour
{
    public static GameObject Player;
    public Vector3 initialPosition;
    public int spaceBar;

    public void setSpaceBar()
    {
        Player.GetComponent<playerSpaceController>().SetSpaceCount(spaceBar);
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }

    public GameObject GetPlayer()
    {
        return Player;
    }

    public void Generate()
    {
        if(GameObject.Find("Player") == null) { 
        Instantiate(Player);       
        }
        Player.transform.position = new Vector2(0, 0);
    }

    public void Regenerate()
    {
        Player.transform.position = initialPosition;
        init(spaceBar);
    }

    private bool init(int count)
    {
        bool initComplete = false;
        if (Player != null)
        {
            Player.GetComponent<playerSpaceController>().SetSpaceCount(count);
            Data.DataClear();
            Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
            if(Player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            if(!Player.GetComponent<Rigidbody2D>().simulated)
            {
                Player.GetComponent<Rigidbody2D>().simulated = true;
            }
            if(rb.gravityScale < 0)
            {
                rb.gravityScale *= -1;
                Player.GetComponent<SpriteRenderer>().flipY = !Player.GetComponent<SpriteRenderer>().flipY;
                Player.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = !Player.transform.GetChild(0).GetComponent<SpriteRenderer>().flipY;
            }
            initComplete = true;
            return initComplete;
        }
        else
        {
            Generate();
            init(count);
        }
        return false;
    }
}
