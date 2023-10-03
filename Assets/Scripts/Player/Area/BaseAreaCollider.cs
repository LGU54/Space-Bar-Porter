using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseAreaCollider: MonoBehaviour
{
    public AreaType areaType;
    private void Awake()
    {
    }
    private void FixedUpdate()
    {
        //Debug.Log(areaType);
    }
    public void SetAreaType(AreaType areaType)
    {
        //this.areaType = areaType;
    }
    public AreaType GetAreaType()
    {
        return areaType;
    }
}

