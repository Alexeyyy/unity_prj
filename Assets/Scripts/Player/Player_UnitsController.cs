using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player_UnitsController : MonoBehaviour {
    private GameObject[] units;
    private List<GameObject> selected_units;
    private bool f = false;

    private List<GameObject> SeekForSelectedUnits()
    {
        units = GameObject.FindGameObjectsWithTag("gameUnit");
        selected_units = units.Where(n => n.GetComponent<Player_Unit>().IsSelected == true).ToList();
    
        return selected_units;
    }

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
    void Update()
    {
        //Debug.Log(ClickedField.pos_x + " " + ClickedField.pos_y);
        /*if (ClickedField.isClickedCellChanged)
        {
            //
            //Debug.Log(SeekForSelectedUnits().Count);
            if(SeekForSelectedUnits().Count != 0)
            {
                //Делай что-нибудь
                //Debug.Log("Count " + SeekForSelectedUnits().Count);
                if (SeekForSelectedUnits().Where(n => n.GetComponent<Player_Unit>().isReady == true).Count() == SeekForSelectedUnits().Count)
                {
                    foreach (GameObject obj in SeekForSelectedUnits())
                    {
                        obj.GetComponent<Player_Unit>().IsMotionOver = false;
                    }
                    Debug.Log("HUI!");
                    ClickedField.isClickedCellChanged = false;
                }
            }
        }*/
    }
}
