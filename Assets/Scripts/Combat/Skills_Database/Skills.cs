using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour {
    //Lena Skills
    public static List<Tile> attTilesToGo = new List<Tile>();
    public static Dictionary<string, Action> dict = new Dictionary<string, Action>();
    public void Start()
    {
        dict.Add("Slash", ()=> Slash());
        dict.Add("Heal", () => Heal());
        dict.Add("Fire", () => Fire());
    }

    public void Slash()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Enemy" && hit.collider.gameObject.GetComponent<EnemyStats>().hittable)
                {
                    hit.collider.gameObject.GetComponent<EnemyStats>().HP -= 50;
                    resetState();    
                }
            }
        }
    }

    public void resetState()
    {
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().EndTurn();
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().hadTurn = true;
        TacticMovement.currentPlayer.GetComponent<Renderer>().material.color = Color.red;
        TacticMovement.currentPlayer = null;
        ButtonSkillScript.currentSkill = null;
        foreach (Tile t in TacticMovement.attTiles)
        {
            t.Reset();
        }
        TacticMovement.state = TacticMovement.turnState.CHECKSTATE;
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
