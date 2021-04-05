using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleCombatState : CombatState
{
    [Header("UI IR References")]
    [SerializeField] TextMeshProUGUI[] currentHPText;
    [SerializeField] Slider[] hpBarSlider;

    [Header("Battle Settings")]
    public TacticsUnit ally;
    public TacticsUnit enemy;
    public bool playerAtk;

    public override void Enter()
    {
        stateMachine.State = "BattleCombatState";

        StartCoroutine(BattleCoroutine());
    }

    public void SetupBattle(TacticsUnit a, TacticsUnit e, bool atk)
    {
        ally = a;
        enemy = e;
        playerAtk = atk;
    }

    IEnumerator BattleCoroutine()
    {
        float hitTmp;
        bool crit;

        int damageToEnemy;
        int currentEnemyHP;
        int targetEnemyHP;

        int damageToAlly;
        int currentAllyHP;
        int targetAllyHP;

        combatManager.ResetLevelTiles();
        combatManager.ShowLevelTiles(false);

        yield return new WaitForSeconds(1);

        // player initiated attack
        if (playerAtk)
        {
            hitTmp = ally.ACC / enemy.EVA * 100;

            // ally hits enemy
            if (Random.Range(0, 100) <= hitTmp)
            {
                // crit chance
                if (Random.Range(0, 100) <= ally.CRIT)
                {
                    crit = true;
                    damageToEnemy = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit2, uiManager.maxDamageToUnit2) * 1.5f);
                }
                else
                {
                    crit = false;
                    damageToEnemy = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit2, uiManager.maxDamageToUnit2));
                }

                currentEnemyHP = enemy.currentHP;
                targetEnemyHP = enemy.currentHP - damageToEnemy;
                if (targetEnemyHP < 0)
                    targetEnemyHP = 0;

                if (crit)
                    print("1ST ATTACK- Ally critting Enemy: " + damageToEnemy + " damage \n Target Enemy HP: " + targetEnemyHP);
                else
                    print("1ST ATTACK- Ally attacking Enemy: " + damageToEnemy + " damage \n Target Enemy HP: " + targetEnemyHP);

                while (currentEnemyHP > targetEnemyHP)
                {
                    currentEnemyHP -= 1;
                    currentHPText[1].text = currentEnemyHP.ToString();
                    hpBarSlider[1].value = currentEnemyHP;

                    yield return null;
                }

                uiManager.ShowBattleTotalDmg(true, false, damageToEnemy);

                yield return new WaitForSeconds(2);

                uiManager.ShowBattleTotalDmg(false, false, 0);

                // enemy dies to ally attack
                enemy.currentHP = targetEnemyHP;
            }
            // ally misses enemy
            else
            {
                print("1ST ATTACK- Ally misses Enemy");

                uiManager.ShowBattleTotalDmg(true, false, 0);

                yield return new WaitForSeconds(2);

                uiManager.ShowBattleTotalDmg(false, false, 0);
            }

            // enemy counterattacks ally if its still alive
            if (enemy.currentHP > 0)
            {
                hitTmp = enemy.ACC / ally.EVA * 100;

                // enemy hits ally
                if (Random.Range(0, 100) <= hitTmp)
                {
                    if (Random.Range(0, 100) <= enemy.CRIT)
                    {
                        crit = true;
                        damageToAlly = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit1, uiManager.maxDamageToUnit1) * 1.5f);
                    }
                    else
                    {
                        crit = false;
                        damageToAlly = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit1, uiManager.maxDamageToUnit1));
                    }

                    currentAllyHP = ally.currentHP;
                    targetAllyHP = ally.currentHP - damageToAlly;
                    if (targetAllyHP < 0)
                        targetAllyHP = 0;

                    if (crit)
                        print("2ND ATTACK- Enemy critting Ally: " + damageToAlly + " damage \n Target Ally HP: " + targetAllyHP);
                    else
                        print("2ND ATTACK- Enemy attacking Ally: " + damageToAlly + " damage \n Target Ally HP: " + targetAllyHP);

                    while (currentAllyHP > targetAllyHP)
                    {
                        currentAllyHP -= 1;
                        currentHPText[0].text = currentAllyHP.ToString();
                        hpBarSlider[0].value = currentAllyHP;

                        yield return null;
                    }

                    uiManager.ShowBattleTotalDmg(true, true, damageToAlly);

                    yield return new WaitForSeconds(2);

                    uiManager.ShowBattleTotalDmg(false, false, 0);

                    // ally dies to enemy attack
                    ally.currentHP = targetAllyHP;
                    if (ally.currentHP <= 0)
                    {
                        print("2ND ATTACK- Ally unit died from player initiated attack");
                        combatManager.CheckBattleEnd(true, ally);
                    }
                }
                // enemy misses ally
                else
                {
                    print("2ND ATTACK- Enemy misses ally");

                    uiManager.ShowBattleTotalDmg(true, true, 0);

                    yield return new WaitForSeconds(2);

                    uiManager.ShowBattleTotalDmg(false, true, 0);
                }
            }
            // enemy dies to ally's first attack
            else
            {
                print("1ST ATTACK- Enemy unit died from player initiated attack");
                combatManager.CheckBattleEnd(false, enemy);
            }
        }

        // enemy initiated attack
        else
        {
            hitTmp = enemy.ACC / ally.EVA * 100;

            // enemy hits ally
            if (Random.Range(0, 100) <= hitTmp)
            {
                if (Random.Range(0, 100) <= enemy.CRIT)
                {
                    crit = true;
                    damageToAlly = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit2, uiManager.maxDamageToUnit2) * 1.5f);
                }
                else
                {
                    crit = false;
                    damageToAlly = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit2, uiManager.maxDamageToUnit2));
                }

                currentAllyHP = ally.currentHP;
                targetAllyHP = ally.currentHP - damageToAlly;
                if (targetAllyHP < 0)
                    targetAllyHP = 0;

                if (crit)
                    print("1ST ATTACK: Enemy critting Ally: " + damageToAlly + " damage \n Target Ally HP: " + targetAllyHP);
                else
                    print("1ST ATTACK: Enemy attacking Ally: " + damageToAlly + " damage \n Target Ally HP: " + targetAllyHP);

                while (currentAllyHP > targetAllyHP)
                {
                    currentAllyHP -= 1;
                    currentHPText[1].text = currentAllyHP.ToString();
                    hpBarSlider[1].value = currentAllyHP;

                    yield return null;
                }

                uiManager.ShowBattleTotalDmg(true, false, damageToAlly);

                yield return new WaitForSeconds(2);

                uiManager.ShowBattleTotalDmg(false, false, 0);

                ally.currentHP = targetAllyHP;
            }
            // enemy misses ally
            else
            {
                print("1ST ATTACK: ENEMY MISSES ALLY");
                uiManager.ShowBattleTotalDmg(true, false, 0);

                yield return new WaitForSeconds(2);

                uiManager.ShowBattleTotalDmg(false, false, 0);
            }

            // ally attacks enemy if its still alive
            if (ally.currentHP > 0)
            {
                hitTmp = ally.ACC / enemy.EVA * 100;
                
                // ally hits enemy
                if (Random.Range(0, 100) <= hitTmp)
                {
                    if (Random.Range(0, 100) <= ally.CRIT)
                    {
                        crit = true;
                        damageToEnemy = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit1, uiManager.maxDamageToUnit1) * 1.5f);
                    }
                    else
                    {
                        crit = false;
                        damageToEnemy = Mathf.RoundToInt(Random.Range(uiManager.minDamageToUnit1, uiManager.maxDamageToUnit1));
                    }

                    currentEnemyHP = enemy.currentHP;
                    targetEnemyHP = enemy.currentHP - damageToEnemy;
                    if (targetEnemyHP < 0)
                        targetEnemyHP = 0;

                    if (crit)
                        print("2ND ATTACK- Ally critting Enemy: " + damageToEnemy + " damage \n Target Enemy HP: " + targetEnemyHP);
                    else
                        print("2ND ATTACK- Ally attacking Enemy: " + damageToEnemy + " damage \n Target Enemy HP: " + targetEnemyHP);

                    while (currentEnemyHP > targetEnemyHP)
                    {
                        currentEnemyHP -= 1;
                        currentHPText[0].text = currentEnemyHP.ToString();
                        hpBarSlider[0].value = currentEnemyHP;

                        yield return null;
                    }

                    uiManager.ShowBattleTotalDmg(true, true, damageToEnemy);

                    yield return new WaitForSeconds(2);

                    uiManager.ShowBattleTotalDmg(false, false, 0);

                    // enemy dies to ally attack
                    enemy.currentHP = targetEnemyHP;
                    if (enemy.currentHP <= 0)
                    {
                        print("2ND ATTACK- Enemy unit died from enemy initiated attack");
                        combatManager.CheckBattleEnd(false, enemy);
                    }
                }
                // ally misses enemy
                else
                {
                    print("2ND ATTACK- Ally misses enemy");

                    uiManager.ShowBattleTotalDmg(true, true, 0);

                    yield return new WaitForSeconds(2);

                    uiManager.ShowBattleTotalDmg(false, false, 0);
                }
            }
            // ally dies to enemy attack
            else
            {
                print("2ND ATTACK- Ally unit died from enemy initiated attack");
                combatManager.CheckBattleEnd(true, ally);
            }
        }

        if (!combatManager.battleComplete && ally != null && enemy != null)
            EndBattlePhase();
    }

    void EndBattlePhase()
    {
        if (playerAtk)
        {
            ally.GetComponentInChildren<SelectActionUnitState>().Standby();
        }
        else
        {
            enemy.GetComponentInChildren<SelectActionUnitState>().Standby();
        }
    }
}
