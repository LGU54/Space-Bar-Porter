using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravityArea : MonoBehaviour
{
    private AreaType areaType = AreaType.ReverseGravityArea;
    private void Awake()
    {
        Pass();
    }
    private void FixedUpdate()
    {

    }

    public void Pass()
    {
        gameObject.BroadcastMessage("setAreaType", areaType);
        Debug.Log(areaType);
    }
    public void setAreaType()
    {

    }
}