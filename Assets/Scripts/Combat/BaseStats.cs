using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStats : MonoBehaviour {

    public int HP;
    public int MP;
    public int TP;
    public int att;
    public int def;
    public int move;

    public void setHP(int damage)
    {
        HP = HP - damage;
    }
    public void setMP(int MPUse)
    {
        MP = MP - MPUse;
    }
    public void setTP(int TPUse)
    {
        TP = TP - TPUse;
    }
}
