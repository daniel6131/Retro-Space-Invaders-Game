using UnityEngine;

 public class PickUpSpeedBoost : PickUp
{
    public override void PickMeUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().PickupSpeedBoost();
        Destroy(gameObject);
    }
}

