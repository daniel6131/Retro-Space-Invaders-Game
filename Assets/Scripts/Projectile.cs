using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Setting direciton and speed of projectile
    public Vector3 direction;
    public float speed;
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    // This function will be called whenever the laser collides with anything, 
    // as a result of the box collider component added to the laser prefab
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.destroyed != null) {
            // A delegate to be invoked by another scipt whenever a collision occurs
            this.destroyed.Invoke();
        }
        // Destory laser object
        Destroy(this.gameObject);
    }
}

