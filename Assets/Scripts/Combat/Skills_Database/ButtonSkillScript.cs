using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class ButtonSkillScript : MonoBehaviour, SkillUse {
    public int range;
    public static string currentSkill;
    public void Use()
    {
        TacticMovement.state = TacticMovement.turnState.SKILLS;
        //typeof(Skills).GetMethod(gameObject.name).Invoke(GameObject.Find("skillsDatabase").GetComponent<Skills>(), null);
        currentSkill = gameObject.name;
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().FindAttTiles(range);
    }
}
