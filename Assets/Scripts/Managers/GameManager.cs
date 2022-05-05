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

    [SerializeField] private int countdownTime;
    [SerializeField] Text countdownDisplay;

    [SerializeField] private AudioClip playerDeathSFX;
    [SerializeField] private AudioClip invaderDeathSFX;
    [SerializeField] private AudioClip mysteryShipSFX;
    [SerializeField] private AudioClip waveCompleteSFX;

    [SerializeField] private FlashEffect flashEffect;

    private static GameManager instance;

    private void Start()
    {
        HideBunkers();
        instance.mysteryShip.gameObject.SetActive(false);
        instance.invaders.gameObject.SetActive(false);
        HideBunkers();
    }

    public static void HideBunkers()
    {
        foreach (Bunker bunker in instance.bunkers)
        {
            bunker.gameObject.SetActive(false);
        }
    }

    // Each time the scene instantiates:
    public static void GameStart()
    {
        // The killed variables for each are reset
        instance.player.killed += instance.OnPlayerKilled;
        instance.invaders.killed += instance.OnInvaderKilled;
        instance.mysteryShip.killed += instance.OnMysteryShipKilled;
        // The method new game is called to reset and restart a game
        instance.NewGame();
    }

    // Countsdown from 3 to 0 to notify the user when they can start playing
    private IEnumerator CountdownToStart()
    {
        AudioManager.UpdateBattleMusicDelay(1);
        AudioManager.StopBattleMusic();
        instance.countdownDisplay.gameObject.SetActive(true);
        instance.countdownTime = 3;
        while (instance.countdownTime > 0)
        {
            instance.countdownDisplay.text = instance.countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            instance.countdownTime--;
        }

        instance.countdownDisplay.text = "GO";

        AudioManager.PlayBattleMusic();

        yield return new WaitForSeconds(0.4f);
        instance.countdownDisplay.gameObject.SetActive(false);
    }

    // Method constantly checks to see;
    private void Update()
    {
        // Whether the player has run out of lives and has chosen to start a new game
        if (instance.player.shipStats.currentLives <= 0 && Input.GetKeyDown(KeyCode.Return)) 
        {
            // If this criteria is fulfilled, then a new game can be started
            instance.NewGame();
        }
    }

    // Upon loading obtain the game objects for all game assets
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        instance.player = FindObjectOfType<Player>();
        instance.bunkers = FindObjectsOfType<Bunker>();
        instance.invaders = FindObjectOfType<Invaders>();
        instance.mysteryShip = FindObjectOfType<MysteryShip>();
    }

    // Method that is called when a fresh/new instance of a game needs to occur
    private void NewGame()
    {
        instance.player.gameObject.SetActive(true);
        instance.gameOverUI.SetActive(false);
        UIManager.SetScore(0);
        UIManager.SetLives(instance.player.shipStats.maxLives);

        NewRound();
    }

    // This resets the invaders grid and bunkers back to default and calls the respawn method
    public static void NewRound()
    {
        instance.player.StartSpawn();
        instance.invaders.StartSpawn();
        instance.mysteryShip.StartSpawn();
        instance.StartCoroutine(instance.CountdownToStart());
        // Increment current wave
        UIManager.SetWave();

        for (int i = 0; i < instance.bunkers.Length; i++) 
        {
            instance.bunkers[i].ResetBunker();
        }

        // Calls the ResetInvaders method to reposition invaders, and set the invaders grid to be visible
        instance.StartCoroutine(instance.invaders.ResetInvaders());
        instance.StartCoroutine(instance.mysteryShip.SpawningGrace());
        instance.invaders.gameObject.SetActive(true);

        instance.Respawn();
    }

    // Reset the player back to the default position after dying
    private void Respawn()
    {
        instance.player.ResetPlayerPosition();
        instance.player.shipStats.currentHealth = instance.player.shipStats.maxHealth;
        UIManager.SetHealthbar(instance.player.shipStats.currentHealth);
        instance.player.gameObject.SetActive(true);
        if (!roundRespawn) {
            instance.StartCoroutine(instance.player.SpawningGrace());
        }
        else
        {
            instance.StartCoroutine(flashEffect.EffectAnim(5, 1));
        }

        instance.roundRespawn = false;
    }

    // This method controls the behaviour when the player has lost all lives;
    private void GameOver()
    {
        instance.player.HidePlayerPosition();
        MenuManager.OpenGameOver();
        instance.mysteryShip.FreezeShip();
        AudioManager.StopBattleMusic();
        // The gameover UI is displayed with a retry message
        instance.gameOverUI.SetActive(true);
        // Invaders grid is removed from the scene
        instance.invaders.gameObject.SetActive(false);
        instance.invaders.CancelInvoke();
        SaveManager.SaveProgress();
    }

    // This method controls the game behavour when the player has been killed:
    private void OnPlayerKilled(bool isInvader)
    {
        AudioManager.PlaySoundEffect(playerDeathSFX);
        instance.player.gameObject.SetActive(false);

        instance.player.shipStats.currentLives--;
        UIManager.SetLives(instance.player.shipStats.currentLives);

        instance.roundRespawn = true;

        // If the player still has sufficient lives
        if (isInvader)
        {
            instance.player.gameObject.SetActive(true);
            instance.GameOver();
        }

        if (instance.player.shipStats.currentLives > 0) 
        {
            instance.Invoke(nameof(Respawn), 1f);
        } 
        else 
        {
            instance.player.gameObject.SetActive(true);
            instance.GameOver();
        }
    }

    // When an invader is killed, increment the player's score based on the assigned score for that invader
    private void OnInvaderKilled(Invader invader)
    {
        AudioManager.PlaySoundEffect(invaderDeathSFX);
        UIManager.SetScore(invader.score);

        AudioManager.UpdateBattleMusicDelay(instance.invaders.invadersAlive);

        // If all invaders are killed, then a new round can begin
        if (instance.invaders.invadersKilled == instance.invaders.totalInvaders) 
        {
            AudioManager.PlaySoundEffect(waveCompleteSFX);
            NewRound();
        }
    }

     // When the mystery ship is killed, update the players score accordingly
    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        AudioManager.PlaySoundEffect(mysteryShipSFX);
        UIManager.SetScore(instance.mysteryShip.score);
    }

    public static void CancelGame()
    {
        instance.StopAllCoroutines();

        if (instance.invaders != null)
        {
            instance.invaders.gameObject.SetActive(false);
        }

        instance.player.FreezeShip();
        instance.mysteryShip.FreezeShip();

        UIManager.ResetUI();
        AudioManager.StopBattleMusic();
    }
}
