
using UnityEngine;

public class PriceCheck : MonoBehaviour
{
    public static bool Check(int cost)
    {
        if (Inventory.currentCoins >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
}
