using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] float initFadeDuration;
    [SerializeField] float endFadeDuration;

    [Header("Inspector References")]
    [SerializeField] Image fadeImage;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    [SerializeField] Color hoverColor;
    [SerializeField] Color defaultColor;

    Image[] startButtonImages;
    Image[] quitButtonImages;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameState = "Main Menu";

        startButtonImages = startButton.GetComponentsInChildren<Image>(); ;
        quitButtonImages = quitButton.GetComponentsInChildren<Image>();

        ScreenFadeIn(false, initFadeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverButton(int index)
    {
        if (index == 0)
        {
            foreach (Image i in startButtonImages)
            {
                i.color = hoverColor;
            }
            foreach (Image i in quitButtonImages)
            {
                i.color = defaultColor;
            }
        }
        else if (index == 1)
        {
            foreach (Image i in quitButtonImages)
            {
                i.color = hoverColor;
            }
            foreach (Image i in startButtonImages)
            {
                i.color = defaultColor;
            }
        }
    }

    public void DefaultButton()
    {
        foreach (Image i in startButtonImages)
        {
            i.color = defaultColor;
        }
        foreach (Image i in quitButtonImages)
        {
            i.color = defaultColor;
        }
    }

    public void StartGame()
    {
        StartCoroutine(SceneChangeCoroutine());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator SceneChangeCoroutine()
    {
        startButton.enabled = false;
        quitButton.enabled = false;

        if (!GameManager.instance.playedPrologueCutscene)
        {
            ScreenFadeIn(true, endFadeDuration);

            yield return new WaitForSeconds(endFadeDuration);

            SceneManager.LoadScene(1);
        }
        else
        {
            ScreenFadeIn(true, endFadeDuration);

            yield return new WaitForSeconds(endFadeDuration);

            SceneManager.LoadScene(2);
        }
    }

    void ScreenFadeIn(bool fadeIn, float fadeDuration)
    {
        DOTween.Kill(fadeImage);

        if (fadeIn)
        {
            fadeImage.DOFade(1, fadeDuration);
        }
        else
        {
            fadeImage.DOFade(0, fadeDuration);
        }
    }
}
