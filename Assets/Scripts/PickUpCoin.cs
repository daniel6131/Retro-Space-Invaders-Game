using UnityEngine;

public class PickUpCoin : PickUp
{
    public override void PickMeUp()
    {
        Inventory.currentCoins++;
        UIManager.SetCoins();
        Destroy(gameObject);
    }
}

