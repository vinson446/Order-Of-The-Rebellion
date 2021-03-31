using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Debugger")]
    public bool battleComplete;
    public TacticsUnitSM selectedUnitSM;
    public TacticsUnit lastUnit;

    public bool allyTurn;
    public bool enemyTurn;

    [Header("Fill Ins")]
    public List<TacticsUnitSM> allyTeam;
    public List<TacticsUnitSM> enemyTeam;
    public int allyTeamSize;
    public int enemyTeamSize;

    public Tile[] levelTiles;

    CombatSM combatSM;
    InputManager inputManager;
    CombatUIManager uiManager;
    CameraManager camManager;

    // Start is called before the first frame update
    void Awake()
    {
        combatSM = GetComponentInChildren<CombatSM>();

        levelTiles = FindObjectsOfType<Tile>();
        inputManager = FindObjectOfType<InputManager>();
        uiManager = FindObjectOfType<CombatUIManager>();
        camManager = FindObjectOfType<CameraManager>();

        FillInAllyTeam();
        SetupAllyTeam();
        FillInEnemyTeam();
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    void FillInAllyTeam()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject a = GameObject.Find("Ally " + i.ToString());
            if (a != null)
                allyTeam.Add(a.GetComponentInChildren<TacticsUnitSM>());
        }
    }

    void SetupAllyTeam()
    {
        List<TacticsUnitSM> allyUnitsToRemove = new List<TacticsUnitSM>();

        for (int i = 0; i < 6; i++)
        {
            // set up ally stats if theyre alive, else remove them from allyTeam list
            if (GameManager.instance.units[i].currentHP > 0)
            {
                AllyUnit unitData = GameManager.instance.units[i];
                AllyUnit unitInBattle = allyTeam[i].GetComponentInParent<AllyUnit>();

                unitInBattle.maxHP = unitData.maxHP;
                unitInBattle.currentHP = unitData.currentHP;
                unitInBattle.maxNRG = unitData.maxNRG;
                unitInBattle.currentNRG = unitData.currentNRG;

                unitInBattle.ATK = unitData.ATK;
                unitInBattle.DEF = unitData.DEF;
                unitInBattle.ACC = unitData.ACC;
                unitInBattle.EVA = unitData.EVA;

                unitInBattle.CRIT = unitData.CRIT;
                unitInBattle.MOV = unitData.MOV;

                unitInBattle.gameObject.SetActive(true);

                allyTeamSize++;
            }
            else
            {
                AllyUnit unitInBattle = allyTeam[i].GetComponentInParent<AllyUnit>();
                allyUnitsToRemove.Add(unitInBattle.GetComponentInChildren<TacticsUnitSM>());

                unitInBattle.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < allyUnitsToRemove.Count; i++)
        {
            if (allyTeam.Contains(allyUnitsToRemove[i]))
            {
                allyTeam.Remove(allyUnitsToRemove[i]);
            }
        }

    }

    void FillInEnemyTeam()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach (GameObject e in enemies)
        {
            enemyTeam.Add(e.GetComponentInChildren<TacticsUnitSM>());
            enemyTeamSize++;
        }
    }

    public void StartPlayerTurn()
    {
        enemyTurn = false;
        allyTurn = true;

        uiManager.ShowStatePanel("PLAYER TURN", 2, true);

        foreach (TacticsUnitSM unit in allyTeam)
        {
            unit.GetComponent<DefaultUnitState>().ResetFlags();
        }
    }

    public void EndPlayerUnitTurn(TacticsUnitSM unit)
    {
        allyTeam.Remove(unit);

        // change to enemy turn
        if (allyTeam.Count == 0)
        {
            FillInAllyTeam();
            StartEnemyTurn();
        }
        else
        {
            camManager.DefaultCamLookAtNextUnit();
        }
    }

    public void StartEnemyTurn()
    {
        allyTurn = false;
        enemyTurn = true;

        uiManager.ShowStatePanel("ENEMY TURN", 2, false);

        foreach (TacticsUnitSM unit in enemyTeam)
        {
            unit.GetComponent<DefaultUnitState>().ResetFlags();
        }

        enemyTeam[0].ChangeState<DefaultUnitState>();
        enemyTeam[0].GetComponent<DefaultUnitState>().GoToMovementState();
    }

    public void StartNextEnemyUnitTurn(TacticsUnitSM unit)
    {
        enemyTeam.Remove(unit);

        if (enemyTeam.Count > 0)
        {
            enemyTeam[0].GetComponent<DefaultUnitState>().GoToMovementState();
        }
        // change to player turn
        else
        {
            FillInEnemyTeam();
            StartPlayerTurn();

            camManager.DefaultCamLookAtNextUnit();
        }
    }

    public void CheckBattleEnd(bool allyDeath, TacticsUnit unit)
    {
        if (allyDeath)
        {
            allyTeam.Remove(unit.GetComponentInChildren<TacticsUnitSM>());
            Destroy(unit.transform.gameObject);
            allyTeamSize--;

            if (allyTeamSize == 0)
            {
                combatSM.GetComponent<EndCombatState>().LoseBattle();
                combatSM.ChangeState<EndCombatState>();

                battleComplete = true;
            }
        }
        else
        {
            enemyTeam.Remove(unit.GetComponentInChildren<TacticsUnitSM>());
            Destroy(unit.transform.gameObject);
            enemyTeamSize--;

            if (enemyTeamSize == 0)
            {
                combatSM.GetComponent<EndCombatState>().WinBattle();
                combatSM.ChangeState<EndCombatState>();

                battleComplete = true;
            }
        }
    }

    public void ResetLevelTiles()
    {
        foreach (Tile tile in levelTiles)
        {
            tile.Reset();
        }
    }
}
