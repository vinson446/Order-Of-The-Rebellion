using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CombatUIManager : MonoBehaviour
{
    [Header("State IR")]
    [SerializeField] GameObject statePanel;
    [SerializeField] TextMeshProUGUI stateText;

    [Header("Shortened Unit Info IR")]
    [SerializeField] GameObject shortenedUnitPanel;
    [SerializeField] Image shortenedCharImage;
    [SerializeField] TextMeshProUGUI shortenedNameText;
    [SerializeField] TextMeshProUGUI shortenedLvlText;
    [SerializeField] Image shortenedTypeImage;
    [SerializeField] TextMeshProUGUI shortenedTypeText;
    [SerializeField] TextMeshProUGUI shortenedHPText;

    [Header("Unit Info IR")]
    [SerializeField] GameObject unitPanel;
    [SerializeField] Image charImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Image typeImage;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI nrgText;
    [SerializeField] Slider nrgSlider;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI defText;
    [SerializeField] TextMeshProUGUI accText;
    [SerializeField] TextMeshProUGUI evaText;
    [SerializeField] TextMeshProUGUI movText;

    [Header("Actions IR")]
    [SerializeField] GameObject actionsPanel;
    public Button[] actionButtons;

    [Header("Selectable Enemies IR")]
    [SerializeField] GameObject selectableEnemiesPanel;
    [SerializeField] Button backButton;

    [Header("Prebattle Info IR")]
    [SerializeField] GameObject prebattleInfoPanel;
    [SerializeField] Button battleButton;
    // top/mid
    [SerializeField] TextMeshProUGUI[] prebattleStrategyText;
    [SerializeField] TextMeshProUGUI[] prebattleNameText;
    [SerializeField] Image[] prebattleCharImage;
    [SerializeField] Image[] prebattleTypeGlowImage;
    [SerializeField] Image[] prebattleTypeImage;
    [SerializeField] TextMeshProUGUI[] prebattleTypeText;
    // bot
    [SerializeField] TextMeshProUGUI[] prebattleHPText;
    [SerializeField] Slider[] prebattleHPBar;
    [SerializeField] TextMeshProUGUI[] prebattleCurrentNRGText;
    [SerializeField] Slider[] prebattleNRGBar;
    [SerializeField] TextMeshProUGUI[] prebattleATKText;
    [SerializeField] TextMeshProUGUI[] prebattleACCText;
    [SerializeField] TextMeshProUGUI[] prebattleCRITText;

    TacticsUnit unit1;
    TacticsUnit unit2;

    [Header("Battle IR")]
    [SerializeField] GameObject battlePanel;
    [SerializeField] TextMeshProUGUI[] battleNameText;
    [SerializeField] Image[] battleTypeGlowImage;
    [SerializeField] Image[] battleTypeImage;
    [SerializeField] TextMeshProUGUI[] battleTypeText;
    [SerializeField] TextMeshProUGUI[] battleCurrentHPText;
    [SerializeField] TextMeshProUGUI[] battleMaxHPText;
    [SerializeField] Slider[] battleHPBar;
    [SerializeField] TextMeshProUGUI[] battleATKText;
    [SerializeField] TextMeshProUGUI[] battleACCText;
    [SerializeField] TextMeshProUGUI[] battleCRITText;
    [SerializeField] GameObject[] battleTotalDmgPanel;
    [SerializeField] TextMeshProUGUI[] battleTotalDmgText;

    [Header("Battle Stats")]
    public int minDamageToUnit2;
    public int maxDamageToUnit2;
    public int minDamageToUnit1;
    public int maxDamageToUnit1;
    int unit1ACC;
    int unit2ACC;
    float tmp;

    public bool unit1Super;
    public bool unit1Weak;
    public bool unit2Super;
    public bool unit2Weak;

    CombatSM combatSM;
    InputManager inputManager;

    private void Awake()
    {
        combatSM = FindObjectOfType<CombatSM>();
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        ResetUI();
    }

    public void ResetUI()
    {
        shortenedUnitPanel.SetActive(false);
        unitPanel.SetActive(false);

        foreach (Button b in actionButtons)
        {
            b.onClick.RemoveAllListeners();
        }
        actionsPanel.SetActive(false);

        selectableEnemiesPanel.SetActive(false);
        backButton.onClick.RemoveAllListeners();

        battleButton.onClick.RemoveAllListeners();
        prebattleInfoPanel.SetActive(false);

        battlePanel.SetActive(false);
    }

    public void ShowStatePanel(string msg, float duration, bool turnOnInput)
    {
        StartCoroutine(ShowStatePanelCoroutine(msg, duration, turnOnInput));
    }

    IEnumerator ShowStatePanelCoroutine(string msg, float duration, bool turnOnInput)
    {
        Time.timeScale = 0;
        inputManager.stopInput = true;

        stateText.text = msg;
        statePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(duration);

        statePanel.SetActive(false);

        Time.timeScale = 1;
        if (turnOnInput)
            inputManager.stopInput = false;
    }

    public void ShowShortenedUnitInfoPanel(TacticsUnit unit)
    {
        shortenedCharImage.sprite = unit.charFaceSprite;
        shortenedNameText.text = unit.Name;
        shortenedLvlText.text = unit.level.ToString();
        switch (unit.type)
        {
            case TacticsUnit.Type.Power:
                shortenedTypeImage.color = Color.red;
                break;
            case TacticsUnit.Type.Speed:
                shortenedTypeImage.color = Color.blue;
                break;
            case TacticsUnit.Type.Tech:
                shortenedTypeImage.color = Color.green;
                break;
        }    
        shortenedTypeText.text = unit.type.ToString();
        shortenedHPText.text = unit.currentHP.ToString() + " / " + unit.maxHP.ToString();

        shortenedUnitPanel.SetActive(true);
    }

    public void ShowUnitInfoPanel(TacticsUnit unit)
    {
        GetUnitInfo(unit);

        unitPanel.SetActive(true);
    }

    void GetUnitInfo(TacticsUnit unit)
    {
        nameText.text = unit.Name;
        charImage.sprite = unit.charFaceBodySprite;
        lvlText.text = unit.level.ToString();
        expText.text = unit.exp.ToString();

        switch (unit.type)
        {
            case TacticsUnit.Type.Power:
                typeImage.color = Color.red;
                typeText.text = "Power";
                break;
            case TacticsUnit.Type.Speed:
                typeImage.color = Color.blue;
                typeText.text = "Speed";
                break;
            case TacticsUnit.Type.Tech:
                typeImage.color = Color.green;
                typeText.text = "Tech";
                break;
        }

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        hpText.text = unit.currentHP.ToString() + " / " + unit.maxHP.ToString();
        nrgSlider.maxValue = unit.maxNRG;
        nrgSlider.value = unit.currentNRG;
        nrgText.text = unit.currentNRG.ToString() + " / " + unit.maxNRG.ToString();

        atkText.text = unit.ATK.ToString();
        defText.text = unit.DEF.ToString();
        accText.text = unit.ACC.ToString();
        evaText.text = unit.EVA.ToString();
        movText.text = unit.MOV.ToString();
    }

    public void ShowActionsPanel(SelectActionUnitState unit)
    {
        actionButtons[0].onClick.AddListener(unit.ResetMove);
        actionButtons[1].onClick.AddListener(unit.Attack);
        actionButtons[2].onClick.AddListener(unit.UseSkill);
        actionButtons[3].onClick.AddListener(unit.Standby);

        actionsPanel.SetActive(true);
    }

    public void ShowSelectableEnemiesPanel(SelectEnemyUnitState unit)
    {
        ResetUI();

        backButton.onClick.AddListener(unit.GoBackToSelectActionUnitState);

        selectableEnemiesPanel.SetActive(true);
    }

    public void ShowPrebattleInfoPanel(TacticsUnit u1, TacticsUnit u2)
    {
        unit1 = u1;
        unit2 = u2;

        CheckTypeAdvantages();

        ResetGlowButtons();
        if (unit1Super)
            StartCoroutine(GlowCoroutine(prebattleTypeGlowImage[0]));
        else if (unit2Super)
            StartCoroutine(GlowCoroutine(prebattleTypeGlowImage[1]));

        // top/mid
        prebattleStrategyText[0].text = "Attack";
        prebattleStrategyText[1].text = "Defense";

        ResetUI();

        prebattleNameText[0].text = unit1.Name;
        prebattleNameText[1].text = unit2.Name;

        prebattleCharImage[0].sprite = unit1.charFullSprite;
        prebattleCharImage[1].sprite = unit2.charFullSprite;

        if (unit1.type == TacticsUnit.Type.Power)
        {
            prebattleTypeImage[0].color = Color.red;
            prebattleTypeText[0].text = "Power";
        }
        else if (unit1.type == TacticsUnit.Type.Speed)
        {
            prebattleTypeImage[0].color = Color.blue;
            prebattleTypeText[0].text = "Speed";
        }
        else if (unit1.type == TacticsUnit.Type.Tech)
        {
            prebattleTypeImage[0].color = Color.green;
            prebattleTypeText[0].text = "Tech";
        }
        if (unit2.type == TacticsUnit.Type.Power)
        {
            prebattleTypeImage[1].color = Color.red;
            prebattleTypeText[1].text = "Power";
        }
        else if (unit2.type == TacticsUnit.Type.Speed)
        {
            prebattleTypeImage[1].color = Color.blue;
            prebattleTypeText[1].text = "Speed";
        }
        else if (unit2.type == TacticsUnit.Type.Tech)
        {
            prebattleTypeImage[1].color = Color.green;
            prebattleTypeText[1].text = "Tech";
        }

        // bot
        prebattleHPText[0].text = unit1.currentHP.ToString() + " / " + u1.maxHP.ToString();
        prebattleHPBar[0].maxValue = unit1.maxHP;
        prebattleHPBar[0].value = unit1.currentHP;
        prebattleHPText[1].text = unit2.currentHP.ToString() + " / " + u2.maxHP.ToString();
        prebattleHPBar[1].maxValue = unit2.maxHP;
        prebattleHPBar[1].value = unit2.currentHP;

        prebattleCurrentNRGText[0].text = unit1.currentNRG.ToString() + " / " + u1.maxNRG.ToString();
        prebattleNRGBar[0].maxValue = unit1.maxNRG;
        prebattleNRGBar[0].value = unit1.currentNRG;
        prebattleCurrentNRGText[1].text = unit2.currentNRG.ToString() + " / " + u2.maxNRG.ToString();
        prebattleNRGBar[1].maxValue = unit2.maxNRG;
        prebattleNRGBar[1].value = unit2.currentNRG;

        tmp = Mathf.Pow(unit1.ATK, 2f) / Mathf.Pow(unit2.DEF, 2f) * ((unit1.ATK + unit2.DEF) * 2);
        if (unit1Super)
            minDamageToUnit2 = Mathf.RoundToInt(tmp * 0.9f * 1.25f);
        else if (unit1Weak)
            minDamageToUnit2 = Mathf.RoundToInt(tmp * 0.9f * 0.75f);
        else
            minDamageToUnit2 = Mathf.RoundToInt(tmp * 0.9f);
        if (unit1Super)
            maxDamageToUnit2 = Mathf.RoundToInt(tmp * 1.1f * 1.25f);
        else if (unit1Weak)
            maxDamageToUnit2 = Mathf.RoundToInt(tmp * 1.1f * 0.75f);
        else
            maxDamageToUnit2 = Mathf.RoundToInt(tmp * 1.1f);
        prebattleATKText[0].text = minDamageToUnit2.ToString() + "-" + maxDamageToUnit2.ToString();

        tmp = Mathf.Pow(unit2.ATK, 2f) / Mathf.Pow(unit1.DEF, 2f) * ((unit2.ATK + unit1.DEF) * 2);
        if (unit2Super)
            minDamageToUnit1 = Mathf.RoundToInt(tmp * 0.9f * 1.25f);
        else if (unit2Weak)
            minDamageToUnit1 = Mathf.RoundToInt(tmp * 0.9f * 0.75f);
        else
            minDamageToUnit1 = Mathf.RoundToInt(tmp * 0.9f);
        if (unit2Super)
            maxDamageToUnit1 = Mathf.RoundToInt(tmp * 1.1f * 1.25f);
        else if (unit2Weak)
            maxDamageToUnit1 = Mathf.RoundToInt(tmp * 1.1f * 0.75f);
        else
            maxDamageToUnit1 = Mathf.RoundToInt(tmp * 1.1f);
        prebattleATKText[1].text = minDamageToUnit1.ToString() + "-" + maxDamageToUnit1.ToString();

        tmp = unit1.ACC / unit2.EVA;
        unit1ACC = Mathf.RoundToInt(tmp * 100);
        if (unit1ACC > 100)
            unit1ACC = 100;
        prebattleACCText[0].text = unit1ACC.ToString();
        tmp = unit2.ACC / unit1.EVA;
        unit2ACC = Mathf.RoundToInt(tmp * 100);
        if (unit2ACC > 100)
            unit2ACC = 100;
        prebattleACCText[1].text = unit2ACC.ToString();

        prebattleCRITText[0].text = unit1.CRIT.ToString();
        prebattleCRITText[1].text = unit2.CRIT.ToString();

        if (u1.GetComponent<AllyUnit>() != null)
        {
            battleButton.gameObject.SetActive(true);
            battleButton.onClick.AddListener(delegate { ShowBattleInfoPanel(true); });
        }
        else
        {
            battleButton.gameObject.SetActive(false);
        }

        prebattleInfoPanel.SetActive(true);
    }

    void CheckTypeAdvantages()
    {
        unit1Super = false;
        unit1Weak = false;
        unit2Super = false;
        unit2Weak = false;

        if (unit1.type == TacticsUnit.Type.Power && unit2.type == TacticsUnit.Type.Speed)
        {
            unit1Super = true;
            unit2Weak = true;
        }
        else if (unit1.type == TacticsUnit.Type.Speed && unit2.type == TacticsUnit.Type.Tech)
        {
            unit1Super = true;
            unit2Weak = true;
        }
        else if (unit1.type == TacticsUnit.Type.Tech && unit2.type == TacticsUnit.Type.Power)
        {
            unit1Super = true;
            unit2Weak = true;
        }

        else if (unit1.type == TacticsUnit.Type.Power && unit2.type == TacticsUnit.Type.Tech)
        {
            unit1Weak = true;
            unit2Super = true;
        }
        else if (unit1.type == TacticsUnit.Type.Speed && unit2.type == TacticsUnit.Type.Power)
        {
            unit1Weak = true;
            unit2Super = true;
        }
        else if (unit1.type == TacticsUnit.Type.Tech && unit2.type == TacticsUnit.Type.Speed)
        {
            unit1Weak = true;
            unit2Super = true;
        }
    }

    public void ShowBattleInfoPanel(bool playerAtk)
    {
        ResetUI();

        if (playerAtk)
            combatSM.GetComponent<BattleCombatState>().SetupBattle(unit1, unit2, playerAtk);
        else
            combatSM.GetComponent<BattleCombatState>().SetupBattle(unit2, unit1, playerAtk);
        combatSM.ChangeState<BattleCombatState>();

        unit1.GetComponentInChildren<TacticsUnitSM>().ChangeState<InBattleUnitState>();

        switch (unit1.type)
        {
            case TacticsUnit.Type.Power:
                battleTypeImage[0].color = Color.red;
                battleTypeText[0].text = "Power";
                break;
            case TacticsUnit.Type.Speed:
                battleTypeImage[0].color = Color.blue;
                battleTypeText[0].text = "Speed";
                break;
            case TacticsUnit.Type.Tech:
                battleTypeImage[0].color = Color.green;
                battleTypeText[0].text = "Tech";
                break;
        }
        switch (unit2.type)
        {
            case TacticsUnit.Type.Power:
                battleTypeImage[1].color = Color.red;
                battleTypeText[1].text = "Power";
                break;
            case TacticsUnit.Type.Speed:
                battleTypeImage[1].color = Color.blue;
                battleTypeText[1].text = "Speed";
                break;
            case TacticsUnit.Type.Tech:
                battleTypeImage[1].color = Color.green;
                battleTypeText[1].text = "Tech";
                break;
        }
        if (unit1Super)
            StartCoroutine(GlowCoroutine(battleTypeGlowImage[0]));
        else if (unit2Super)
            StartCoroutine(GlowCoroutine(battleTypeGlowImage[1]));

        battleNameText[0].text = unit1.Name;
        battleNameText[1].text = unit2.Name;

        battleCurrentHPText[0].text = unit1.currentHP.ToString();
        battleHPBar[0].maxValue = unit1.maxHP;
        battleHPBar[0].value = unit1.currentHP;
        battleCurrentHPText[1].text = unit2.currentHP.ToString();
        battleHPBar[1].maxValue = unit2.maxHP;
        battleHPBar[1].value = unit2.currentHP;

        // not implemented yet
        // battleMaxHPText[0].text = ally.maxHP.ToString();
        // battleCurrentHPText[1].text = enemy.maxHP.ToString();

        battleATKText[0].text = minDamageToUnit2.ToString() + "-" + maxDamageToUnit2.ToString();
        battleATKText[1].text = minDamageToUnit1.ToString() + "-" + maxDamageToUnit1.ToString();

        battleACCText[0].text = unit1ACC.ToString();
        battleACCText[1].text = unit2ACC.ToString();

        battleCRITText[0].text = unit1.CRIT.ToString();
        battleCRITText[1].text = unit2.CRIT.ToString();

        battlePanel.SetActive(true);
    }

    public void ShowBattleTotalDmg(bool on, bool left, int dmg)
    {
        // turn on panels
        if (on)
        {
            // left panel
            if (left)
            {
                // miss
                if (dmg == 0)
                {
                    battleTotalDmgText[0].text = "MISS";
                    battleTotalDmgPanel[0].SetActive(true);
                }
                // damage
                else
                {
                    battleTotalDmgText[0].text = "Total DMG: " + dmg.ToString();
                    battleTotalDmgPanel[0].SetActive(true);
                }
            }
            // right panel
            else
            {
                // miss
                if (dmg == 0)
                {
                    battleTotalDmgText[1].text = "MISS";
                    battleTotalDmgPanel[1].SetActive(true);
                }
                // damage
                else
                {
                    battleTotalDmgText[1].text = "Total DMG: " + dmg.ToString();
                    battleTotalDmgPanel[1].SetActive(true);
                }
            }
        }
        // turn off panels
        else
        {
            battleTotalDmgPanel[0].SetActive(false);
            battleTotalDmgPanel[1].SetActive(false);
        }
    }

    IEnumerator GlowCoroutine(Image image)
    {
        image.gameObject.SetActive(true);

        while (true)
        {
            image.DOFade(0.15f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            image.DOFade(0.75f, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResetGlowButtons()
    {
        prebattleTypeGlowImage[0].gameObject.SetActive(false);
        prebattleTypeGlowImage[1].gameObject.SetActive(false);

        battleTypeGlowImage[0].gameObject.SetActive(false);
        battleTypeGlowImage[1].gameObject.SetActive(false);
    }
}
