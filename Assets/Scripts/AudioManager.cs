using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] AudioSource battleMusicSource;
    [SerializeField] AudioSource sfxSource;

    private bool isPlaying;
    private bool muted;
    private float delay;

    private const float delayTick = 0.05f;

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
        muted = PlayerPrefs.GetInt("MUTED") == 1;

        if (muted)
        {
            AudioListener.pause = true;
        }
    }

    public void ToggleMute()
    {
        muted = !muted;

        if (muted)
        {
            PlayerPrefs.SetInt("MUTED", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MUTED", 0);
        }

        if (muted)
        {
            AudioListener.pause = true;
        }
    }

    public static void PlayBattleMusic()
    {
        instance.battleMusicSource.Play();
        // instance.delay = 1;
        // instance.isPlaying = true;
        // instance.StartCoroutine(instance.BattleSound());
    }

    public static void StopBattleMusic()
    {
        instance.isPlaying = false;
        instance.StopCoroutine(instance.BattleSound());
    }

    public static void PlaySoundEffect(AudioClip clip)
    {
        if (!instance.muted)
        {
            instance.sfxSource.PlayOneShot(clip);
        }
    }

    public static void UpdateBattleMusicDelay(int i)
    {
        float delayTime = i * delayTick;

        if (delayTime < 0.2f)
        {
            delayTime = 0.2f;
        }

        if (delayTime > 1)
        {
            delayTime = 1;
        }

        instance.delay = delayTime;
    }

    private IEnumerator BattleSound()
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(delay);
            battleMusicSource.Play();
        }
    }
}
