using UnityEngine.UI;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    private Player player;
    private Invaders invaders;
    private Bunker[] bunkers;

    public GameObject gameOverUI;
    public Text scoreText;
    public Text livesText;

    public int score { get; private set; }
    public int lives { get; private set; }

    // Each time the scene instantiates:
    private void Start()
    {
        // TYhe player killed variable is reset
        player.killed += OnPlayerKilled;
        // The method new game is called to reset and restart a game
        NewGame();
    }

    // Method constantly checks to see;
    private void Update()
    {
        // Whether the player has run out of lives and has chosen to start a new game
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            // If this criteria is fulfilled, then a new game can be started
            NewGame();
        }
    }

    // Upon loading obtain the objects for all game assets
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bunkers = FindObjectsOfType<Bunker>();
        invaders = FindObjectOfType<Invaders>();
    }

    // Method that is called when a fresh/new instance of a game needs to occur
    private void NewGame()
    {
        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    // This resets the invaders grid and bunkers back to default and calls the respawn method
    private void NewRound()
    {
        for (int i = 0; i < bunkers.Length; i++) {
            bunkers[i].ResetBunker();
        }

        invaders.gameObject.SetActive(true);

        Respawn();
    }

    // Reset the player back to the default position after dying
    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    // This method controls the behaviour when the player has lost all lives;
    private void GameOver()
    {
        // The gameover UI is displayed with a retry message
        gameOverUI.SetActive(true);
        // Invaders grid is removed from the scene
        invaders.gameObject.SetActive(false);
    }

    // Method to set the value of the score according to whatever is passed in
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    // Method to set the value of the lives according to whatever is passed in
    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = lives.ToString();
    }

    // This method controls the game behavour when the player has been killed:
    private void OnPlayerKilled()
    {
        // Decrement the number of lives
        SetLives(lives - 1);
        
        player.gameObject.SetActive(false);

        // If the player still has sufficient lives
        if (lives > 0) {
            // Start a new round
            Invoke(nameof(NewRound), 1f);
        } else {
            GameOver();
        }
    }
}
