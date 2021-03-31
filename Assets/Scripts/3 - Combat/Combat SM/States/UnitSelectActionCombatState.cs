using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectActionCombatState : CombatState
{
    public override void Enter()
    {
        stateMachine.State = "UnitSelectActionCombatState";
    }
}
