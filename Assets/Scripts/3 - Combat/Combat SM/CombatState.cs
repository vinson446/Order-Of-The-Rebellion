using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
{
    protected CombatSM stateMachine;

    protected CombatManager combatManager;
    protected CameraManager camManager;
    protected CombatUIManager uiManager;

    private void Awake()
    {
        stateMachine = GetComponent<CombatSM>();

        combatManager = FindObjectOfType<CombatManager>();
        camManager = FindObjectOfType<CameraManager>();
        uiManager = FindObjectOfType<CombatUIManager>();
    }
}
