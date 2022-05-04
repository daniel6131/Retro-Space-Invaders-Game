using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public GameObject shopMenu;
    public GameObject inGameMenu;
    public GameObject pauseMenu;

    [SerializeField] AudioClip mainMenuSFX;
    [SerializeField] AudioClip pauseInSFX;
    [SerializeField] AudioClip pauseOutSFX;

    public static MenuManager instance;

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

    public static void OpenGameOver()
    {
        instance.gameOverMenu.SetActive(true);
        instance.inGameMenu.SetActive(false);
    }

    public void OpenShop()
    {
        GameManager.HideBunkers();
        instance.mainMenu.SetActive(false);
        instance.shopMenu.SetActive(true);
    }

    public void CloseShop()
    {
        instance.mainMenu.SetActive(true);
        instance.shopMenu.SetActive(false);
    }

    public void OpenInGame()
    {
        Time.timeScale = 1;
        instance.mainMenu.SetActive(false);
        instance.pauseMenu.SetActive(false);
        instance.shopMenu.SetActive(false);
        instance.gameOverMenu.SetActive(false);
        instance.inGameMenu.SetActive(true);

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
