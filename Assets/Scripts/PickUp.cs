using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    public float fallSpeed;

    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
    }

    public abstract void PickMeUp();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickMeUp();
        }
    }
}
