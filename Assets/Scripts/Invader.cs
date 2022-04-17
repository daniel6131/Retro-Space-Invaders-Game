using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Invader : MonoBehaviour
{
    // Array of sprites to cycle between
    public Sprite[] animationSprites = new Sprite[0];
    // How often the animation cycles to the next sprite
    public float animationTime = 1.0f;
    public System.Action<Invader> killed;

    // Reference to change which sprite is being rendered
    public SpriteRenderer _spriteRenderer { get; private set; }
    // Reference to currently used sprite
    public int _animationFrame { get; private set; }

    // The score awarded for eliminating an invader
    public int score = 10;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = animationSprites[0];
    }

    private void Start()
    {
        // Calls animation sprite every x amount of seconds
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        // Check our current frame does not exceed the amount of sprites provided
        if (_animationFrame >= animationSprites.Length) {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = animationSprites[_animationFrame];
    }

    // Handling the collisions between a laser and a invader
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Checking whether the collision layer is the laser
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")) {
            // A delegate to be invoked by another scipt whenever a collision occurs
            killed?.Invoke(this);
        }
    }
}
