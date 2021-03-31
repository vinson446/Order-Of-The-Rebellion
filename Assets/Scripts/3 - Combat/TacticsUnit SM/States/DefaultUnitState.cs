using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUnitState : TacticsUnitState
{
    [Header("Turn Settings")]
    public bool finishedTurn;

    public override void Enter()
    {
        combatSM.ChangeState<DefaultCombatState>();
        stateMachine.State = "DefaultUnitState";

        uiManager.ResetUI();

        tacticsMovement.SetLastUnitLocation();
    }

    public void ResetFlags()
    {
        finishedTurn = false;

        GetComponentInParent<TacticsMovement>().hasMoved = false;
    }

    // player
    public void AllyUnitClick()
    {
        camManager.TurnOnFocusCam(transform.parent);

        if (combatManager.allyTurn)
        {
            if (!finishedTurn)
            {
                stateMachine.ChangeState<MovementUnitState>();
            }
            else
            {
                uiManager.ShowUnitInfoPanel(unit);
            }
        }
    }

    // player
    public void EnemyUnitClick()
    {
        uiManager.ShowUnitInfoPanel(unit);
        camManager.TurnOnFocusCam(transform.parent);
    }

    // ai
    public void GoToMovementState()
    {
        if (combatManager.enemyTurn)
        {
            if (!finishedTurn)
            {
                StartCoroutine(GoToMovementCoroutine());
            }
        }
    }

    IEnumerator GoToMovementCoroutine()
    {
        camManager.TurnOnFocusCam(transform.parent);

        yield return new WaitForSeconds(1);

        stateMachine.ChangeState<MovementUnitState>();
    }
}
