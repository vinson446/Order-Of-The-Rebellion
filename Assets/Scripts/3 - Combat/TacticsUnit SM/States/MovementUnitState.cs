using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUnitState : TacticsUnitState
{
    private void Start()
    {
        if (gameObject.transform.parent.tag == "AllyUnit")
            tacticsMovement = GetComponentInParent<AllyMovement>();
        else
            tacticsMovement = GetComponentInParent<EnemyMovement>();
    }

    public override void Enter()
    {
        combatSM.ChangeState<UnitMovementCombatState>();
        stateMachine.State = "MovementUnitState";

        combatManager.lastUnit = GetComponentInParent<TacticsUnit>();

        uiManager.ResetUI();
        uiManager.ShowShortenedUnitInfoPanel(unit);

        if (camManager.isDefault)
            camManager.TurnOnFocusCam(transform.parent.transform);

        tacticsMovement.StartMovePhase();
    }

    public void ResetMove()
    {
        tacticsMovement.hasMoved = false;
        tacticsMovement.ResetMove();
    }

    public override void Exit()
    {
        tacticsMovement.RemoveSelectableTiles();

        inputManager.stopInput = false;
    }
}
