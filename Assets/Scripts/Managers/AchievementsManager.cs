using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager instance;

    private Queue<string> achievementQueue = new Queue<string>();
    private List<string> achievements;
    private Player player;

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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        achievements = player.achievements;
        StartCoroutine("achievementQueueCheck");
    }

    public void NotifyAchievementComplete(string ID)
    {
        if (!achievements.Contains(ID) && !achievementQueue.Contains(ID))
        {
            achievementQueue.Enqueue(ID);
        }
    }

    private void UnlockAchievement(string ID)
    {
        Achievement a = new Achievement(ID);
        UIManager.instance.ShowAchievement(a.GetTitle());
        achievements.Add(a.GetID());
    }

    private IEnumerator AchievementQueueCheck()
    {
        for (; ; )
        {
            if (achievementQueue.Count > 0)
            {
                UnlockAchievement(achievementQueue.Dequeue());
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
