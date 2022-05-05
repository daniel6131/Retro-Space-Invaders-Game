using UnityEngine;

public class PickUpHealth : PickUp
{
    public override void PickMeUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddHealth();
        Destroy(gameObject);
    }
}
