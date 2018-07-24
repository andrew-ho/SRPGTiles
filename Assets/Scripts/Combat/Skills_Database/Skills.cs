using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour {
    //Lena Skills
    public void Slash()
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
