using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Reference to the player's ship stats
    public ShipStats shipStats;
    
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip healthSFX;
    [SerializeField] private AudioClip lifeSFX;
    [SerializeField] private AudioClip coinSFX;
    [SerializeField] private AudioClip damageSFX;
    
    [SerializeField] private FlashEffect flashEffect;

    public Projectile laserPrefab;
    public System.Action<bool> killed;
    private bool _laserActive;

    private Vector2 offScreenPos = new Vector2(0, 20f);
    private Vector2 startPos = new Vector2(0, -13f);

    // Bool representing state of initial game killing grace period
    private bool _grace = true;

    // Set the ship's health and lives accoridng to the maximum the player gives
    public void StartSpawn()
    {
        shipStats.currentHealth = shipStats.maxHealth;
        shipStats.currentLives = shipStats.maxLives;

        UIManager.SetHealthbar (shipStats.currentHealth);
        UIManager.SetLives(shipStats.currentLives);

        StartCoroutine(SpawningGrace());

    }

    public void FreezeShip()
    {
        _grace = true;
    }

    // When the game starts, a grace period begins to stopm player from killing invaders too soon
    public IEnumerator SpawningGrace()
    {
        _grace = true;
        yield return new WaitForSeconds(3.5f);
        _grace = false;
    }

    private void Update()
    {
        if (!_grace) 
        {
            // Retrieving the current player position to perform operations om
            Vector2 position = transform.position;
            // Setting the boundaries for the players left and right movement
            Vector2 leftEdge = Camera.main.ViewportToWorldPoint(Vector2.zero);
            Vector2 rightEdge = Camera.main.ViewportToWorldPoint(Vector2.right);
            
            // Transform position to left/right if a left/right input is receieved from the user
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
            {
                position.x -= this.shipStats.shipSpeed * Time.deltaTime;
            } 
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
            {
                position.x += this.shipStats.shipSpeed * Time.deltaTime;
            }

            // If the player is past the world-space view, then move to the opposite side
            if (position.x < leftEdge.x - 1.0f) 
            {
                position.x = rightEdge.x + 1.0f;
            } 
            else if (position.x > rightEdge.x + 1.0f) 
            {
                position.x = leftEdge.x - 1.0f;
            }

            // Update player position with the operations performed
            transform.position = position;

            // Call function to shoot if space key is pressed or left mouse button is clicked
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && position.y == -13) 
            {
                StartCoroutine(Shoot());
            }
        }
    }

    public void AddHealth()
    {
        AudioManager.PlaySoundEffect(healthSFX);
        if (shipStats.currentHealth == shipStats.maxHealth)
        {
            UIManager.SetScore(50);
        }
        else
        {
            shipStats.currentHealth++;
            UIManager.SetHealthbar(shipStats.currentHealth);
        }
    }

    public void AddLife()
    {
        AudioManager.PlaySoundEffect(lifeSFX);
        if (shipStats.currentLives == shipStats.maxLives)
        {
            UIManager. SetScore(100);
        }
        else
        {
            shipStats.currentLives++;
            UIManager.SetLives(shipStats.currentLives);
        }
    }

    public void PickupSpeedBoost()
    {
        StartCoroutine(flashEffect.EffectAnim(10, 2));
        StartCoroutine(BoostSpeed());
    }

    private IEnumerator BoostSpeed()
    {
        float initalSpeed = shipStats.shipSpeed;
        shipStats.shipSpeed = initalSpeed + 5f;
        yield return new WaitForSeconds(6f);
        shipStats.shipSpeed = initalSpeed;
    }

    public void PickupFireRateBoost()
    {
        StartCoroutine(flashEffect.EffectAnim(10, 3));
        StartCoroutine(BoostFireRate());
    }

    private IEnumerator BoostFireRate()
    {
        float initalFireRate = shipStats.fireRate;
        shipStats.fireRate = initalFireRate - 0.5f;
        yield return new WaitForSeconds(6f);
        shipStats.fireRate = initalFireRate;
    }

    public void AddCoin()
    {
        AudioManager.PlaySoundEffect(coinSFX);
    }

    private IEnumerator Shoot()
    {
        // Only shoot if there is currently not an active laser on the game
        if (!_laserActive)
        {
            _laserActive = true;
            // Create laser
            Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            AudioManager.PlaySoundEffect(shootSFX);
            // Only return according to the ships preset firerate
            yield return new WaitForSeconds(shipStats.fireRate);
            _laserActive = false;
        }
    }

    // Handles decreasing the health of a player's ship when it has collided with a missile
    private void TakeDamage()
    {
        AudioManager.PlaySoundEffect(damageSFX);
        shipStats.currentHealth--;
        UIManager.SetHealthbar(shipStats.currentHealth);

        // If the ship has no remaining health
        if(shipStats.currentHealth <= 0)
        {
            transform.position = offScreenPos;
            // Invoke the kill system action killed
            killed(false);
            _laserActive = false;
        } else
        {
            flashEffect.Flash(0);
        }
    }

    public void ResetPlayerPosition()
    {
        transform.position = startPos;
    }

    public void HidePlayerPosition()
    {
        transform.position = offScreenPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking if the player collides with an invader or a missile
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) 
        {
            if (killed != null) 
            {
                // Invoke the kill system action killed
                killed(true);
            }
        } 
        else if (other.gameObject.layer == LayerMask.NameToLayer("Missile")) 
        {
            TakeDamage();
        }
    }
}
