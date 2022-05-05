using UnityEngine;

 public class PickUpLife : PickUp
{
    public override void PickMeUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddLife();
        Destroy(gameObject);
    }
}

