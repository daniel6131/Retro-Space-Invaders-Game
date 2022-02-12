using UnityEngine;

public class Invader : MonoBehaviour
{
    // Array of sprites to cycle between
    public Sprite[] animationSprites;
    // How often the animation cycles to the next sprite
    public float animationTime = 1.0f;
    // Reference to change which sprite is being rendered
    private SpriteRenderer _spriteRenderer;
    // Reference to currently used sprite
    private int _animationFrame;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Calls animation sprite every x amount of seconds
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        // Check our current frame does not exceed the amount of sprites provided
        if (_animationFrame >= this.animationSprites.Length) {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }
}