using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Invader : MonoBehaviour
{
    public System.Action<Invader> killed;

    // The score awarded for eliminating an invader
    public int score = 10;

    public GameObject explosion;

    // Handling the collisions between a laser and a invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking whether the collision layer is the laser
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            // A delegate to be invoked by another scipt whenever a collision occurs
            killed?.Invoke(this);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
