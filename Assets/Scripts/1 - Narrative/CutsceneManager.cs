using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float typingSpeed;

    [Header("Fade Settings")]
    [SerializeField] float[] sceneFadeDurations;
    [SerializeField] float[] textFadeDurations;

    [Header("Scene Duration Settings")]
    [SerializeField] float[] sceneDurations;

    [Header("Prologue Cutscene References")]
    [SerializeField] DialogueScript prologueDialogueScript;

    [Header("Inspector References")]
    [SerializeField] Image cutsceneImage;
    [SerializeField] Image fadeImage;
    [SerializeField] Image airplaneImage;
    [SerializeField] Vector3 airplaneEndPos;

    [SerializeField] TextMeshProUGUI cutsceneText1;
    [SerializeField] TextMeshProUGUI cutsceneText2;

    // for fading text
    bool useFirstText = true;

    float time = 0;
    int slide;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameState = "Cutscene";

        if (!GameManager.instance.playedPrologueCutscene)
        {
            StartPrologueCutscene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // print(slide + " " + Mathf.RoundToInt(time));
    }

    public void StartPrologueCutscene()
    {
        StartCoroutine(PrologueCoroutine());
    }

    IEnumerator PrologueCoroutine()
    {
        int imageIndex = 0;

        yield return new WaitForSeconds(2);

        // every scene will take sceneDurations[i] seconds + fade out duration every other scene for changing image
        for (int i = 0; i < prologueDialogueScript.sentences.Length; i++)
        {
            // change image and fade it in if needed
            if (prologueDialogueScript.envTags[i][0] == 'C')
            {
                SceneFade(true, sceneFadeDurations[i]);
                cutsceneImage.sprite = prologueDialogueScript.envImages[imageIndex];

                if (imageIndex < prologueDialogueScript.envImages.Length - 1)
                    imageIndex++;
            }

            // always fade in text
            TextFadeIn(textFadeDurations[i]);
            if (useFirstText)
            {
                cutsceneText1.text = prologueDialogueScript.sentences[i];
                cutsceneText2.text = "";
                useFirstText = false;
            }
            else
            {
                cutsceneText1.text = "";
                cutsceneText2.text = prologueDialogueScript.sentences[i];
                useFirstText = true;
            }

            yield return new WaitForSeconds(sceneDurations[i]);

            // always fade out text
            TextFadeOut(textFadeDurations[i]);

            // fade out scene if needed to fade in a new image
            if (prologueDialogueScript.envTags[i][0] == 'K')
            {
                if (i == 3)
                    AirplaneTransition();

                SceneFade(false, sceneFadeDurations[i]);

                yield return new WaitForSeconds(sceneFadeDurations[i]);
            }
            slide++;
        }

        yield return new WaitForSeconds(2);

        GameManager.instance.playedPrologueCutscene = true;

        SceneManager.LoadScene(2);
    }

    void SceneFade(bool fadeIn, float fadeDuration)
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

    void TextFadeIn(float fadeDuration)
    {
        DOTween.Kill(cutsceneText1);
        DOTween.Kill(cutsceneText2);

        if (useFirstText)
        {
            cutsceneText1.DOFade(1, fadeDuration);
            cutsceneText2.DOFade(0, fadeDuration);
        }
        else
        {
            cutsceneText1.DOFade(0, fadeDuration);
            cutsceneText2.DOFade(1, fadeDuration);
        }
    }

    void TextFadeOut(float fadeDuration)
    {
        DOTween.Kill(cutsceneText1);
        DOTween.Kill(cutsceneText2);

        cutsceneText1.DOFade(0, fadeDuration);
        cutsceneText2.DOFade(0, fadeDuration);
    }

    void AirplaneTransition()
    {
        airplaneImage.gameObject.SetActive(true);

        airplaneImage.transform.DOLocalMove(airplaneEndPos, 0.35f);
    }
}
