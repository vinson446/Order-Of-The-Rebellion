using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectActionUnitState : TacticsUnitState
{
    public List<TacticsUnit> selectableUnits;

    MovementUnitState movementUnitState;

    private void Start()
    {
        movementUnitState = GetComponent<MovementUnitState>();
    }

    public override void Enter()
    {
        combatSM.ChangeState<UnitSelectActionCombatState>();
        stateMachine.State = "SelectActionUnitState";

        inputManager.stopInput = true;
        uiManager.ResetUI();

        selectableUnits.Clear();
        // get list of selectable tiles to check for selectable units
        UpdateSelectableTilesForSelectableUnits(false);

        if (transform.parent.tag == "AllyUnit")
        {
            uiManager.ShowUnitInfoPanel(unit);
            uiManager.ShowActionsPanel(this);

            CheckForEnemiesInAtkRange(true);

            // enable atk and skill buttons if there is an enemy in range
            if (selectableUnits.Count == 0)
            {
                uiManager.actionButtons[1].enabled = false;
                uiManager.actionButtons[2].enabled = false;
            }
            else
            {
                uiManager.actionButtons[1].enabled = true;
                uiManager.actionButtons[2].enabled = true;
            }
        }
        else
        {
            CheckForEnemiesInAtkRange(false);
            StartCoroutine(EnemySelectActionCoroutine());
        }
    }

    public void ResetMove()
    {
        movementUnitState.ResetMove();
        stateMachine.ChangeState<MovementUnitState>();

        inputManager.stopInput = false;
    }

    public void UpdateSelectableTilesForSelectableUnits(bool showTiles)
    {
        if (showTiles)
            tacticsMovement.FindSelectableTiles(true, true, true);
        else
            tacticsMovement.FindSelectableTiles(false, true, true);
    }

    void CheckForEnemiesInAtkRange(bool enemies)
    {
        foreach (Tile t in tacticsMovement.selectableTiles)
        {
            RaycastHit hit;

            if (Physics.Raycast(t.transform.position, Vector3.up, out hit, Mathf.Infinity))
            {
                if (enemies)
                {
                    EnemyUnit eUnit = hit.collider.GetComponent<EnemyUnit>();

                    if (eUnit != null && !selectableUnits.Contains(eUnit))
                    {
                        selectableUnits.Add(eUnit);
                    }
                }
                else
                {
                    AllyUnit aUnit = hit.collider.GetComponent<AllyUnit>();

                    if (aUnit != null && !selectableUnits.Contains(aUnit))
                    {
                        selectableUnits.Add(aUnit);
                    }
                }
            }
        }
    }

    public void Attack()
    {
        uiManager.ResetUI();

        stateMachine.ChangeState<SelectEnemyUnitState>();
    }

    public void UseSkill()
    {
        /*
        uiManager.ResetUI();

        stateMachine.ChangeState<SelectEnemyUnitState>();
        */
    }

    public void Standby()
    {
        uiManager.ResetUI();

        inputManager.stopInput = false;
        combatManager.ShowLevelTiles(false);

        DefaultUnitState defaultUnitState = GetComponent<DefaultUnitState>();
        defaultUnitState.finishedTurn = true;
        stateMachine.ChangeState<DefaultUnitState>();

        if (transform.parent.tag == "AllyUnit")
        {
            combatManager.EndPlayerUnitTurn(GetComponent<TacticsUnitSM>());
        }
        else
        {
            combatManager.StartNextEnemyUnitTurn(GetComponent<TacticsUnitSM>());
        }
    }

    IEnumerator EnemySelectActionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        if (selectableUnits.Count > 0)
        {
            stateMachine.ChangeState<SelectEnemyUnitState>();
        }
        else
        {
            yield return new WaitForSeconds(1.5f);

            Standby();
        }
    }
}
