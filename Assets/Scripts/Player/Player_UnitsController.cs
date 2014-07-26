using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player_UnitsController : MonoBehaviour {
    private GameObject[] units;
    private List<GameObject> selected_units;

    private List<GameObject> SeekForSelectedUnits()
    {
        units = GameObject.FindGameObjectsWithTag("gameUnit");
        selected_units = units.Where(n => n.GetComponent<Player_Unit>().isSelected == true).ToList();

        return selected_units;
    }

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (ClickedField.isClickedCellChanged)
        {
            if(SeekForSelectedUnits().Count != 0)
            {
                //Делай что-нибудь

            }
        }
    }
}
