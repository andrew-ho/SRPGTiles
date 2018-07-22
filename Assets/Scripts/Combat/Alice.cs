using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice : BaseStats, AttackScript
{
    public void Attack(EnemyStats enemy)
    {

        enemy.HP = enemy.HP - att;
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
    }
}
