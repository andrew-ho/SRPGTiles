using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticMovement : MonoBehaviour {
    public bool turn = false;
    public static List<Tile> selectTiles = new List<Tile>();
    public static List<Tile> attTiles = new List<Tile>();
    GameObject[] tiles;

    public Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public Animator animator;
    public bool action = false;
    public Vector3 currentPos;

    public bool falling = false;
    public bool jumpUp = false;
    public bool movingEdge = false;
    public int attRange = 1;

    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    public bool hadTurn = false;
    public bool alive = true;

    public float jumpVelocity = 4.5f;
    float halfHeight = 0;
    Vector3 jumpTarget;
    public Tile actualTargetTile;
    public static GameObject currentPlayer = null;

    public enum turnState
    {
        INITIALIZING,
        CHECKSTATE,
        SELECTING,
        FINDINGPATH,
        MOVING,
        WAIT,
        ATTACKING
    };

    public static turnState state;
    public void init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        state = turnState.SELECTING;
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        //TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
        currentPos = transform.localPosition;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }
        return tile;
    }

    public void ComputeAdjList(float jumpHeight, Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.findNeighbor(jumpHeight, target);
        }
    }

    public void FindSelectableTiles()
    {
        ComputeAdjList(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = null;
        Queue<Tile> attT = new Queue<Tile>();
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            selectTiles.Add(t);
                
            t.selectable = true;
            if (t.distance == move)
            {
                attT.Enqueue(t);
            }
            //t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }                
            }
        }
        //Debug.Log(attT.Count);
        while (attT.Count > 0)
        {
            Tile t = attT.Dequeue();
            foreach (Tile tile in t.adjList)
            {
                if (!tile.visited)
                {
                    attTiles.Add(tile);
                    tile.parent = t;
                    tile.visited = true;
                    tile.distance = 1 + t.distance;
                    tile.attSpace = true;
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;
        state = turnState.MOVING;
        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;
            //calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= .05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Debug.Log("JUMP!!!!!!");
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            removeSelectTiles();
            RemoveAttTiles();
            moving = false;
            state = turnState.WAIT;
            //state = turnState.MOVING;
            animator.SetBool("isOpen", true);
            currentTile = null;
            //TurnManager.done = true;
            //this.GetComponent<Renderer>().material.color = Color.red;
            //TurnManager.EndTurn();
            if (TurnManager.myState == TurnManager.states.PLAYER)
            {
                //this.EndTurn();
                //state = turnState.WAIT;
                //animator.SetBool("isOpen", false);
                //TurnManager.heroes.Remove(this.gameObject);
            }
            if (TurnManager.myState == TurnManager.states.ENEMY)
            {
                //this.EndTurn();
                //TurnManager.enemies.Remove(this.gameObject);
            }
        }
    }
    //reset tiles meant for attacking
    public void RemoveAttTiles()
    {
        foreach(Tile tile in attTiles)
        {
            tile.Reset();
        }
        attTiles.Clear();
    }
    //reset tiles meant for selecting where to move
    public void removeSelectTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        foreach(Tile tile in selectTiles)
        {
            tile.Reset();
        }

        selectTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }
    //Self-explanatory
    void Jump(Vector3 target)
    {
        if (falling)
        {
            FallDownward(target);
        }
        else if (jumpUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }
    //Self-explanatory
    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            falling = false;
            jumpUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            falling = false;
            jumpUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }
    //Self-explanatory
    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            falling = false;
            jumpUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }
    //Self-explanatory
    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpUp = false;
            falling = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            falling = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    //A* pathfinding for enemynpc
    public Tile FindLowestF(List<Tile> list)
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

    public Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }
        if (tempPath.Count <= move)
        {
            return t.parent;
        }
        Tile endTile = null;
        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }
        return endTile;
    }
    public void FindPath(Tile target)
    {
        ComputeAdjList(jumpHeight, target);
        GetCurrentTile();
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }
            foreach (Tile tile in t.adjList)
            {
                if (closedList.Contains(tile))
                {
                    continue;
                }
                else if (openList.Contains(tile))
                {
                    float temp = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    if (temp < tile.g)
                    {
                        tile.parent = t;
                        tile.g = temp;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;
                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }
    }
    //Begins turn for enemy or ally
    public void BeginTurn()
    {
        turn = true;
    }
    //Ends turn for enemy or ally
    public void EndTurn()
    {
        turn = false;
    }
}
