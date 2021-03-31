using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float typingSpeed;

    [Header("Dialogue References")]
    [SerializeField] DialogueScript[] ch1Dialogue;
    [SerializeField] DialogueScript[] ch2Dialogue;
    [SerializeField] DialogueScript[] ch3Dialogue;
    DialogueScript[] dialogueToUse;

    [Header("Inspector References")]
    [SerializeField] Image envImageDisplay;
    [SerializeField] Image[] charImage;
    [SerializeField] Image dialogueBoxImage;
    [SerializeField] Image fadeImage;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] TextMeshProUGUI dialogueDisplay;
    [SerializeField] GameObject continueButton;

    int dialogueIndex = 0;
    int imageIndex = -1;

    string oldName = "";
    bool firstChar = true;

    bool animateInChar1;
    float xStartPosChar1;
    bool animateInChar2;
    float xStartPosChar2;

    private void Awake()
    {
        nameDisplay.text = "";
        dialogueDisplay.text = "";

        xStartPosChar1 = charImage[0].transform.position.x;
        xStartPosChar2 = charImage[1].transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetChapterDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && continueButton.activeInHierarchy)
            NextText();

        if (Input.GetKeyDown(KeyCode.V))
        {
            NextChapter();
        }
    }

    void SetChapterDialogue()
    {
        switch (GameManager.instance.currentChapter)
        {
            case 1:
                dialogueToUse = ch1Dialogue;
                break;
            case 2:
                dialogueToUse = ch2Dialogue;
                break;
            case 3:
                dialogueToUse = ch3Dialogue;
                break;
        }

        GameManager.instance.gameState = dialogueToUse[GameManager.instance.currentStage].name;

        fadeImage.DOFade(0, 0.5f);

        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // pause 
        if (dialogueToUse[GameManager.instance.currentStage].envTags[dialogueIndex][1] == 'P')
        {
            charImage[0].DOFade(0, 0.5f);
            charImage[1].DOFade(0, 0.5f);
            dialogueBoxImage.DOFade(0, 0.5f);

            yield return new WaitForSeconds(1);

            charImage[0].enabled = false;
            charImage[1].enabled = false;
            charImage[0].transform.position = new Vector3(xStartPosChar1, charImage[0].transform.position.y, charImage[0].transform.position.z);
            charImage[1].transform.position = new Vector3(xStartPosChar2, charImage[1].transform.position.y, charImage[1].transform.position.z);

            firstChar = true;
            animateInChar1 = false;
            animateInChar2 = false;

            yield return new WaitForSeconds(1);
        }

        dialogueBoxImage.DOFade(1, 0.5f);

        // change cutscene image
        if (dialogueToUse[GameManager.instance.currentStage].envTags[dialogueIndex][0] == 'C')
        {
            imageIndex++;
            envImageDisplay.sprite = dialogueToUse[GameManager.instance.currentStage].envImages[imageIndex];
        }

        // animate char in 1st position first
        if (dialogueToUse[GameManager.instance.currentStage].envTags[dialogueIndex][2] == '1')
        {
            // fade in char image (just in case char in 1st pos is already speaking)
            charImage[0].DOFade(0, 0);

            firstChar = true;
            animateInChar1 = false;
            animateInChar2 = false;

            yield return new WaitForSeconds(0.1f);
        }
        // animate char in 2nd position first
        else if (dialogueToUse[GameManager.instance.currentStage].envTags[dialogueIndex][2] == '2')
        {
            // fade in char image (just in case char in 2nd pos is already speaking)
            charImage[1].DOFade(0, 0);

            firstChar = false;
            animateInChar1 = false;
            animateInChar2 = false;

            yield return new WaitForSeconds(0.1f);
        }

        // display name and char image
        if (dialogueToUse[GameManager.instance.currentStage].names[dialogueIndex] != null)
        {
            // new char introduced or after pause -> animate in char
            if (oldName != dialogueToUse[GameManager.instance.currentStage].names[dialogueIndex] 
                || dialogueToUse[GameManager.instance.currentStage].envTags[dialogueIndex][1] == 'P')
            {
                if (firstChar)
                {
                    if (!animateInChar1)
                    {
                        charImage[0].enabled = true;
                        charImage[0].transform.DOLocalMoveX(-550, 0.35f);

                        animateInChar1 = true;
                    }

                    oldName = dialogueToUse[GameManager.instance.currentStage].names[dialogueIndex];
                    firstChar = false;

                    Char1Talks();
                }
                else
                {
                    if (!animateInChar2)
                    {
                        charImage[1].enabled = true;
                        charImage[1].transform.DOLocalMoveX(550, 0.35f);

                        animateInChar2 = true;
                    }

                    oldName = dialogueToUse[GameManager.instance.currentStage].names[dialogueIndex];
                    firstChar = true;

                    Char2Talks();

                }
            }

            nameDisplay.text = dialogueToUse[GameManager.instance.currentStage].names[dialogueIndex];
        }

        // display text
        foreach (char letter in dialogueToUse[GameManager.instance.currentStage].sentences[dialogueIndex].ToCharArray())
        {
            dialogueDisplay.text += letter;
            yield return new WaitForSeconds(1 / typingSpeed);
        }

        continueButton.SetActive(true);
    }

    void Char1Talks()
    {
        charImage[0].sprite = dialogueToUse[GameManager.instance.currentStage].charImages[dialogueIndex];

        charImage[0].DOColor(new Color32(255, 255, 255, 255), 0.35f);
        charImage[1].DOColor(new Color32(100, 100, 100, 255), 0.35f);
    }

    void Char2Talks()
    {
        charImage[1].sprite = dialogueToUse[GameManager.instance.currentStage].charImages[dialogueIndex];

        charImage[1].DOColor(new Color32(255, 255, 255, 255), 0.35f);
        charImage[0].DOColor(new Color32(100, 100, 100, 255), 0.35f);
    }

    public void NextText()
    {
        nameDisplay.text = "";
        dialogueDisplay.text = "";
        continueButton.SetActive(false);

        if (dialogueIndex < dialogueToUse[GameManager.instance.currentStage].sentences.Length - 1)
        {
            dialogueIndex++;
            StartCoroutine(TypeText());
        }
        else
        {
            StartCoroutine(TransitionToBattlePrep());
        }
    }

    IEnumerator TransitionToBattlePrep()
    {
        fadeImage.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);

        // SceneManager.LoadScene(3);
    }

    // TODO REMOVE THIS
    public void NextChapter()
    {
        GameManager.instance.NextStage();

        SceneManager.LoadScene(2);
    }
}
