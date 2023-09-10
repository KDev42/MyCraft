using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigurationData
{
    public ConfigurationType type;
    public int distance;
    public int padding;
    public int height;
    public bool isTwoDirection;
}

public enum ConfigurationType
{
    none =0,
    cylinder=1,
    cube=2,
}

