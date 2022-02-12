using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    // Setting player speed
    public float speed = 5.0f;
    private bool _laserActive;

    private void Update()
    {
        // Transform position to left/right if a left/right input is receieved from the user
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

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
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
    }
   
}
