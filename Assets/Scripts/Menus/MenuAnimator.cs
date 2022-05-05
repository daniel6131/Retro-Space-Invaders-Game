using System.Collections;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject shopMenuPanel;

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

    public static IEnumerator CloseMainMenuAnimation(MenuManager menuInstance)
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
        menuInstance.shopMenu.SetActive(true);
        instance.StartCoroutine(OpenShopmenuAnimation(menuInstance));
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

    public static IEnumerator CloseShopMenuAnimation(MenuManager menuInstance)
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
        instance.StartCoroutine(OpenMainMenuAnimation(menuInstance));
    }

    public static IEnumerator OpenMainMenuAnimation(MenuManager menuInstance)
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
        menuInstance.shopMenu.SetActive(false);
    }
}
