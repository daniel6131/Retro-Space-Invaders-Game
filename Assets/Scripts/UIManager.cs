using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    public TextMeshProUGUI highscoreText;
    private int highscore;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI waveText;
    private int wave;

    // Array to hold the 3 life sprite images
    public Image[] lifeSprites;
    private Color32 active = new Color(1, 1, 1, 1);
    private Color32 inactive = new Color(1, 1, 1, 0.25f);

    // Reference to current health bar
    public Image healthBar;
    // Array to hold all sprites for the different health
    public Sprite[] healthBars;

    // Current instance of the class
    private static UIManager instance;

    // Upon loading obtain the game objects for all game assets
    private void Awake()
    {
        // Ensuring only one instance of the UI manager is running
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Method to set the value of the score according to whatever value is passed in
     public static void SetScore(int score)
    {
        instance.score += score;
        instance.scoreText.text = instance.score.ToString().PadLeft(4, '0');
    }

    // Method to set the value of the lives according to whatever is passed in
     public static void SetLives(int lives)
    {
        // For every image in the array of life sprites, set its colour to represent it being inactive
        foreach (Image i in instance.lifeSprites) 
        {
            i.color = instance.inactive;
        }

        // For the amount of lives that are passed in, set that many life sprites to be active
        for (int i = 0; i < lives; i++)
        {
            instance.lifeSprites[i].color = instance.active;
        }
    }

    // Set the healthbar sprite to be that which represents the health value passed in
    public static void SetHealthbar(int health)
    {
        instance.healthBar.sprite = instance.healthBars[health];
    }

    public static void SetHighscore(int hs)
    {
        if (instance.highscore < hs)
        {
            instance.highscore = hs;
            instance.highscoreText.text = instance.highscore.ToString().PadLeft(4, '0');
        }
    }

    public static void HighScoreCheck()
    {
        if (instance.highscore < instance.score)
        {
            SetHighscore(instance.score);
            SaveManager.SaveProgress();
        }
    }

    public static int GetHighscore()
    {
        return instance.highscore;
    }

    // After a wave has been completed, increment the wave counter 
    public static void SetWave()
    {
        instance.wave++;
        instance.waveText.text = instance.wave.ToString();
    }

    public static void SetCoins()
    {
        instance.coinsText.text = Inventory.currentCoins.ToString().PadLeft(4, '0');
    }

    public static void ResetUI()
    {
        instance.score = 0;
        instance.wave = 0;
        instance.scoreText.text = instance.score.ToString().PadLeft(4, '0');
        instance.waveText.text = instance.wave.ToString();
    }
}
