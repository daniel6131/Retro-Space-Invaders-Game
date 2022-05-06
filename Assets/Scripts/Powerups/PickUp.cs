using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    // Speed at which a pickup will fall
    [SerializeField] private float fallSpeed;

    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
    }

    // PickMeUp method to be accessed and altered by other pickup scripts
    public abstract void PickMeUp();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickMeUp();
        }
    }
}
