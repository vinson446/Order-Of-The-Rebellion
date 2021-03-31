using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSM : StateMachine
{
    // Start is called before the first frame update
    void Start()
    {
        State = "DefaultCombatState";
    }
}
