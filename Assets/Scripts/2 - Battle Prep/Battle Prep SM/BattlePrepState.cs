using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePrepState : State
{
    protected StateMachine stateMachine;

    BattlePrepManager battlePrepManager;

    private void Awake()
    {
        stateMachine = GetComponent<BattlePrepSM>();

        battlePrepManager = FindObjectOfType<BattlePrepManager>();
    }
}
