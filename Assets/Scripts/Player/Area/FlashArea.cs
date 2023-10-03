using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashArea : MonoBehaviour
{
    private AreaType areaType = AreaType.FlashArea;
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
    }
    public void setAreaType()
    {

    }
}