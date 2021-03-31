using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public string gameState;

    [Header("Narrative Settings")]
    public int currentChapter;
    public int currentStage;
    public Sprite currentStorySprite;
    public Sprite[] storySprites;
    public int storySpriteIndex = -1;

    [Header("Player Settings")]
    public string playerName;
    public int gold;
    public List<AllyUnit> units;

    [Header("Bool Checks")]
    public bool playedPrologueCutscene;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupCharStats();
        SetupNextStorySprite();
    }

    void SetupCharStats()
    {
        for (int i = 0; i < units.Count; i++)
        {
            // units[i].ATK = Random.Range(6, 10);
        }
    }

    public void NextChapter()
    {
        currentChapter++;

        SetupNextStorySprite();
    }

    public void NextStage()
    {
        currentStage++;

        SetupNextStorySprite();
    }

    void SetupNextStorySprite()
    {
        storySpriteIndex++;
        currentStorySprite = storySprites[storySpriteIndex];
    }

    public void UseGold(int g)
    {
        gold -= g;
        if (gold < 0)
            gold = 0;
    }
}
