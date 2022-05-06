using System.Collections;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject shopMenuPanel;
    [SerializeField] private GameObject achievementsMenuPanel;

    public static MenuAnimator instance;

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

    public static IEnumerator CloseMainMenuAnimation(MenuManager menuInstance, bool isShop)
    {
        if (instance.mainMenuPanel != null)
        {
            Animator mainMenuAnimator = instance.mainMenuPanel.GetComponent<Animator>();
            if (mainMenuAnimator != null)
            {
                bool menuIsOpen = mainMenuAnimator.GetBool("close");

                mainMenuAnimator.SetBool("close", !menuIsOpen);
            }
        }

        yield return new WaitForSeconds(0.5f);
        if (isShop)
        {
            menuInstance.shopMenu.SetActive(true);
            instance.StartCoroutine(OpenShopmenuAnimation(menuInstance));
        }
        else
        {
            menuInstance.achievementsMenu.SetActive(true);
            instance.StartCoroutine(OpenAchievementsAnimation(menuInstance));
        }
    }

    public static IEnumerator OpenShopmenuAnimation(MenuManager menuInstance)
    {
        if (instance.shopMenuPanel != null)
        {
            Animator shopMenuAnimator = instance.shopMenuPanel.GetComponent<Animator>();
            if (shopMenuAnimator != null)
            {
                bool shopIsOpen = shopMenuAnimator.GetBool("open");

                shopMenuAnimator.SetBool("open", !shopIsOpen);
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public static IEnumerator OpenAchievementsAnimation(MenuManager menuInstance)
    {
        if (instance.achievementsMenuPanel != null)
        {
            Animator achievementsMenuAnimator = instance.achievementsMenuPanel.GetComponent<Animator>();
            if (achievementsMenuAnimator != null)
            {
                bool achievementIsOpen = achievementsMenuAnimator.GetBool("open");

                achievementsMenuAnimator.SetBool("open", !achievementIsOpen);
            }
        }

        yield return new WaitForSeconds(0.5f);
    }

    public static IEnumerator CloseShopMenuAnimation(MenuManager menuInstance, bool isShop)
    {
        if (instance.shopMenuPanel != null)
        {
            Animator shopMenuAnimator = instance.shopMenuPanel.GetComponent<Animator>();
            if (shopMenuAnimator != null)
            {
                bool shopIsOpen = shopMenuAnimator.GetBool("open");

                shopMenuAnimator.SetBool("open", !shopIsOpen);
            }
        }

        yield return new WaitForSeconds(0.5f);
        menuInstance.mainMenu.SetActive(true);
        instance.StartCoroutine(OpenMainMenuAnimation(menuInstance, isShop));
    }

    public static IEnumerator CloseAchievementsMenuAnimation(MenuManager menuInstance, bool isShop)
    {
        if (instance.achievementsMenuPanel != null)
        {
            Animator achievementsMenuAnimator = instance.achievementsMenuPanel.GetComponent<Animator>();
            if (achievementsMenuAnimator != null)
            {
                bool achievementIsOpen = achievementsMenuAnimator.GetBool("open");

                achievementsMenuAnimator.SetBool("open", !achievementIsOpen);
            }
        }

        menuInstance.achievementsMenu.SetActive(false);
        yield return new WaitForSeconds(0f);
        menuInstance.mainMenu.SetActive(true);
        instance.StartCoroutine(OpenMainMenuAnimation(menuInstance, isShop));
    }

    public static IEnumerator OpenMainMenuAnimation(MenuManager menuInstance, bool isShop)
    {
        if (instance.mainMenuPanel != null)
        {
            Animator mainMenuAnimator = instance.mainMenuPanel.GetComponent<Animator>();
            if (mainMenuAnimator != null)
            {
                bool menuIsOpen = mainMenuAnimator.GetBool("close");

                mainMenuAnimator.SetBool("close", !menuIsOpen);
            }
        }

        yield return new WaitForSeconds(0.5f);
        if (isShop)
        {
            menuInstance.shopMenu.SetActive(false);
        }
        else
        {
            menuInstance.achievementsMenu.SetActive(false);
        }
    }
}
