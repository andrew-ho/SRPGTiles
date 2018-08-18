using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour, SkillUse {
    public void Use()
    {
        GameObject target = TacticMovement.checkForPlayer(TacticMovement.currentPlayer, 0);
        if (target == null)
        {
            if (Input.GetButtonDown("x"))
            {

            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        TacticMovement.currentPlayer.transform.LookAt(target.transform);
                        target.GetComponent<EnemyStats>().HP -= (int)(TacticMovement.currentPlayer.GetComponent<BaseStats>().att * 1.5);
                    }
                }
            }
        }
    }
}
