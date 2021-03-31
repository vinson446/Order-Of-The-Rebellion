using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMovement : MonoBehaviour
{
    [Header("Debugger")]
    public bool myMovePhase;
    public bool hasMoved;

    // move settings
    [SerializeField] protected bool isMoving;
    protected float moveSpeed;
    protected int moveSpaces = 4;

    // to clear tiles after unit moves
    public List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    protected Tile currentTile;
    protected Vector3 lastPos;
    protected Quaternion lastRot;

    // how fast player moves
    Vector3 velocity;
    // dir player is heading in
    Vector3 heading;

    // used to set unit on top of tile
    float halfHeight;

    // for A*- enemy pathfinding
    protected Tile actualTargetTile;

    // references
    TacticsUnitSM unitStateMachine;
    CombatManager combatManager;

    protected virtual void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        unitStateMachine = GetComponentInChildren<TacticsUnitSM>();
        combatManager = FindObjectOfType<CombatManager>();
    }

    public void StartMovePhase()
    {
        myMovePhase = true;
    }

    public void SetLastUnitLocation()
    {
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    public void EndMovePhase()
    {
        myMovePhase = false;

        if (hasMoved)
            unitStateMachine.ChangeState<SelectActionUnitState>();
    }

    // helper function for FindSelectableTiles() and FindPath()
    protected void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.isCurrentlyUsed = true;
    }

    // helper function for GetCurrentTile() and CalculatePath() for enemies
    protected Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    // helper function- figure out if a tile is selectable
    void ComputeAdjacencyList(Tile target, bool checkColl)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target, checkColl);
        }
    }

    // BFS
    // for player and enemy- limit unit's selectable tiles to unit's # of moveSpaces
    public void FindSelectableTiles(bool showTiles, bool checkColl)
    {
        ComputeAdjacencyList(null, checkColl);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        // leave currentTile's parent as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);

            if (showTiles)
            {
                if (checkColl)
                    t.isSelectable = true;
                else
                    t.isAttackable = true;
            }

            if (t.distance < moveSpaces)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parentTile = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;

                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    // for player and enemy
    public void CreatePathToTargetTile(Tile tile)
    {
        path.Clear();

        tile.isTarget = true;
        isMoving = true;

        // start at end location
        Tile next = tile;
        while (next != null)
        {
            // add tiles from end location to start location on stack to traverse path 
            path.Push(next);
            next = next.parentTile;
        }
    }

    // for player and enemy
    protected void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            // calculate unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.1f)
            {
                CalculateHeading(target);
                SetVelocity();

                // set dir and then move to tile
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            // reach destination
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            isMoving = false;
            hasMoved = true;

            EndMovePhase();
        }
    }

    public void ResetMove()
    {
        transform.position = lastPos;
        transform.rotation = lastRot;
    }

    // helper function for Move()
    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    // helper function for Move()
    void SetVelocity()
    {
        velocity = heading * moveSpeed;
    }

    // helper function for Move()
    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.isCurrentlyUsed = false;

            // for ai who has nowhere to move
            currentTile.isTarget = false;

            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    // for enemy
    protected void FindPathForEnemy(Tile target)
    {
        ComputeAdjacencyList(target, true);
        GetCurrentTile();

        List<Tile> unprocessedList = new List<Tile>();
        List<Tile> processedList = new List<Tile>();

        unprocessedList.Add(currentTile);

        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;
        // currentTile.G = 0 already since currentTile is the starting tile

        while (unprocessedList.Count > 0)
        {
            Tile t = FindLowestF(unprocessedList);
            processedList.Add(t);

            // found target
            if (t == target)
            {
                actualTargetTile = FindActualEndTile(t);
                CreatePathToTargetTile(actualTargetTile);

                return;
            }
            
            foreach (Tile tile in t.adjacencyList)
            {
                // if tile in processed list, do nothing
                if (processedList.Contains(tile))
                {

                }
                // if tile in unprocessed list, compare g scores to see which path is closer
                else if (unprocessedList.Contains(tile))
                {
                    // G- distance from starting tile to current tile
                    float tmpG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tmpG < tile.g)
                    {
                        tile.parentTile = t;

                        tile.g = tmpG;
                        tile.f = tile.g + tile.h;
                    }
                }
                // if tile not in unprocessed or processed list, add tile to unprocessed list
                else
                {
                    tile.parentTile = t;
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    unprocessedList.Add(tile);
                }
            }
        }

        // TODO - wat do u do if there is no path to target tile
        Debug.Log("Path not found");
    }

    // helper function for FindPathForEnemy()
    Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    // need to look at dis to fix enemy ranges
    // helper function for FindPathForEnemy()
    Tile FindActualEndTile(Tile t)
    {
        Stack<Tile> tmpPath = new Stack<Tile>();

        Tile next = t.parentTile;
        while (next != null)
        {
            tmpPath.Push(next);
            next = next.parentTile;
        }

        Tile endTile = null;
        for (int i = 0; i <= Random.Range(1, moveSpaces); i++)
        {
            if (tmpPath.Count > 0)
                endTile = tmpPath.Pop();
            else
                endTile = t.parentTile;
        }

        return endTile;

        /*
        // target is within range of moveSpaces
        if (tmpPath.Count <= moveSpaces)
        {
            return t.parentTile;
        }
        // target is out of movement range
        else
        {
            Tile endTile = null;
            for (int i = 0; i <= Random.Range(1, moveSpaces); i++)
            {
                endTile = tmpPath.Pop();
            }

            return endTile;
        }
        */
    }
};