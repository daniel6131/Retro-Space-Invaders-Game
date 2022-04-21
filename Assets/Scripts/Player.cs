using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Reference to the player's ship stats
    public ShipStats shipStats;

    public Projectile laserPrefab;

    public System.Action killed;
    private bool _laserActive;

    // Bool representing state of initial game killing grace period
    private bool grace = true;

    // Set the ship's health and lives accoridng to the maximum the player gives
    private void Start()
    {
        shipStats.currentHealth = shipStats.maxHealth;
        shipStats.currentLives = shipStats.maxLives;

        UIManager.SetHealthbar (shipStats.currentHealth);
        UIManager.SetLives(shipStats.currentLives);

        StartCoroutine(SpawningGrace());

    }

    // When the game starts, a grace period begins to stopm player from killing invaders too soon
    private IEnumerator SpawningGrace()
    {
        yield return new WaitForSeconds(3);
        grace = false;
    }

    private void Update()
    {
        if (!grace) {
            // Retrieving the current player position to perform operations om
            Vector3 position = transform.position;
            // Setting the boundaries for the players left and right movement
            Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
            Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
            
            // Transform position to left/right if a left/right input is receieved from the user
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                position.x -= this.shipStats.shipSpeed * Time.deltaTime;
            } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                position.x += this.shipStats.shipSpeed * Time.deltaTime;
            }

            // If the player is past the world-space view, then move to the opposite side
            if (position.x < leftEdge.x - 1.0f) {
                position.x = rightEdge.x + 1.0f;
            } else if (position.x > rightEdge.x + 1.0f) {
                position.x = leftEdge.x - 1.0f;
            }

            // Update player position with the operations performed
            transform.position = position;

            // Call function to shoot if space key is pressed or left mouse button is clicked
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        // Only shoot if there is currently not an active laser on the game
        if (!_laserActive)
        {
            _laserActive = true;
            // Create laser
            Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            // Only return according to the ships preset firerate
            yield return new WaitForSeconds(shipStats.fireRate);
            _laserActive = false;
        }
    }

    // Handles decreasing the health of a player's ship when it has collided with a missile
    private void TakeDamage()
    {
        shipStats.currentHealth--;
        UIManager.SetHealthbar(shipStats.currentHealth);

        // If the ship has no remaining health
        if(shipStats.currentHealth <= 0)
        {
            // Invoke the kill system action killed
            killed.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking if the player collides with an invader or a missile
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            if (killed != null) {
                // Invoke the kill system action killed
                killed.Invoke();
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
            TakeDamage();
        }
    }
}
