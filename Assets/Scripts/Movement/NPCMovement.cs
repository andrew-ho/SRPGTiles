using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : TacticMovement {
    GameObject target;
	// Use this for initialization
	void Start () {
        init();
	}

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);
        if (TurnManager.myState == TurnManager.states.ENEMY)
        {
            if (state == turnState.INITIALIZING)
            {
                /*if (TurnManager.enemies.Count == 0)
                {
                    TurnManager.myState = TurnManager.states.PLAYER;
                }
                else
                {
                    state = turnState.SELECTING;
                }*/
                foreach (GameObject enemy in TurnManager.enemies)
                {
                    enemy.GetComponent<TacticMovement>().hadTurn = false;
                }
                state = turnState.SELECTING;
            }
            if (state == turnState.CHECKSTATE)
            {
                int count = 0;
                foreach (GameObject enemy in TurnManager.enemies)
                {
                    if (enemy.GetComponent<TacticMovement>().hadTurn == true)
                    {
                        count++;
                    }
                }
                if (count == TurnManager.enemies.Count)
                {
                    state = turnState.INITIALIZING;
                    TurnManager.myState = TurnManager.states.PLAYER;
                }
                else
                {
                    state = turnState.SELECTING;
                }
            }
            if (state == turnState.SELECTING)
            {
                //TurnManager.enemies[0].GetComponent<TacticMovement>().BeginTurn();
                foreach (GameObject enemy in TurnManager.enemies)
                {
                    if (enemy.GetComponent<TacticMovement>().hadTurn == false)
                    {
                        enemy.GetComponent<TacticMovement>().BeginTurn();
                        break;
                    }
                }
                state = turnState.MOVING;
            }
            if (!turn)
            {
                return;
            }
            if (state == turnState.MOVING)
            {
                Debug.Log(moving);
                if (!moving)
                {
                    FindNearestTarget();
                    CalculatePath();
                    FindSelectableTiles();
                    actualTargetTile.target = true;
                }
                else if (moving)
                {
                    Move();
                }
            }
            if (state == turnState.WAIT)
            {
                gameObject.GetComponent<TacticMovement>().EndTurn();
                //TurnManager.enemies.Remove(gameObject);
                gameObject.GetComponent<TacticMovement>().hadTurn = true;
                state = turnState.CHECKSTATE;
            }
        }
    }

    void checkAttack()
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(target.transform.position + Vector3.right, halfExtents);


    }
    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach(GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);
            //Vector3.SqrMagnitude;

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }
}
