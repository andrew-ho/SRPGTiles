using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonBehavior : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button m_YourFirstButton, m_YourSecondButton, skill_button;

    void Start()
    {
        Button btn1 = m_YourFirstButton.GetComponent<Button>();
        Button btn2 = m_YourSecondButton.GetComponent<Button>();

        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        btn1.onClick.AddListener(TaskOnClick);
        btn2.onClick.AddListener(WaitAction);
        
    }

    void TaskOnClick()
    {
        //Output this to console when the Button is clicked
        //Debug.Log(TacticMovement.currentPlayer.transform.position);
        Collider[] colliders = Physics.OverlapBox(TacticMovement.currentPlayer.transform.position - Vector3.up, new Vector3(.2f, .2f, .2f));
        //Debug.Log(colliders.Length);
        Tile tile = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Tile>() != null)
            {
                tile = colliders[i].gameObject.GetComponent<Tile>();
            }
        }
        //Debug.Log("Tile " + tile);
        //Debug.Log(tile.transform.position);
        foreach (Tile t in TacticMovement.selectTiles)
        {
            t.Reset();
        }
        foreach (Tile t in TacticMovement.attTiles)
        {
            t.Reset();
        }
        TacticMovement.state = TacticMovement.turnState.ATTACKING;
        tile.CheckAttackTilesMelee(Vector3.forward, 0, tile);
        tile.CheckAttackTilesMelee(-Vector3.forward, 0, tile);
        tile.CheckAttackTilesMelee(Vector3.right, 0, tile);
        tile.CheckAttackTilesMelee(Vector3.left, 0, tile);
        //TacticMovement.state = TacticMovement.turnState.ATTACKING;
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
