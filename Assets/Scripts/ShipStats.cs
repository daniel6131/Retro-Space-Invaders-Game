using UnityEngine;

[System.Serializable]
public class ShipStats
{
    // Setting the base stats for a ship which can later be edited
    [Range(1,5)]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int maxLives = 3;
    [HideInInspector]
    public int currentLives = 3;

    public float shipSpeed;
    public float fireRate;
}
