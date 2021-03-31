using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BattlePrepManager : MonoBehaviour
{
    [Header("Battle Prep Settings")]
    [SerializeField] int currentBattlePanelIndex;
    [SerializeField] int currentCharIndex;

    [Header("IR - General")]
    [SerializeField] GameObject[] battlePrepPanels;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Image[] backgroundImage;
    [SerializeField] Image fadeImage;

    [Header("IR - Default Panel")]
    [SerializeField] Image[] defaultUnitImages;

    [SerializeField] Image defaultHoverUnitImage;
    [SerializeField] Image defaultLdrImage;
    [SerializeField] TextMeshProUGUI defaultNameText;
    [SerializeField] TextMeshProUGUI defaultLvlText;
    [SerializeField] TextMeshProUGUI defaultTypeText;

    [Header("IR - Char Unit Panel")]
    [SerializeField] GameObject[] charUnitPanels;

    [SerializeField] Image charUnitImage;
    [SerializeField] Image charLdrImage;
    [SerializeField] Image charTypeImage;
    [SerializeField] TextMeshProUGUI charTypeText;

    [SerializeField] TextMeshProUGUI charNameText;
    [SerializeField] TextMeshProUGUI charLvlText;
    [SerializeField] TextMeshProUGUI charExpText;
    [SerializeField] TextMeshProUGUI charHPText;
    [SerializeField] Slider charHPSlider;
    [SerializeField] TextMeshProUGUI charNRGText;
    [SerializeField] Slider charNRGSlider;
    [SerializeField] TextMeshProUGUI[] charStatsText;

    [SerializeField] TextMeshProUGUI[] charButtonSkillsText;
    [SerializeField] TextMeshProUGUI[] charSkillsText;

    [Header("IR - Upgrade Panel")]
    [SerializeField] TextMeshProUGUI[] upgradeStatsText;
    [SerializeField] TextMeshProUGUI[] upgradeAddStatsText;
    [SerializeField] int[] upgradeCost;
    [SerializeField] int[] upgradeAddCounter;
    [SerializeField] TextMeshProUGUI[] upgradeCostStatsText;

    AllyUnit unit;

    private void Start()
    {
        GameManager.instance.gameState = "Battle Prep";

        SetupBattlePrep();
        ResetUI();
        ShowBattlePanel(0);

        FadeScene(true);
    }

    void FadeScene(bool start)
    {
        if (start)
        {
            fadeImage.DOFade(0, 0.5f);
        }
        else
        {
            fadeImage.DOFade(1, 0.5f);
        }
    }

    public void ResetUI()
    {
        foreach (GameObject b in battlePrepPanels)
        {
            b.SetActive(false);
        }
    }

    void SetupBattlePrep()
    {
        goldText.text = "Gold: " + GameManager.instance.gold.ToString();

        defaultUnitImages[0].sprite = GameManager.instance.units[0].battlePrepSprites[GameManager.instance.units[0].upgradeLvl];
        defaultUnitImages[1].sprite = GameManager.instance.units[1].battlePrepSprites[GameManager.instance.units[1].upgradeLvl];
        defaultUnitImages[2].sprite = GameManager.instance.units[2].battlePrepSprites[GameManager.instance.units[2].upgradeLvl];
        defaultUnitImages[3].sprite = GameManager.instance.units[3].battlePrepSprites[GameManager.instance.units[3].upgradeLvl];
        defaultUnitImages[4].sprite = GameManager.instance.units[4].battlePrepSprites[GameManager.instance.units[4].upgradeLvl];
        defaultUnitImages[5].sprite = GameManager.instance.units[5].battlePrepSprites[GameManager.instance.units[5].upgradeLvl];

        foreach (Image i in backgroundImage)
        {
            i.sprite = GameManager.instance.currentStorySprite;
        }

        HoverCharButton(0);
    }

    public void ShowBattlePanel(int index)
    {
        ResetUI();
        battlePrepPanels[index].SetActive(true);
    }

    public void StartBattle()
    {
        StartCoroutine(TransitionToBattle());
    }

    IEnumerator TransitionToBattle()
    {
        FadeScene(false);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(4);
    }

    // default panel
    public void HoverCharButton(int charIndex)
    {
        unit = GameManager.instance.units[charIndex];

        defaultHoverUnitImage.sprite = unit.battlePrepSprites[unit.upgradeLvl];
        defaultLdrImage.sprite = unit.leaderSprite;
        defaultNameText.text = unit.Name;
        defaultLvlText.text = "Lvl " + unit.level.ToString();
        switch (unit.type)
        {
            case TacticsUnit.Type.Power:
                defaultTypeText.color = Color.red;
                break;
            case TacticsUnit.Type.Speed:
                defaultTypeText.color = Color.blue;
                break;
            case TacticsUnit.Type.Tech:
                defaultTypeText.color = Color.green;
                break;
        }
        defaultTypeText.text = unit.type.ToString();

        currentCharIndex = charIndex;
    }

    // char panel
    public void ShowCharPanel()
    {
        ResetUI();

        unit = GameManager.instance.units[currentCharIndex];

        // lhs
        charUnitImage.sprite = unit.battlePrepSprites[unit.upgradeLvl];
        charLdrImage.sprite = unit.leaderSprite;
        switch (unit.type)
        {
            case TacticsUnit.Type.Power:
                charTypeImage.color = Color.red;
                break;
            case TacticsUnit.Type.Speed:
                charTypeImage.color = Color.blue;
                break;
            case TacticsUnit.Type.Tech:
                charTypeImage.color = Color.green;
                break;
        }
        charTypeText.text = unit.type.ToString();

        charNameText.text = unit.Name;
        charLvlText.text = "Lvl " + unit.level.ToString();
        charExpText.text = "Exp " + unit.exp.ToString();

        charHPText.text = unit.currentHP.ToString() + " / " + unit.maxHP.ToString();
        charHPSlider.maxValue = unit.maxHP;
        charHPSlider.value = unit.currentHP;
        charNRGText.text = unit.currentNRG.ToString() + " / " + unit.maxNRG.ToString();
        charNRGSlider.maxValue = unit.maxNRG;
        charNRGSlider.value = unit.currentNRG;

        charStatsText[0].text = unit.ATK.ToString();
        charStatsText[1].text = unit.DEF.ToString();
        charStatsText[2].text = unit.ACC.ToString();
        charStatsText[3].text = unit.EVA.ToString();
        charStatsText[4].text = unit.MOV.ToString();

        // rhs
        charButtonSkillsText[0].text = unit.skillName[0];
        charButtonSkillsText[1].text = unit.skillName[1];
        charButtonSkillsText[2].text = unit.skillName[2];

        charUnitPanels[0].SetActive(true);
        charUnitPanels[1].SetActive(false);

        battlePrepPanels[1].SetActive(true);
    }

    public void ShowSkillPanel(int skillIndex)
    {
        charSkillsText[0].text = "SKILL: " + unit.skillName[skillIndex];
        charSkillsText[1].text = unit.skillType[skillIndex];
        charSkillsText[2].text = unit.skillPWR[skillIndex].ToString();
        charSkillsText[3].text = unit.skillNRG[skillIndex].ToString();
        charSkillsText[4].text = unit.skillDescript[skillIndex];

        charUnitPanels[0].SetActive(false);
        charUnitPanels[1].SetActive(true);
    }

    public void ShowCharPanelAgain()
    {
        charUnitPanels[0].SetActive(true);
        charUnitPanels[1].SetActive(false);
    }

    // upgrade char unit panel
    void ResetUpgradesPanel()
    {
        for (int i = 0; i < 6; i++)
        {
            upgradeAddCounter[i] = 0;
            upgradeCost[i] = 0;
        }
    }

    public void SetupUpgradesPanel()
    {
        ResetUpgradesPanel();

        // current texts
        upgradeStatsText[0].text = unit.currentHP.ToString() + " / " + unit.maxHP.ToString();
        upgradeStatsText[1].text = unit.currentNRG.ToString() + " / " + unit.maxNRG.ToString();
        upgradeStatsText[2].text = unit.ATK.ToString();
        upgradeStatsText[3].text = unit.DEF.ToString();
        upgradeStatsText[4].text = unit.ACC.ToString();
        upgradeStatsText[5].text = unit.EVA.ToString();

        // cost texts
        CalculateUpgradeCosts();
    }

        // add text buttons
    public void IncreaseAddUpgradeText(int index)
    {
        upgradeCost[index] += 100;
        if (index == 0)
            upgradeAddCounter[index] += 100;
        else if (index == 1)
            upgradeAddCounter[index] += 20;
        else
            upgradeAddCounter[index] += 1;

        CalculateUpgradeCosts();
    }

    public void DecreaseAddUpgradeText(int index)
    {
        upgradeCost[index] -= 100;
        if (index == 0)
            upgradeAddCounter[index] -= 100;
        else if (index == 1)
            upgradeAddCounter[index] -= 20;
        else
            upgradeAddCounter[index] -= 1;

        if (upgradeCost[index] < 0)
            upgradeCost[index] = 0;

        if (upgradeAddCounter[index] < 0)
            upgradeAddCounter[index] = 0;

        CalculateUpgradeCosts();
    }

    void CalculateUpgradeCosts()
    {
        for (int i = 0; i < upgradeCostStatsText.Length; i++)
        {
            upgradeAddStatsText[i].text = upgradeAddCounter[i].ToString();
            upgradeCostStatsText[i].text = upgradeCost[i].ToString();
        }
    }

        // upgrade buttons
    public void RecoverHP(bool hp)
    {
        if (hp)
        {
            if (GameManager.instance.gold >= upgradeCost[0])
            {
                GameManager.instance.UseGold(upgradeCost[0]);
                goldText.text = "Gold: " + GameManager.instance.gold.ToString();

                unit.currentHP += int.Parse(upgradeAddStatsText[0].text);
                if (unit.currentHP > unit.maxHP)
                    unit.currentHP = unit.maxHP;

                SetupUpgradesPanel();
            }
        }
        else
        {
            if (GameManager.instance.gold >= upgradeCost[1])
            {
                GameManager.instance.UseGold(upgradeCost[1]);
                goldText.text = "Gold: " + GameManager.instance.gold.ToString();

                unit.currentNRG += int.Parse(upgradeAddStatsText[1].text);
                if (unit.currentNRG > unit.maxNRG)
                    unit.currentNRG = unit.maxNRG;

                SetupUpgradesPanel();
            }
        }
    }

    public void UpgradeStat(int index)
    {
        if (GameManager.instance.gold >= upgradeCost[index])
        {
            GameManager.instance.UseGold(upgradeCost[index]);
            goldText.text = "Gold: " + GameManager.instance.gold.ToString();

            switch (index)
            {
                case 2:
                    unit.ATK += upgradeAddCounter[2];
                    break;
                case 3:
                    unit.DEF += upgradeAddCounter[3];
                    break;
                case 4:
                    unit.ACC += upgradeAddCounter[4];
                    break;
                case 5:
                    unit.EVA += upgradeAddCounter[5];
                    break;
            }

            SetupUpgradesPanel();
        }
    }
}
