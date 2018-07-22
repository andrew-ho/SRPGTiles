using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticMovement {

    // Use this for initialization
    void Start () {
        init();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.Log(TurnManager.myState + " " + state);
        //Debug.Log(currentPlayer);
        //Debug.Log("current gameObject 2" + gameObject);
        if (TurnManager.myState == TurnManager.states.PLAYER)
        {
            if (state == turnState.INITIALIZING)
            {
                foreach (GameObject hero in TurnManager.heroes)
                {
                    if (hero.GetComponent<TacticMovement>().alive == true)
                    {
                        hero.GetComponent<TacticMovement>().hadTurn = false;
                        hero.GetComponent<Renderer>().material.color = Color.white;
                    }
                }
                state = turnState.SELECTING;
                //state = turnState.SELECTING;
                /*if (TurnManager.heroes.Count == 0)
                {
                    TurnManager.myState = TurnManager.states.ENEMY;
                }
                else
                {
                    state = turnState.SELECTING;
                }*/
                //foreach (GameObject hero in TurnManager.heroes)
                //{
                    //Debug.Log("hadTurn " + hero.GetComponent<TacticMovement>().hadTurn);
                    /*if (hero.GetComponent<TacticMovement>().hadTurn == false)
                    {
                        state = turnState.SELECTING;
                        break;
                    }
                    else
                    {
                        TurnManager.myState = TurnManager.states.ENEMY;
                    }*/
                //}

            }
            if (state == turnState.CHECKSTATE)
            {
                int count = 0;
                foreach (GameObject hero in TurnManager.heroes)
                {
                    if (hero.GetComponent<TacticMovement>().hadTurn == true)
                    {
                        count++;
                    }
                }
                if (count == TurnManager.heroes.Count)
                {
                    state = turnState.INITIALIZING;
                    TurnManager.myState = TurnManager.states.ENEMY;
                }
                else
                {
                    state = turnState.SELECTING;
                }
            }
            if (state == TacticMovement.turnState.SELECTING)
            {
                PlayerTurnMoving();
            }
            if (!turn)
            {
                return;
            }
            if (state == turnState.FINDINGPATH)
            {
                if (!moving)
                {
                    if (Input.GetKeyDown("x"))
                    {
                        foreach (Tile t in TacticMovement.selectTiles)
                        {
                            t.Reset();
                        }
                        foreach (Tile t in TacticMovement.attTiles)
                        {
                            t.Reset();
                        }
                        currentPlayer = null;
                        gameObject.GetComponent<TacticMovement>().EndTurn();
                        state = turnState.SELECTING;
                    }
                    else
                    {
                        FindSelectableTiles();
                        checkMouse();
                    }
                }
            }
            else if (state == turnState.MOVING)
            {
                Move();
            }
            else if (state == turnState.WAIT)
            {
                PlayerTurnWaiting();
            }
            else if (state == turnState.ATTACKING)
            {

                PlayerTurnAttacking();
            }
        }
	}

    void PlayerTurnMoving()
    {

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {
                        if (PlayerMovement.currentPlayer == null)
                        {
                            if (PlayerMovement.state == TacticMovement.turnState.SELECTING)
                            {
                                if (TurnManager.heroes.Contains(hit.collider.gameObject) && hit.collider.gameObject.GetComponent<TacticMovement>().hadTurn == false)
                                {
                                    PlayerMovement.currentPlayer = hit.collider.gameObject;

                                    hit.collider.gameObject.GetComponent<TacticMovement>().BeginTurn();
                                    state = TacticMovement.turnState.FINDINGPATH;
                                }
                            }
                        }
                    }
                }
            }
        
    }

    void PlayerTurnWaiting()
    {
        if (Input.GetKeyDown("x"))
        {
            this.gameObject.transform.position = currentPos;
            animator.SetBool("isOpen", false);
            this.gameObject.GetComponent<TacticMovement>().EndTurn();
            state = turnState.SELECTING;
            currentPlayer = null;
        }
    }

    void PlayerTurnAttacking()
    {
        if (Input.GetKeyDown("x"))
        {
            foreach (Tile t in Tile.newTiles)
            {
                t.Reset();
            }
            state = turnState.WAIT;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyStats>().hittable)
                {
                    AttackScript attk = currentPlayer.GetComponent<AttackScript>();
                    EnemyStats enemyHP = hit.collider.gameObject.GetComponent<EnemyStats>();
                    //Debug.Log("ally attack " + attk);
                    //Debug.Log("enemy stats " + enemyHP);
                    attk.Attack(enemyHP);
                    //TurnManager.heroes.Remove(currentPlayer);
                    currentPlayer.GetComponent<TacticMovement>().hadTurn = true;
                    //currentPlayer.GetComponent<Renderer>().material.color = Color.red;
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    currentPlayer.GetComponent<TacticMovement>().EndTurn();
                    currentPlayer = null;
                    foreach (Tile t in Tile.newTiles)
                    {
                        t.Reset();
                    }
                    Debug.Log(hit.collider.gameObject.GetComponent<EnemyStats>().HP);
                    state = turnState.CHECKSTATE;
                }
            }
        }
    }
    void checkMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
}
