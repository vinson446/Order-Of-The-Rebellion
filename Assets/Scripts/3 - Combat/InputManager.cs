using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Debugger")]
    public bool stopInput;

    // references
    CombatSM combatSM;
    CombatManager combatManager;
    CameraManager camManager;
    CombatUIManager uiManager;

    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    

    // Start is called before the first frame update
    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        combatSM = FindObjectOfType<CombatSM>();
        camManager = FindObjectOfType<CameraManager>();
        uiManager = FindObjectOfType<CombatUIManager>();

        raycaster = uiManager.GetComponent<GraphicRaycaster>();
        eventSystem = uiManager.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopInput && combatManager.allyTurn)
        {
            if (combatSM.State == "DefaultCombatState")
            {
                DefaultInput();
            }
            else if (combatSM.State == "UnitMovementCombatState")
            {
                UnitMovementInput();
            }
            else if (combatSM.State == "UnitSelectActionCombatState")
            {

            }
            else if (combatSM.State == "UnitSelectEnemyCombatState")
            {
                UnitSelectEnemyInput();
            }
        }
    }

    void ResetSelection()
    {
        if (combatManager.selectedUnitSM != null)
        {
            if (combatManager.selectedUnitSM.State == "MovementUnitState")
            {
                combatManager.selectedUnitSM.GetComponentInParent<TacticsMovement>().EndMovePhase();
                combatManager.selectedUnitSM.ChangeState<DefaultUnitState>();
            }
        }

        combatManager.ResetLevelTiles();
        combatManager.ShowLevelTiles(false);

        combatManager.selectedUnitSM = null;
    }

    void DefaultInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetSelection();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // clicking on an ally unit starts its state machine
                if (hit.collider.tag == "AllyUnit")
                {
                    combatManager.selectedUnitSM = hit.collider.gameObject.GetComponentInChildren<TacticsUnitSM>();

                    DefaultUnitState defaultUnitState = hit.collider.gameObject.GetComponentInChildren<DefaultUnitState>();
                    defaultUnitState.AllyUnitClick();

                    combatSM.ChangeState<UnitMovementCombatState>();
                }
                // clicking on an enemy unit displays its info
                else if (hit.collider.tag == "EnemyUnit")
                {
                    combatManager.selectedUnitSM = hit.collider.gameObject.GetComponentInChildren<TacticsUnitSM>();

                    DefaultUnitState defaultUnitState = hit.collider.gameObject.GetComponentInChildren<DefaultUnitState>();
                    defaultUnitState.EnemyUnitClick();

                    combatSM.ChangeState<UnitMovementCombatState>();
                }
            }
        }
    }

    void UnitMovementInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // clicking on one of a unit's selectable tiles moves the unit to the tile
                if (hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelectable)
                {
                    combatManager.selectedUnitSM.GetComponentInParent<TacticsMovement>().CreatePathToTargetTile(hit.collider.GetComponent<Tile>());
                    stopInput = true;
                }
                // clicking on an ally unit starts its state machine
                else if (hit.collider.tag == "AllyUnit")
                {
                    ResetSelection();

                    combatManager.selectedUnitSM = hit.collider.gameObject.GetComponentInChildren<TacticsUnitSM>();

                    DefaultUnitState defaultUnitState = combatManager.selectedUnitSM.GetComponentInChildren<DefaultUnitState>();
                    defaultUnitState.AllyUnitClick();
                }
                // clicking on an enemy unit displays its info
                else if (hit.collider.tag == "EnemyUnit")
                {
                    ResetSelection();

                    combatManager.selectedUnitSM = hit.collider.gameObject.GetComponentInChildren<TacticsUnitSM>();

                    DefaultUnitState defaultUnitState = combatManager.selectedUnitSM.GetComponentInChildren<DefaultUnitState>();
                    defaultUnitState.EnemyUnitClick();
                }
                // clicking elsewhere brings back initial map/combat state
                else 
                {
                    ResetSelection();
                    camManager.TurnOnDefaultCam();
                    uiManager.ResetUI();

                    combatSM.ChangeState<DefaultCombatState>();
                }
            }
        }
    }

    void UnitSelectEnemyInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // check if ui is clicked (press battle button)

            // set up new pointer event
            pointerEventData = new PointerEventData(eventSystem);
            // set pointer event pos to mouse pos
            pointerEventData.position = Input.mousePosition;
            // create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            // raycast using the Graphics raycaster and mouse click position
            raycaster.Raycast(pointerEventData, results);

            // if no ui is clicked
            if (results.Count == 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "EnemyUnit")
                    {
                        EnemyUnit eUnit = hit.collider.GetComponent<EnemyUnit>();
                        SelectEnemyUnitState unit = combatManager.selectedUnitSM.GetComponentInChildren<SelectEnemyUnitState>();
                        unit.selectedEnemy = eUnit;
                        unit.ShowPreBattleInfo(true);
                    }
                    else
                    {
                        SelectEnemyUnitState unit = combatManager.selectedUnitSM.GetComponentInChildren<SelectEnemyUnitState>();
                        uiManager.ShowSelectableEnemiesPanel(unit);

                        camManager.TurnOnFocusCam(combatManager.selectedUnitSM.transform);
                    }
                }
            }
            /*
            else
            {
                foreach (RaycastResult r in results)
                {
                    print(r.gameObject.name);
                }
            }
            */
        }
    }
}
