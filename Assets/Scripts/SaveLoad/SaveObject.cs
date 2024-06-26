using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public int coins;
    public int highscore;
    public ShipStats shipStats;

    // Contstructor for creating a saveobject
    public SaveObject()
    {
        coins = 0;
        highscore = 0;

        shipStats = new ShipStats();
        shipStats.maxHealth = 1;
        shipStats.maxLives = 3;
        shipStats.shipSpeed = 10;
        shipStats.fireRate = 1f;
    }
}
 