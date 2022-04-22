using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Invader : MonoBehaviour
{
    public System.Action<Invader> killed;

    // The score awarded for eliminating an invader
    public int score;

    public GameObject explosion;

    public GameObject coinPrefab;
    public GameObject lifePrefab;
    public GameObject healthPrefab;

    private const int LIFE_CHANCE = 1;
    private const int HEALTH_CHANCE = 10;
    private const int COIN_CHANCE = 50;

    // Handling the collisions between a laser and a invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking whether the collision layer is the laser
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            // A delegate to be invoked by another scipt whenever a collision occurs
            killed?.Invoke(this);
            Instantiate(explosion, transform.position, Quaternion.identity);

            int ran = Random.Range(0, 1000);

            if (ran == LIFE_CHANCE)
            {
                Instantiate(lifePrefab, transform.position, Quaternion.identity);
            }
            else if (ran <= HEALTH_CHANCE)
            {
                Instantiate(healthPrefab, transform.position, Quaternion.identity);
            }
            else if (ran <= COIN_CHANCE)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }

        }
    }
}
