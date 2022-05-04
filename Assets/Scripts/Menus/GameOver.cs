using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField] float timer;
    private bool isRunning;

    public static bool Retry()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (PriceCheck.Check(100))
        {
            Inventory.currentCoins -= 100;
            player.shipStats.currentHealth = player.shipStats.maxHealth;
            player.shipStats.currentLives = 1;
            UIManager.SetHealthbar(player.shipStats.currentHealth);
            UIManager.SetLives(player.shipStats.currentLives);

            SaveManager.SaveProgress();

            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnEnable()
    {
        timer = 10;
        timerText.text = timer.ToString("0.00");
        isRunning = true;
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (isRunning)
        {
            timer -= Time.unscaledDeltaTime;
            timerText.text = timer.ToString("0.00");

            if (timer <= 0)
            {
                timerText.text = "0.00";
                MenuManager.GameOverToMainMenu();
                isRunning = false;
            }

        }
    }
}
