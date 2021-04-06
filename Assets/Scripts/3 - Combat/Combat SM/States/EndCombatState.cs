using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCombatState : CombatState
{
    bool winBattle;

    public override void Enter()
    {
        uiManager.ResetUI();
    }

    public void WinBattle()
    {
        winBattle = true;
        StartCoroutine(EndBattleCoroutine());
    }

    public void LoseBattle()
    {
        winBattle = false;
        StartCoroutine(EndBattleCoroutine());
    }

    IEnumerator EndBattleCoroutine()
    {
        if (winBattle)
        {
            uiManager.ShowStatePanel("BATTLE COMPLETE", 3, false);
        }
        else
        {
            uiManager.ShowStatePanel("DEFEAT", 3, false);
        }

        // save stats from combat in prefab
        AllyUnit[] units = FindObjectsOfType<AllyUnit>();
        for (int i = 0; i < units.Length; i++)
        {
            AllyUnit unitData = GameManager.instance.units[i];

            unitData.Name = units[i].Name;
            unitData.charFullSprite = units[i].charFullSprite;
            unitData.charFaceSprite = units[i].charFaceSprite;
            unitData.charFaceBodySprite = units[i].charFaceBodySprite;

            unitData.maxHP = units[i].maxHP;
            unitData.currentHP = units[i].currentHP;
            unitData.maxNRG = units[i].maxNRG;
            unitData.currentNRG = units[i].currentNRG;

            unitData.ATK = units[i].ATK;
            unitData.DEF = units[i].DEF;
            unitData.ACC = units[i].ACC;
            unitData.EVA = units[i].EVA;

            unitData.CRIT = units[i].CRIT;
            unitData.MOV = units[i].MOV;
        }

        yield return new WaitForSecondsRealtime(3);

        SceneManager.LoadScene(0);
    }
}
