using UnityEngine;

public class PickUpCoin : PickUp
{
    public override void PickMeUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddCoin();
        Inventory.currentCoins += 10;
        UIManager.SetCoins();
        Destroy(gameObject);
    }
}

