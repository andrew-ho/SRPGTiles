using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    [Header("Menu Buttons")]
    public Button attack_button;
    public Button wait_button, skill_button, items_button;
    [Header("Lena Buttons")]
    public Button Slash;
    public Button CrossSlash;
    public Animator animator;
    public Animator skillsAnimator;
    void Start()
    {
        Button btn1 = attack_button.GetComponent<Button>();
        Button btn2 = wait_button.GetComponent<Button>();
        Button btn3 = skill_button.GetComponent<Button>();
        Button btn4 = items_button.GetComponent<Button>();
        Button btn5 = Slash.GetComponent<Button>();
        Button btn6 = CrossSlash.GetComponent<Button>();
        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        btn1.onClick.AddListener(TaskOnClick);
        btn2.onClick.AddListener(WaitAction);
        btn3.onClick.AddListener(OpenSkillsList);
        btn4.onClick.AddListener(null);

        btn5.onClick.AddListener(() => Skill(btn5));
        btn6.onClick.AddListener(() => Skill(btn6));
    }
    private void Update()
    {
        if (TurnManager.myState == TurnManager.states.PLAYER)
        {
            if (TacticMovement.state == TacticMovement.turnState.WAIT)
            {
                animator.SetBool("isOpen", true);
            }
            else
            {
                animator.SetBool("isOpen", false);
            }
            if (TacticMovement.state == TacticMovement.turnState.SKILLS && ButtonSkillScript.currentSkill == null)
            {
                skillsAnimator.SetBool("SkillsOpen", true);
            }
            else
            {
                skillsAnimator.SetBool("SkillsOpen", false);
            }
        }
    }

    public void Skill(Button button)
    {
        button.GetComponent<SkillUse>().Use();
    }
    void TaskOnClick()
    {
        TacticMovement.state = TacticMovement.turnState.ATTACKING;
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().FindAttTiles(1);
    }
    void OpenSkillsList()
    {
        TacticMovement.state = TacticMovement.turnState.SKILLS;
        skillsAnimator.SetBool("SkillsOpen", true);
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button is clicked
        Debug.Log(message);
    }

    void WaitAction()
    {
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().EndTurn();
        foreach (Tile t in TacticMovement.selectTiles)
        {
            t.Reset();
        }
        foreach (Tile t in TacticMovement.attTiles)
        {
            t.Reset();
        }
        //TurnManager.heroes.Remove(TacticMovement.currentPlayer);
        TacticMovement.currentPlayer.GetComponent<TacticMovement>().hadTurn = true;
        TacticMovement.currentPlayer.GetComponent<Renderer>().material.color = Color.red;
        TacticMovement.currentPlayer = null;
        TacticMovement.state = TacticMovement.turnState.CHECKSTATE;
    }
    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode

        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(TacticMovement.currentPlayer.transform.position - Vector3.up, new Vector3(.5f, .5f, .5f));
    }

}
