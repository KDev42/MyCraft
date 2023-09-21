using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item Container")]
public class ItemContainer :ScriptableObject
{
    public List<Item> items;
}
