using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : TacticsMovement
{
    EnemyUnit enemyUnit;

    GameObject target;
    Tile targetTile;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        enemyUnit = GetComponent<EnemyUnit>();
        moveSpaces = enemyUnit.MOV;
        moveSpeed = enemyUnit.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myMovePhase)
            return;

        if (!isMoving)
        {
            FindNearestTarget();
            CalculatePath();

            if (CheckToShowSelectableTiles())
                FindSelectableTiles(true, true, false);

            // actualTargetTile.isTarget = true;
        }
        else
        {
            Move();
        }
    }

    // find closest player unit to move to and attack
    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("AllyUnit");

        // look for nearest unit
        GameObject nearestTarget = null;
        float distance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float d = Vector3.Distance(transform.position, target.transform.position);

            if (d < distance)
            {
                nearestTarget = target;
                distance = d;
            }
        }

        target = nearestTarget;
    }

    void CalculatePath()
    {
        targetTile = GetTargetTile(target);
        FindPathForEnemy(targetTile);
    }

    bool CheckToShowSelectableTiles()
    {
        if (currentTile.adjacencyList.Contains(targetTile))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
