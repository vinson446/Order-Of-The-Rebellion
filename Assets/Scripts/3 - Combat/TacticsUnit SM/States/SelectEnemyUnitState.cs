using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnemyUnitState : TacticsUnitState
{
    public TacticsUnit selectedEnemy;
    TacticsUnit selectedAlly;

    SelectActionUnitState selectActionUnitState;

    private void Start()
    {
        selectActionUnitState = GetComponent<SelectActionUnitState>();
    }

    public override void Enter()
    {
        combatSM.ChangeState<UnitSelectEnemyCombatState>();
        stateMachine.State = "SelectEnemyUnitState";

        uiManager.ResetUI();
        inputManager.stopInput = false;

        if (transform.parent.tag == "AllyUnit")
        {
            uiManager.ShowSelectableEnemiesPanel(this);

            // show selectable enemies in atk range
            selectActionUnitState.UpdateSelectableTilesForSelectableUnits(true);
        }
        else
        {
            FindClosestAlly();
            StartCoroutine(EnemyAttackCoroutine());
        }
    }

    public void GoBackToSelectActionUnitState()
    {
        selectedEnemy = null;

        stateMachine.ChangeState<SelectActionUnitState>();
    }

    // input manager- called when selecting an enemy in atk range
    public void ShowPreBattleInfo(bool playerAtk)
    {
        if (selectActionUnitState.selectableUnits.Contains(selectedEnemy))
        {
            uiManager.ShowPrebattleInfoPanel(unit, selectedEnemy);
            camManager.TurnOnFocusCam(selectedEnemy.transform);
        }
    }

    // for enemies
    void FindClosestAlly()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < selectActionUnitState.selectableUnits.Count; i++)
        {
            if (Vector3.Distance(transform.parent.transform.position, unit.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(transform.parent.transform.position, unit.transform.position);
                closestIndex = i;
            }
        }

        selectedAlly = selectActionUnitState.selectableUnits[closestIndex];
    }

    IEnumerator EnemyAttackCoroutine()
    {
        // show selectable enemies in atk range
        selectActionUnitState.UpdateSelectableTilesForSelectableUnits(true);

        yield return new WaitForSeconds(2);

        uiManager.ShowPrebattleInfoPanel(unit, selectedAlly);
        camManager.TurnOnFocusCam(selectedAlly.transform);

        yield return new WaitForSeconds(2f);

        uiManager.ShowBattleInfoPanel(false);
        selectActionUnitState.UpdateSelectableTilesForSelectableUnits(false);
    }
}
