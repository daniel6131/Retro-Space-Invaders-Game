using UnityEngine;
using System.Collections;

public class Invaders : MonoBehaviour
{
    // The size of the invaders grid
    public int rows = 5;
    public int columns = 11;

    // Array of prefabs corresponding to each row in the grid
    public Invader[] prefabs = new Invader[5];
    // The speed at which the invaders move based upon the percentage of invaders killed
    public AnimationCurve speed = new AnimationCurve();
    // The inital direction the invaders move
    public Vector3 _direction { get; private set; } = Vector3.right;
    // Initial position of the invaders
    public Vector2 _initialPosition { get; private set; } = new Vector2(0,20);
    // Y value which we want the invaders grid to start functioning at
    private const float START_Y = 3.5f;
    public System.Action<Invader> killed;

    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip spawnSFX;

    public Projectile missilePrefab;
    // How often there will be missiles
    public float missileAttackRate = 1.0f;

    // Bool to represent whether the invaders are in the process of entering or not
    private bool entering;
    private bool respawning = false;

    // Initialising values to be used to calculate the invaders speed and missile spawn rate
    // as well as to be used to know when a round is over
    public int invadersKilled { get; private set; }
    public int totalInvaders => rows * columns;
    public int invadersAlive => totalInvaders - invadersKilled;
    public float percentKilled => (float)invadersKilled / (float)totalInvaders;

    private void Awake()
    {
        // Set invaders initial position to default values
        transform.position = _initialPosition;

        for (int row = 0; row < rows; row++)
        {
            // Establishing the dimensions of center position for the invader grid
            float width = 2.0f * (columns - 1);
            float height = 2.0f * (rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            // Offsetting each invader based on the row in which they're positioned
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < columns; col++)
            {
                Invader invader = Instantiate(prefabs[row], transform);
                invader.killed += OnInvaderKilled;
                
                // Offsetting each invader based on the column in which they're positioned
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    public void StartSpawn()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        if (entering) 
        {
            transform.Translate(Vector2.down * Time.deltaTime * 10);

            if (transform.position.y <= START_Y) 
            {
                entering = false;
            }
        } 
        else {
            if (!respawning) 
            {
                // Evaluate speed of the invaders reflective of how many have been killed
                transform.position += _direction * this.speed.Evaluate(percentKilled) * Time.deltaTime;

                // Obtaining the worldspace cooridnates for left and right edges of the game screen
                Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
                Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

                // Looping through each invader to see if it has reached the end of the screen 
                // if so we will flip the direciton
                foreach (Transform invader in transform)
                {
                    // Checking if the invader is alive / active
                    if (!invader.gameObject.activeInHierarchy) {
                        continue;
                    }

                    // Checking if the invader has hit the right edge of the screen
                    if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f)) 
                    {
                        AdvanceRow();
                    } 
                    else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f)) 
                    {
                        AdvanceRow();
                    }
                }
            }
        }
    }

    private void AdvanceRow()
    {
        // Flipping direction value
        _direction = new Vector3(-_direction.x, 0f, 0f);

        // Moving row position down 1
        Vector3 position = transform.position;
        position.y -= 1.0f;
        transform.position = position;
    }

    private void MissileAttack()
    {
        if (!entering && !respawning) 
        {
            foreach (Transform invader in transform)
            {
                // Checking if the invader is alive / active
                if (!invader.gameObject.activeInHierarchy) 
                {
                    continue;
                }

                if (Random.value < (1.0f / (float)invadersAlive)) 
                {
                    Instantiate(missilePrefab, invader.position, Quaternion.identity);
                    AudioManager.PlaySoundEffect(shootSFX);
                    // Only one missile should be active, so break here in order to spawn no more
                    break;
                }
            }
        }
    }
 
    // Remove invader from scene when killed and increment killed value
    private void OnInvaderKilled(Invader invader)
    {
        // Deactivating/turning off the invader game object
        invader.gameObject.SetActive(false);
        invadersKilled++;
        killed(invader);
    }

    // When the game ends, reset the invaders grid to the intial values
    public IEnumerator ResetInvaders()
    {
        respawning = true;

        invadersKilled = 0;
        _direction = Vector3.right;
        transform.position = _initialPosition;

        yield return new WaitForSeconds(2);
        AudioManager.PlaySoundEffect(spawnSFX);

        respawning = false;
        entering = true;

        // For every invader in the grid, reset them to be visible if they are not already
        foreach (Transform invader in transform) 
        {
            invader.gameObject.SetActive(false);
            invader.gameObject.SetActive(true);
        }
    }
}
