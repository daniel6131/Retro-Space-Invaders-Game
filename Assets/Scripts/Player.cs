using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    // Setting player speed
    public float speed = 5.0f;
    public System.Action killed;
    private bool _laserActive;

    private void Update()
    {
        // Creating a variable holding the current position of the players movement
        Vector3 position = transform.position;
        // Finding the left and right boundaries of the viewport
        Vector3 minLeftBounds = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 maxRightBounds = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Transform position to left/right if a left/right input is receieved from the user
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }

        // Clamp the position of the user so that their sprite cannot move beyond the boundaries of the screen
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1);
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
            // Creatr projectile
            Projectile projectile = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            // Subscribe to destroyed event in order to call a function whenever it occurs
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }
       
    }

    private void LaserDestroyed()
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
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
    }
   
}
