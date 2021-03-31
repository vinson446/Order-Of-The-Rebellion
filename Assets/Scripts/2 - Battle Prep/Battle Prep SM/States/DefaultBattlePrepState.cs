using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBattlePrepState : BattlePrepState
{
    public override void Enter()
    {
        stateMachine.State = "DefaultBattlePrepState";
    }
}
