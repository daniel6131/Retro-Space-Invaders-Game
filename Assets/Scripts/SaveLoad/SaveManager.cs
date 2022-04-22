using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    // Ensure only one instance of the savemanager is running
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
    }

    // Upon starting, load any exisitng save progress
    private void Start()
    {
        LoadProgress();
    }

    // Save current game progress to a saveobject 
    public static void SaveProgress()
    {
        SaveObject so = new SaveObject();

        so.coins = Inventory.currentCoins;
        so.highscore = UIManager.GetHighscore();
        so.shipStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shipStats;
    }

    // Load in save object from saveload script, set the game states to match that of the save
    public static void LoadProgress()
    {
        SaveObject so = SaveLoad.LoadState();

        Inventory.currentCoins = so.coins;
        UIManager.SetHighscore(so.highscore);

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shipStats = so.shipStats;
    }
}
