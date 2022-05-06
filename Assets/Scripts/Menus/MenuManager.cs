using UnityEngine;
using UnityEditor;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject shopMenu;
    public GameObject inGameMenu;
    public GameObject pauseMenu;
    public GameObject achievementsMenu;

    [SerializeField] private AudioClip mainMenuSFX;
    [SerializeField] private AudioClip pauseInSFX;
    [SerializeField] private AudioClip pauseOutSFX;
    [SerializeField] private AudioClip noSale;
    [SerializeField] private AudioClip sale;

    public static MenuManager instance { get; private set; }

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
        ReturnToMainMenu();
    }

    public void OpenMainMenu()
    {
        instance.mainMenu.SetActive(true);
        instance.inGameMenu.SetActive(false);
    }

    [MenuItem("Cheats/OpenGameOver")]
    public static void OpenGameOver()
    {
        Time.timeScale = 0;
        UIManager.HighScoreCheck();

        instance.gameOverMenu.SetActive(true);
        instance.inGameMenu.SetActive(false);
    }

    public static void GameOverToMainMenu()
    {
        Time.timeScale = 1;

        instance.gameOverMenu.SetActive(false);
        instance.shopMenu.SetActive(false);
        instance.pauseMenu.SetActive(false);
        instance.inGameMenu.SetActive(false);
        instance.mainMenu.SetActive(true);
        
        AudioManager.PlaySoundEffect(instance.mainMenuSFX);
        GameManager.CancelGame();
    }

    public static void Retry()
    {
        if (GameOver.Retry())
        {
            AudioManager.PlaySoundEffect(instance.sale);
            GameManager.NewRound();

            Time.timeScale = 1;
            instance.gameOverMenu.SetActive(false);
            instance.inGameMenu.SetActive(true);
        }
        else
        {
            AudioManager.PlaySoundEffect(instance.noSale);
        }
    }

    public void OpenShop()
    {
        instance.StartCoroutine(MenuAnimator.CloseMainMenuAnimation(instance, true));
    }

    public void CloseShop()
    {
        AudioManager.PlaySoundEffect(mainMenuSFX);
        instance.StartCoroutine(MenuAnimator.CloseShopMenuAnimation(instance, true));
    }

    public void OpenAchievements()
    {
        instance.StartCoroutine(MenuAnimator.CloseMainMenuAnimation(instance, false));
    }

    public void CloseAchievements()
    {
        instance.achievementsMenu.SetActive(false);
        AudioManager.PlaySoundEffect(mainMenuSFX);
        instance.StartCoroutine(MenuAnimator.CloseShopMenuAnimation(instance, false));
    }

    public void OpenInGame()
    {
        Time.timeScale = 1;
        instance.mainMenu.SetActive(false);
        instance.pauseMenu.SetActive(false);
        instance.shopMenu.SetActive(false);
        instance.gameOverMenu.SetActive(false);
        instance.inGameMenu.SetActive(true);

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.ResetPlayerPosition();

        GameManager.GameStart();
    }

    public void OpenPause()
    {
        AudioManager.PlaySoundEffect(pauseInSFX);
        Time.timeScale = 0;
        instance.inGameMenu.SetActive(false);
        instance.pauseMenu.SetActive(true);
    }

    public void ClosePause()
    {
        AudioManager.PlaySoundEffect(pauseOutSFX);
        Time.timeScale = 1;
        instance.inGameMenu.SetActive(true);
        instance.pauseMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.PlaySoundEffect(mainMenuSFX);
        Time.timeScale = 1;
        instance.gameOverMenu.SetActive(false);
        instance.shopMenu.SetActive(false);
        instance.pauseMenu.SetActive(false);
        instance.inGameMenu.SetActive(false);
        instance.mainMenu.SetActive(true);

        GameManager.CancelGame();
    }
}
