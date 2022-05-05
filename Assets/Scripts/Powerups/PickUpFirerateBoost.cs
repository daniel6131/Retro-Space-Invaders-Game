using UnityEngine;

 public class PickUpFirerateBoost : PickUp
{
    public override void PickMeUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PickupFireRateBoost();
        Destroy(gameObject);
    }
}

