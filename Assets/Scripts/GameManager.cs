using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    // Providing variables for the different types of game objects to be managed
    private Player player;
    private Bunker[] bunkers;
    private Invaders invaders;
    private MysteryShip mysteryShip;

    private bool roundRespawn = false;

    public int countdownTime;
    [SerializeField] Text countdownDisplay;

    // Each time the scene instantiates:
    private void Start()
    {
        // The killed variables for each are reset
        player.killed += OnPlayerKilled;
        invaders.killed += OnInvaderKilled;
        mysteryShip.killed += OnMysteryShipKilled;
        // The method new game is called to reset and restart a game
        NewGame();
    }

    // Countsdown from 3 to 0 to notify the user when they can start playing
    private IEnumerator CountdownToStart()
    {
        countdownDisplay.gameObject.SetActive(true);
        countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO";

        yield return new WaitForSeconds(0.4f);
        countdownDisplay.gameObject.SetActive(false);
    }

    // Method constantly checks to see;
    private void Update()
    {
        // Whether the player has run out of lives and has chosen to start a new game
        if (player.shipStats.currentLives <= 0 && Input.GetKeyDown(KeyCode.Return)) 
        {
            // If this criteria is fulfilled, then a new game can be started
            NewGame();
        }
    }

    // Upon loading obtain the game objects for all game assets
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bunkers = FindObjectsOfType<Bunker>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
    }

    // Method that is called when a fresh/new instance of a game needs to occur
    private void NewGame()
    {
        gameOverUI.SetActive(false);
        UIManager.SetScore(0);
        UIManager.SetLives(player.shipStats.maxLives);

        NewRound();
    }

    // This resets the invaders grid and bunkers back to default and calls the respawn method
    private void NewRound()
    {
        StartCoroutine(CountdownToStart());
        // Increment current wave
        UIManager.SetWave();

        for (int i = 0; i < bunkers.Length; i++) 
        {
            bunkers[i].ResetBunker();
        }

        // Calls the ResetInvaders method to reposition invaders, and set the invaders grid to be visible
        StartCoroutine(invaders.ResetInvaders());
        StartCoroutine(mysteryShip.SpawningGrace());
        invaders.gameObject.SetActive(true);

        Respawn();
    }

    // Reset the player back to the default position after dying
    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.shipStats.currentHealth = player.shipStats.maxHealth;
        UIManager.SetHealthbar(player.shipStats.currentHealth);
        player.gameObject.SetActive(true);
        if (!roundRespawn) {
            StartCoroutine(player.SpawningGrace());
        }
        roundRespawn = false;
    }

    // This method controls the behaviour when the player has lost all lives;
    private void GameOver()
    {
        // The gameover UI is displayed with a retry message
        gameOverUI.SetActive(true);
        // Invaders grid is removed from the scene
        invaders.gameObject.SetActive(false);
    }

    // This method controls the game behavour when the player has been killed:
    private void OnPlayerKilled()
    {
        player.gameObject.SetActive(false);

        player.shipStats.currentLives--;
        UIManager.SetLives(player.shipStats.currentLives);

        roundRespawn = true;

        // If the player still has sufficient lives
        if (player.shipStats.currentLives > 0) 
        {
            // // Start a new round
            // Invoke(nameof(NewRound), 1f);
            Invoke(nameof(Respawn), 1f);
        } 
        else 
        {
            GameOver();
        }
    }

    // When an invader is killed, increment the player's score based on the assigned score for that invader
    private void OnInvaderKilled(Invader invader)
    {
        UIManager.SetScore(invader.score);

        // If all invaders are killed, then a new round can begin
        if (invaders.invadersKilled == invaders.totalInvaders) 
        {
            NewRound();
        }
    }

     // When the mystery ship is killed, update the players score accordingly
    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        UIManager.SetScore(mysteryShip.score);
    }
}
