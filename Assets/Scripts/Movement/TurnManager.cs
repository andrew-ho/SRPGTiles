using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : TacticMovement
{

    //static Dictionary<string, List<TacticMovement>> units = new Dictionary<string, List<TacticMovement>>();
    //static Queue<string> turnKey = new Queue<string>();
    //static Queue<TacticMovement> turnTeam = new Queue<TacticMovement>();
    //GameObject[] currentTeam;
    public static bool done = false;
    public enum states { PLAYER, ENEMY };
    public static states myState;
    public static List<GameObject> heroes = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();
    public void Start()
    {
        //heroTeam = GameObject.FindGameObjectsWithTag("Player");
        //enemyTeam = GameObject.FindGameObjectsWithTag("NPC");
        myState = states.PLAYER;
        GameObject[] heroTeam;
        heroTeam = GameObject.FindGameObjectsWithTag("Player");
        heroes.AddRange(heroTeam);
        GameObject[] enemyTeam = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.AddRange(enemyTeam);
        Debug.Log("enemies.count " + enemies.Count);
        bool lost = false;
    }

    public void FixedUpdate()
    {
        //Debug.Log(heroes.ToString());
        Debug.Log("myState " + myState);
        if (myState == states.PLAYER)
        {

            /*if (heroes.Count == 0)
            {
                //Debug.Log("Wtf?");
                FindEnemies();
                myState = states.ENEMY;
            }

            else
            {
                foreach (GameObject obj in heroes)
                {
                    obj.GetComponent<Renderer>().material.color = Color.white;
                }

            }*/
            foreach (GameObject obj in enemies)
            {
                if (obj.GetComponent<EnemyStats>().HP <= 0)
                {
                    enemies.Remove(obj);
                    Destroy(obj);
                }
            }
            if (enemies.Count == 0)
            {
                Debug.Log("You won");
            }
        }
        else if (myState == states.ENEMY)
        {
            /*if (enemies.Count == 0)
            {
                FindAllies();
                //myState = states.PLAYER;
            }
            else
            {
                foreach (GameObject obj in enemies)
                {
                    //obj.GetComponent<TacticMovement>().BeginTurn();
                }
            }*/
            int count = 0;
            foreach (GameObject obj in heroes)
            {
                if (obj.GetComponent<BaseStats>().HP <= 0)
                {
                    obj.GetComponent<TacticMovement>().alive = false;
                    count++;
                }
            }
            if (count == heroes.Count)
            {
                Debug.Log("You lost");
            }
        }
    }

    public void FindAllies()
    {
        heroes.Clear();
        GameObject[] heroTeam;
        heroTeam = GameObject.FindGameObjectsWithTag("Player");
        heroes.AddRange(heroTeam);
    }

    public void FindEnemies()
    {
        enemies.Clear();
        GameObject[] enemyTeam;
        enemyTeam = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.AddRange(enemyTeam);
    }
}
    /*// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
		
	}

    static void InitTeamTurnQueue()
    {
        List<TacticMovement> teamList = units[turnKey.Peek()];
        foreach (TacticMovement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }
        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        TacticMovement unit = turnTeam.Dequeue();
        unit.EndTurn();
        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(TacticMovement unit)
    {
        List<TacticMovement> list;
        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticMovement>();
            units[unit.tag] = list;
            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }*/

