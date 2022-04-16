using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Setting direciton and speed of projectile
    public Vector3 direction;
    public float speed;
    public System.Action<Projectile> destroyed;
    public new BoxCollider2D collider { get; private set; }

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    // Retrieve the boxcollider component for the projectile in question
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        if (destroyed != null) {
            // A delegate to be invoked by another scipt whenever a collision occurs
            destroyed.Invoke(this);
        }
    }

    // Handles what type of collision will occur
    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(collider, transform.position)) {
            // Destory laser object
            Destroy(gameObject);
        }
    }

    // This function will be called whenever the laser collides with anything, 
    // as a result of the box collider component added to the laser prefab
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }
}

