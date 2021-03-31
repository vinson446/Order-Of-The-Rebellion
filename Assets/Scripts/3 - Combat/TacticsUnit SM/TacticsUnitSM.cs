using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsUnitSM : StateMachine
{
    // Start is called before the first frame update
    void Start()
    {
        ChangeState<DefaultUnitState>();
    }
}
