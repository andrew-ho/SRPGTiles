using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour {
    //Lena Skills
    public static GameObject skillsDatabase;
    public static List<Tile> attTilesToGo = new List<Tile>();
    public void Start()
    {
        skillsDatabase = GameObject.Find("skillsDatabase");
    }

    public void Slash()
    {
        /*GameObject target = TacticMovement.checkForPlayer(TacticMovement.currentPlayer, 0);
        Debug.Log(target);
        if (target == null)
        {
            if (Input.GetButtonDown("x"))
            {
                Debug.Log("todo");
            }
            Debug.Log("TODO");
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject == target)
                    {
                        TacticMovement.currentPlayer.transform.LookAt(target.transform);
                        target.GetComponent<EnemyStats>().HP -= (int)(TacticMovement.currentPlayer.GetComponent<BaseStats>().att * 1.5);
                        TacticMovement.state = TacticMovement.turnState.CHECKSTATE;
                    }
                }
            }
            
        }*/
        //TacticMovement.currentPlayer.GetComponent<TacticMovement>().FindAttTiles(3);
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyStats>().hittable)
                {
                    //typeof(Skills).GetMethod(ButtonSkillScript.currentSkill).Invoke(GameObject.Find("skillsDatabase").GetComponent<Skills>(), null);
                    //Debug.Log("hit hit hit");
                    hit.collider.gameObject.GetComponent<EnemyStats>().HP -= 50;
                    TacticMovement.currentPlayer.GetComponent<TacticMovement>().EndTurn();
                    TacticMovement.currentPlayer = null;
                    TacticMovement.state = TacticMovement.turnState.CHECKSTATE;
                    
                }
            }
        }
    }

    public void CrossSlash()
    {

    }
    //ALice Skills
    public void Heal()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    hit.collider.gameObject.GetComponent<BaseStats>().HP += 100;
                }
            }
        }
    }

    public void Fire()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<BaseStats>().HP -= 100;
                }
            }
        }
    }
}
