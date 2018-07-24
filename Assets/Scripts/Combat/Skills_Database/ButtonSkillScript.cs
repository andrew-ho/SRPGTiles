using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonSkillScript : SkillUse {
    Skills skill = new Skills();
    public enum SkillList
    {
        Slash,
        CrossSlash,
        Heal,
        Fire
    };
    [SerializeField]
    SkillList skillSelected;
    public void Use()
    {

    }
}
