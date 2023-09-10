using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData 
{
    public static Inventory handInventory;
    public static Inventory mainInventory;

    public static void Initialize()
    {
        handInventory = new Inventory(9);
        mainInventory = new Inventory(27);
    }
}
