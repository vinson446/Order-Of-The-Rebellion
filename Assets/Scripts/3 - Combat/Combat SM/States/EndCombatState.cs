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

        yield return new WaitForSecondsRealtime(3);

        SceneManager.LoadScene(0);
    }
}
