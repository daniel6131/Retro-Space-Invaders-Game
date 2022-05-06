using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsMenu : MonoBehaviour
{
    [SerializeField] private GameObject a1;
    [SerializeField] private GameObject a2;
    [SerializeField] private GameObject a3;
    [SerializeField] private GameObject a4;
    [SerializeField] private GameObject a5;
    [SerializeField] private GameObject a6;
    [SerializeField] private GameObject a7;
    [SerializeField] private GameObject a8;
    [SerializeField] private GameObject a9;

    private Player player;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        List<string> achievements = player.achievements;

        if (achievements.Contains("alien_kill_1"))
        {
            a1.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("alien_kill_50"))
        {
            a2.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("alien_kill_250"))
        {
            a3.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("score_500"))
        {
            a4.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("score_1000"))
        {
            a5.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("score_2000"))
        {
            a6.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("mysteryShip_kill_1"))
        {
            a7.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("mysteryShip_kill_3"))
        {
            a8.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
        if (achievements.Contains("mysteryShip_kill_5"))
        {
            a9.transform.GetChild(3).GetComponent<Text>().text = "UNLOCKED";
        }
    }
}
