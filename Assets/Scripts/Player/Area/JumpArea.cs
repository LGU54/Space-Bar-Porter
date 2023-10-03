using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArea: MonoBehaviour
{
    private AreaType areaType = AreaType.JumpArea;
    private void Awake()
    {
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
