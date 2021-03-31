using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectEnemyCombatState : CombatState
{
    public override void Enter()
    {
        stateMachine.State = "UnitSelectEnemyCombatState";
    }
}
