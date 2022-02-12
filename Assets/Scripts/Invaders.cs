using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    // Array of prefabs corresponding to each row in the grid
    public Invader[] prefabs;
    // The size of the invaders grid
    public int rows = 5;
    public int columns = 11;

    public Projectile missilePrefab;
    // The speed at which the invaders move based upon the percentage of invaders killed
    public AnimationCurve speed;
    // The inital direction the invaders move
    private Vector3 _direction = Vector2.right;

    // How often there will be missiles
    public float missileAttackRate = 1.0f;

    public int invadersKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;
    public int invadersAlive => this.totalInvaders - this.invadersKilled;
    public float percentKilled => (float)this.invadersKilled / (float)this.totalInvaders;

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            // Establishing the dimensions of center position for the invader grid
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            // Offsetting each invader based on the row in which they're positioned
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                
                // Offsetting each invader based on the column in which they're positioned
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update()
    {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        // Obtaining the worldspace cooridnates for left and right edges of the game screen
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Looping through each invader to see if it has reached the end of the screen 
        // if so we will flip the direciton
        foreach (Transform invader in this.transform)
        {
            // Checking if the invader is alive / active
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            // Checking if the invader has hit the right edge of the screen
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f)) {
                AdvanceRow();
            } else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f)) {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        // Flipping direction value
        _direction.x *= -1.0f;

        // Moving row position down 1
        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            // Checking if the invader is alive / active
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (Random.value < (1.0f / (float)this.invadersAlive)) {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                // Only one missile should be active, so break here in order to spawn no more
                break;
            }
        }
    }
 
    private void InvaderKilled()
    {
        this.invadersKilled++;

        if (this.invadersKilled >= this.totalInvaders) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
