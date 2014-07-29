using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player_UnitsController : MonoBehaviour {
    private GameObject[] units;
    private List<GameObject> selected_units;

    public bool isPathCalculated = false;
    
    //Определение тех юнитов, которые выделены в данный момент
    private List<GameObject> SeekForSelectedUnits()
    {
        units = GameObject.FindGameObjectsWithTag("gameUnit");
        selected_units = units.Where(n => n.GetComponent<Player_Unit>().IsSelected == true).ToList();
    
        return selected_units;
    }

    //Поиск клетки, где в данный момент находится игрок
    private MapCell GetCurrentCellLocation(GameObject fUnit)
    {
        Vector3 currentUnitPosition = fUnit.transform.position;

        //Проверям, какой клетке принадлежит юнит
        for (int i = 0; i < Grid_Manager.S_Instance.Game_Field.GetLength(0); i++)
        {
            for (int j = 0; j < Grid_Manager.S_Instance.Game_Field.GetLength(1); j++)
            {
                if (currentUnitPosition.x < Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.x + Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.x / 2
                    && currentUnitPosition.x > Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.x - Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.x / 2
                    && currentUnitPosition.y > Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.y - Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.y / 2
                    && currentUnitPosition.y < Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.y + Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.y / 2)
                {
                    return Grid_Manager.S_Instance.Game_Field[i, j];
                }
            }
        }

        return null;
    }

    //Функция вычисления путей для юнитов
    private void CalculatePathes(List<GameObject> fUnits)
    {
        foreach (GameObject unit in fUnits)
        {
            unit.GetComponent<Player_Unit>().Path = A_Start_PathFinding.FindPath(GetCurrentCellLocation(unit), Grid_Manager.S_Instance.Game_Field[ClickedField.pos_x, ClickedField.pos_y]);
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
    void Update()
    {
        if (ClickedField.isClickedCellChanged)
        {
            selected_units = SeekForSelectedUnits();
            if(selected_units.Count != 0)
            {
                CalculatePathes(selected_units);
                isPathCalculated = true;
                ClickedField.isClickedCellChanged = false;
            }
        }
    }
}
