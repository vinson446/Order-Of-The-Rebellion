using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBattleUnitState : TacticsUnitState
{
    public override void Enter()
    {
        tacticsMovement.RemoveSelectableTiles();
    }
}
