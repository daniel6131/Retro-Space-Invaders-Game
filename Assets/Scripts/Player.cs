using UnityEngine;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    // Setting player speed
    private float speed = 10f;
    public System.Action killed;
    public bool _laserActive { get; private set; }

    private void Update()
    {
        // Retrieving the current player position to perform operations om
        Vector3 position = transform.position;
        // Setting the boundaries for the players left and right movement
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        
        // Transform position to left/right if a left/right input is receieved from the user
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += this.speed * Time.deltaTime;
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
            Shoot();
        }

    }

    private void Shoot()
    {
        // Only shoot if there is currently not an active laser on the game
        if (!_laserActive) 
        {
            // Create laser
            Projectile laser = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            // Subscribe to destroyed event in order to call a function whenever it occurs
            laser.destroyed += OnLaserDestroyed;
            _laserActive = true;
        }
       
    }

    private void OnLaserDestroyed(Projectile laser)
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking if the player collides with an invader or a missile
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
                if (killed != null) {
                    killed.Invoke();
                }
            }
    }
   
}
