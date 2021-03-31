using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCombatState : CombatState
{
    public override void Enter()
    {
        stateMachine.State = "DefaultCombatState";
    }
}
