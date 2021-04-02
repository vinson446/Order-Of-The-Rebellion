using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Bool Checks")]
    // public bool isWalkable = true;
    public bool isCurrentlyUsed;
    public bool isTarget;
    public bool isSelectable;

    public bool isAttackable;

    public List<Tile> adjacencyList;

    [Header("BFS Settings")]
    public bool visited;
    public Tile parentTile;
    // not part of BFS, but needed for unit's movement spaces
    public int distance;

    [Header("A* Settings")]
    // f is sum of g and h
    public float f;
    // distance from starting tile to current tile
    public float g;
    // distance from current tile to target tile
    public float h;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isCurrentlyUsed)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        */
        if (isTarget)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (isSelectable)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (isAttackable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        isCurrentlyUsed = false;
        isTarget = false;
        isSelectable = false;
        isAttackable = false;

        adjacencyList.Clear();

        // bfs
        visited = false;
        parentTile = null;
        distance = 0;

        // A*
        f = g = h = 0;
    }

    // checkColl is true for moving, false for attacking
    public void FindNeighbors(Tile target, bool checkColl)
    {
        Reset();

        CheckTile(Vector3.forward, target, checkColl);
        CheckTile(-Vector3.forward, target, checkColl);
        CheckTile(Vector3.right, target, checkColl);
        CheckTile(-Vector3.right, target, checkColl);
    }

    // add neighboring, walkable tiles to adjacency list
    public void CheckTile(Vector3 direction, Tile target, bool checkColl)
    {
        Vector3 halfExtents = new Vector3(0.5f, 1, 0.5f);
        Collider[] colls = Physics.OverlapBox(transform.position + (direction * transform.localScale.x), halfExtents);

        foreach (Collider item in colls)
        {
            Tile tile = item.GetComponent<Tile>();

            // check if there is something on top of the tile for movement
            if (checkColl)
            {
                if (tile != null)
                {
                    RaycastHit hit;

                    // if nothing is on top of an adjacent tile, add the tile to the adjacency list
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 5) || tile == target)
                    {
                        adjacencyList.Add(tile);
                    }
                }
            }
            // for finding selectable enemies
            else
            {
                if (tile != null)
                    adjacencyList.Add(tile);
            }
        }
    }

    // test CheckTile() check
    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3.forward * transform.localScale.x), new Vector3(0.5f, 1, 0.5f));
    }
    */
}
