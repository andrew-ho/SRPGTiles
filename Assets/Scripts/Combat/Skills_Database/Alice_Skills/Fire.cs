using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : SkillUse {

	// Use this for initialization
	public void Use()
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
