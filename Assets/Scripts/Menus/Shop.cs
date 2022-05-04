using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class Shop : MonoBehaviour
{
    [SerializeField] AudioClip noSale;
    [SerializeField] AudioClip sale;

    public TextMeshProUGUI currentCoins;
    public TextMeshProUGUI healthValues;
    public TextMeshProUGUI fireRateValues;
    public TextMeshProUGUI healthCost;
    public TextMeshProUGUI fireRateCost;

    public Button healthButton;
    public Button fireRateButton;

    private int currentMaxHealth;
    private float currentFireRate;

    private int nextHealthCost;
    private int nextFireRateCost;

    private int maxHealthMultiplier = 5;
    private int fireRateMultiplier = 5;

    private int maxHealthBaseCost = 10;
    private int fireRateBaseCost = 5;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentMaxHealth = player.shipStats.maxHealth;
        currentFireRate = player.shipStats.fireRate;
        currentCoins.text = Inventory.currentCoins + "C";

        UpdateUIAndTotals();
    }

    private void UpdateUIAndTotals()
    {
        if (currentMaxHealth < 5)
        {
            nextHealthCost = currentMaxHealth * (maxHealthBaseCost * maxHealthMultiplier);
            healthValues.text = currentMaxHealth + " => " + (currentMaxHealth + 1);
            healthCost.text = nextHealthCost + "C";
            healthButton.interactable = true;
        }
        else
        {
            healthCost.text = "MAX";
            healthValues.text = currentMaxHealth.ToString();
            healthButton.interactable = false;
        }

        if (currentFireRate > 0.2)
        {
            nextFireRateCost = 0;

            for (float f = 1; f > 0.2f; f -= 0.1f)
            {
                nextFireRateCost += (fireRateBaseCost * fireRateMultiplier);

                if (f <= currentFireRate)
                {
                    break;
                }
            }
            
            fireRateValues.text = currentFireRate.ToString("0.00") + " => " + (currentFireRate - 0.1f).ToString("0.00");
            fireRateCost.text = nextFireRateCost + "C";
            fireRateButton.interactable = true;
        }
        else
        {
            fireRateCost.text = "MAX";
            fireRateValues.text = "0.20";
            fireRateButton.interactable = false;
        }
    }

    public void BuyHealth()
    {
        if (PriceCheck(nextHealthCost))
        {
            Inventory.currentCoins -= nextHealthCost;
            currentCoins.text = Inventory.currentCoins + "G";

            player.shipStats.maxHealth++;
            currentMaxHealth = player.shipStats.maxHealth;

            SaveManager.SaveProgress();
            UpdateUIAndTotals();

            AudioManager.PlaySoundEffect(sale);
        }
        else
        {
            AudioManager.PlaySoundEffect(noSale);
        }
    }

    public void BuyFireRate()
    {
        if (PriceCheck(nextFireRateCost))
        {
            Inventory.currentCoins -= nextFireRateCost;
            currentCoins.text = Inventory.currentCoins + "G";

            player.shipStats.fireRate++;
            currentFireRate = player.shipStats.fireRate;

            SaveManager.SaveProgress();
            UpdateUIAndTotals();

            AudioManager.PlaySoundEffect(sale);
        }
        else
        {
            AudioManager.PlaySoundEffect(noSale);
        }
    }

    private bool PriceCheck(int cost)
    {
        if (Inventory.currentCoins >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Cheats/Add Gold")]
    private static void AddGoldCheat()
    {
        Inventory.currentCoins += 1000;
    }
#endif
}
