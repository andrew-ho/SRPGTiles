using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;
    public bool attSpace = false;
    public bool nonSelect = true;
    public bool attackable = false;

    public List<Tile> adjList = new List<Tile>();
    public static List<Tile> newTiles = new List<Tile>();

    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //A*
    public float f, g, h = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
		else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else if (attSpace)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (attackable)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
        else if (nonSelect)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
	}

    public void Reset()
    {
        adjList.Clear();
        visited = false;
        current = false;
        target = false;
        selectable = false;
        parent = null;
        distance = 0;
        attSpace = false;
        nonSelect = true;
        attackable = false;
        f = g = h = 0;
    }

    public void findNeighbor(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);

    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            RaycastHit hit;
            if (TacticMovement.state == TacticMovement.turnState.SKILLS)
            {

                if (tile != null)
                {
                    if (!Physics.Raycast(tile.transform.position, -Vector3.up, out hit, 1) || (tile == target))
                    {
                        adjList.Add(tile);
                    }
                }
            }
            else
            {
                if (tile != null)
                {
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                    {
                        adjList.Add(tile);
                    }
                }
            }
        }
    }

    public void CheckAttackTilesMelee(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        //Debug.Log(target.transform.position);
        Collider[] colliders = Physics.OverlapBox(target.transform.position + direction, halfExtents);
        foreach (Collider item in colliders)
        {
            //Debug.Log(item.gameObject.name);
            Tile tile = item.GetComponent<Tile>();
            //Debug.Log(tile);
            if (item.tag == "Enemy")
            {
                item.GetComponent<EnemyStats>().hittable = true;
            }
            if (tile != null)
            {
                //Debug.Log(GetComponent<Renderer>());
                tile.nonSelect = false;
                newTiles.Add(tile);
                item.gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }
}
