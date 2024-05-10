using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class FadeInManager : MonoBehaviour
{
    [SerializeField] GameObject loadInScreen;
    [SerializeField] Image loadingImage;

    float fadingNotificationImageValue = 1;

    bool fading;


    //--------------------


    private void Start()
    {
        loadInScreen.SetActive(true);

        StartCoroutine(FadeInStartmenu());
    }
    private void Update()
    {
        if (fading)
        {
            FadingOutLoadingScreen();
        }
    }

    //--------------------


    IEnumerator FadeInStartmenu()
    {
        yield return new WaitForSeconds(0.5f);

        fading = true;
    }

    void FadingOutLoadingScreen()
    {
        fadingNotificationImageValue -= Time.deltaTime * 2;

        loadingImage.color = new Color(loadingImage.color.r, loadingImage.color.g, loadingImage.color.b, fadingNotificationImageValue);
        //loadingImage_Icon.color = new Color(loadingImage_Icon.color.r, loadingImage_Icon.color.g, loadingImage_Icon.color.b, fadingNotificationImageValue * fadingNotificationImageValue_Icon);
        //loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, fadingNotificationImageValue);

        if (fadingNotificationImageValue <= 0)
        {
            loadInScreen.SetActive(false);

            fading = false;
        }
    }
}
