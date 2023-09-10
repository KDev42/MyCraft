using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DropDownResources 
{
    public ItemType itemType;
    [Range(0.001f,100)]
    public float percent;
    [Range(0, 999)]
    public int minNumber;
    [Range(1, 1000)]
    public int maxNumber;
}
