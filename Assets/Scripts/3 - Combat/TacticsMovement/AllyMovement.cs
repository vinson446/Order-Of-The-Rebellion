using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : TacticsMovement
{
    AllyUnit allyUnit;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        allyUnit = GetComponent<AllyUnit>();
        moveSpaces = allyUnit.MOV;
        moveSpeed = allyUnit.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myMovePhase)
            return;

        if (!isMoving)
        {
            FindSelectableTiles(true, true);
        }
        else
        {
            Move();
        }
    }
}
