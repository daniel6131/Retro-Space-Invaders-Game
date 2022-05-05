using UnityEngine;
using System.Collections;

public class MysteryShip : MonoBehaviour
{
    // Initialise the standard variables for the mystery ship
    public float speed = 5f;
    // The rate at which the mystery ship will appear on the scene
    public float cycleTime = 30f;
    public int score = 300;
    public GameObject explosion;
    public System.Action<MysteryShip> killed;

    // The initial direction of the mystery ship
    public int _direction { get; private set; } = -1;
    // A variable to determine whether the mystery ship is in play or not
    public bool _spawned { get; private set; }

    // The end points for the ship to travel to on the right or left side of the scene
    public Vector3 _leftDestination { get; private set; }
    public Vector3 _rightDestination { get; private set; }

    // Bool representing state of initial game killing grace period
    private bool _grace = true;

    public void StartSpawn()
    {
        // Transform the viewport to world coordinates so we can set the mystery
        // ship's destination points to the ends of the scene
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Offset the destination by a unit so the ship is fully out of sight when moving
        Vector3 left = transform.position;
        left.x = leftEdge.x - 1f;
        _leftDestination = left;

        Vector3 right = transform.position;
        right.x = rightEdge.x + 1f;
        _rightDestination = right;

        transform.position = _leftDestination;
        Despawn();

        StartCoroutine(SpawningGrace());
    }

    // When the game starts, a grace period begins to stopm player from killing invaders too soon
    public IEnumerator SpawningGrace()
    {
        _grace = true;
        yield return new WaitForSeconds(3);
        _grace = false;
    }

    public void FreezeShip()
    {
        _grace = true;
    }

    // Constantly checking if the mystery ship is in scene or not
    // and determining whether the ship needs to move from left to right or right to left
    private void Update()
    {
        if (!_grace) 
        {
            if (!_spawned) 
            {
                return;
            }

            if (_direction == 1) 
            {
                MoveRight();
            } 
            else {
                MoveLeft();
            }
        }
    }

    // Move the ship across the scene from left to right
    // If the ship's movement will take it off of the scene, the ship will be despawned
    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= _rightDestination.x) 
        {
            Despawn();
        }
    }

    // Move the ship across the scene from right to left
    // If the ship's movement will take it off of the scene, the ship will be despawned
    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= _leftDestination.x) 
        {
            Despawn();
        }
    }

    // Allow the ship to be put into the scene, and set its direction of movement according to the 
    // side of the scene in which it left
    private void Spawn()
    {
        _direction *= -1;

        if (_direction == 1) 
        {
            transform.position = _leftDestination;
        } 
        else 
        {
            transform.position = _rightDestination;
        }

        _spawned = true;
    }

    // Stop the mystery ship from being able to be put into the scene
    // Set the direction of movement according to the side of the scene in which it left
    private void Despawn()
    {
        _spawned = false;

        if (_direction == 1) 
        {
            transform.position = _rightDestination;
        } 
        else 
        {
            transform.position = _leftDestination;
        }

        // Invoke the behaviour of the spawn method in accordance with the rate of spawn set
        Invoke(nameof(Spawn), cycleTime);
    }

    // Method to handle the behaviour when an object collides with the mystery ship
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the game object colliding with the ship is a laser then despawn and kill the ships instane
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Despawn();

            if (killed != null) 
            {
                killed.Invoke(this);
            }
        }
    }
}
