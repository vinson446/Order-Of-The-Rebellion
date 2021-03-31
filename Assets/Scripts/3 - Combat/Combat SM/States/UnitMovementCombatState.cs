using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementCombatState : CombatState
{
    public override void Enter()
    {
        stateMachine.State = "UnitMovementCombatState";
    }
}
