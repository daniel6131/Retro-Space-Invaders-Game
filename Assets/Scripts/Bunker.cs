using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bunker : MonoBehaviour
{
    // Array storing all the bunker states
    public Sprite[] states;
    private int health;

    [SerializeField] AudioClip destroySFX;

    public SpriteRenderer spriteRenderer { get; private set; }

    // When the bunker experiences a collision, the behaviour depends on the game object it is colliding with;
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.PlaySoundEffect(destroySFX);
        // If its an invader then the bunker will be instantly destroyed
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            gameObject.SetActive(false);
        // If its a projectile, then it will decrement the bunker's health
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Missile") || 
                    other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            health--;

            if (health <= 0)
            {
                gameObject.SetActive(false);
            } else {
                spriteRenderer.sprite = states[health - 1];
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ResetBunker();
    }

    public void ResetBunker()
    {
        // Reset each bunker health
        health = 4;
        // Each bunker needs a unique instance of the sprite
        spriteRenderer.sprite = states[health - 1];

        gameObject.SetActive(true);
    }

    // Checking if there is a collision with a bunker occuring
    // Returning a bool to determine how to handle the projectile that is calling it
    public bool CheckCollision(Bunker bunker)
    {
        if (bunker != null) {
            return true;
        }
        else {
            return false;
        }
    }
}
