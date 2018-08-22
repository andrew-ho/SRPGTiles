using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : TacticMovement
{

    public Animator aliceAnimator;
    public Animator lenaAnimator;
    // Use this for initialization
    void Start()
    {
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
    public void AnimationUpdates()
    {
        /*if (state == turnState.MOVING && TacticMovement.currentPlayer.name == "Alice")
        {
            aliceAnimator.SetBool("isMoving", true);
        }
        else
        {
            aliceAnimator.SetBool("isMoving", false);
        }
        if (state == turnState.MOVING && TacticMovement.currentPlayer.name == "Lena")
        {
            lenaAnimator.SetBool("isLenaMoving", true);
        }
        else
        {
            lenaAnimator.SetBool("isLenaMoving", false);
        }*/
    }
    void FixedUpdate()
    {
        Debug.Log(TurnManager.myState + " " + state);
        AnimationUpdates();
        PlayerStages();
    }

    public void PlayerStages()
    {
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
                if (ButtonSkillScript.currentSkill != null)
                {
                    //typeof(Skills).GetMethod(ButtonSkillScript.currentSkill).Invoke(GameObject.Find("skillsDatabase").GetComponent<Skills>(), null);
                    Skills.dict[ButtonSkillScript.currentSkill]();
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
            foreach (Tile t in TacticMovement.attTiles)
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
                    currentPlayer.GetComponent<AttackScript>().Attack(hit.collider.gameObject.GetComponent<EnemyStats>());
                    currentPlayer.GetComponent<TacticMovement>().hadTurn = true;
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                    currentPlayer.GetComponent<TacticMovement>().EndTurn();
                    hit.collider.gameObject.GetComponent<EnemyStats>().hittable = false;
                    currentPlayer = null;
                    foreach (Tile t in TacticMovement.attTiles)
                    {
                        t.Reset();
                    }
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
