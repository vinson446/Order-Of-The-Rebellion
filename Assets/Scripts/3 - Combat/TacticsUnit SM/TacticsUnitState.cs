using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsUnitState : State
{
    protected StateMachine stateMachine;

    protected TacticsUnit unit;
    protected TacticsMovement tacticsMovement;

    protected CombatManager combatManager;
    protected CombatSM combatSM;
    protected CombatUIManager uiManager;
    protected CameraManager camManager;
    protected InputManager inputManager;

    private void Awake()
    {
        stateMachine = GetComponent<TacticsUnitSM>();

        unit = GetComponentInParent<TacticsUnit>();
        tacticsMovement = GetComponentInParent<TacticsMovement>();

        combatManager = FindObjectOfType<CombatManager>();
        combatSM = FindObjectOfType<CombatSM>();
        uiManager = FindObjectOfType<CombatUIManager>();
        camManager = FindObjectOfType<CameraManager>();
        inputManager = FindObjectOfType<InputManager>();
    }
}
