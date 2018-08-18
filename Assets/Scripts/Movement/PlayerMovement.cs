using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : TacticMovement {

    public Animator MenuAnimator;
    public Animator SkillsAnimator;
    // Use this for initialization
    void Start () {
        init();
	}

    void Initialize()
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
    }

    void CheckState()
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
    // Update is called once per frame
    void FixedUpdate () {
        Debug.Log(TurnManager.myState + " " + state);
        if (TurnManager.myState == TurnManager.states.PLAYER)
        {
            if (state == turnState.INITIALIZING)
            {
                Initialize();
            }
            if (state == turnState.CHECKSTATE)
            {
                CheckState();
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
                        ResetTiles();
                        currentPlayer = null;
                        gameObject.GetComponent<TacticMovement>().EndTurn();
                        state = turnState.SELECTING;
                    }
                    else if (Input.GetKeyDown("z"))
                    {
                        ResetTiles();
                        state = turnState.WAIT;
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
            else if (state == turnState.SKILLS)
            {
                //Code to be place in a MonoBehaviour with a GraphicRaycaster component
                //GraphicRaycaster gr = this.GetComponent<GraphicRaycaster>();
                //Create the PointerEventData with null for the EventSystem
                //PointerEventData ped = new PointerEventData(null);
                //Set required parameters, in this case, mouse position
                //ped.position = Input.mousePosition;
                //Create list to receive all results
                //List<RaycastResult> results = new List<RaycastResult>();
                //Raycast it
                //gr.Raycast(ped, results);
                /*if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyStats>().hittable)
                        {
                            //typeof(Skills).GetMethod(ButtonSkillScript.currentSkill).Invoke(GameObject.Find("skillsDatabase").GetComponent<Skills>(), null);
                        }
                    }
                }*/
                if (ButtonSkillScript.currentSkill != null)
                {
                    typeof(Skills).GetMethod(ButtonSkillScript.currentSkill).Invoke(GameObject.Find("skillsDatabase").GetComponent<Skills>(), null);
                }
            }
        }
	}

    void ResetTiles()
    {
        foreach (Tile t in TacticMovement.selectTiles)
        {
            t.Reset();
        }
        foreach (Tile t in TacticMovement.attTiles)
        {
            t.Reset();
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
                if (hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyStats>().hittable)
                {
                    AttackScript attk = currentPlayer.GetComponent<AttackScript>();
                    EnemyStats enemyHP = hit.collider.gameObject.GetComponent<EnemyStats>();
                    attk.Attack(enemyHP);
                    currentPlayer.GetComponent<TacticMovement>().hadTurn = true;
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    currentPlayer.GetComponent<TacticMovement>().EndTurn();
                    hit.collider.gameObject.GetComponent<EnemyStats>().hittable = false;
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
