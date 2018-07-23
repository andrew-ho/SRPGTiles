using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : TacticMovement
{
    public enum states { PLAYER, ENEMY };
    public static states myState;
    public static List<GameObject> heroes = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();
    public void Start()
    {
        myState = states.PLAYER;
        GameObject[] heroTeam;
        heroTeam = GameObject.FindGameObjectsWithTag("Player");
        heroes.AddRange(heroTeam);
        GameObject[] enemyTeam = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.AddRange(enemyTeam);
        Debug.Log("enemies.count " + enemies.Count);
    }

    public void FixedUpdate()
    {
        Debug.Log("myState " + myState);
        if (myState == states.PLAYER)
        {
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
