﻿using UnityEngine;
using System.Collections;
using System.IO;

public class Player_Unit : MonoBehaviour {
    public float speed;
    public string name;
    public bool isSelected = false;
    
    private int[,] path = null;
    private int index_motion = 0;
    private bool isMotionOver = true;
        
    //Поиск клетки, где в данный момент находится игрок
    private MapCell GetCurrentCellLocation()
    {
        Vector3 currentUnitPosition = this.transform.position;
        
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
                    return Grid_Manager.S_Instance.Game_Field[i,j];
                }
            }
        }

        return null;
    }

    private void LaunchPathSearch()
    {
        if (!isSelected)
            ClickedField.isClickedCellChanged = false;

        if (isSelected && ClickedField.isClickedCellChanged)
        {
            isMotionOver = true;
            index_motion = 0;
            MapCell startCell = GetCurrentCellLocation();
            MapCell goalCell = Grid_Manager.S_Instance.Game_Field[ClickedField.pos_x, ClickedField.pos_y];

            if (goalCell.Cell_Identificator != 1)
            {
                path = A_Start_PathFinding.FindPath(startCell, goalCell);
                isMotionOver = false;
                ClickedField.isClickedCellChanged = false;
            }
        }
    }

    private void MoveAlongPath()
    {
        if (!isMotionOver)
        {
            if (!isBuilt)
            {
                isBuilt = true;
            }
            if (this.transform.position != Grid_Manager.S_Instance.Game_Field[path[index_motion, 0], path[index_motion, 1]].Cell_Object.transform.position)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, Grid_Manager.S_Instance.Game_Field[path[index_motion, 0], path[index_motion, 1]].Cell_Object.transform.position, speed * Time.deltaTime);
            }
            else
            {
                index_motion++;
                if (index_motion > path.GetLength(0) - 1)
                {
                    path = null;
                    index_motion = 0;
                    isMotionOver = true;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        isSelected = true;
    }
    
	// Use this for initialization
	private void Start () {
        index_motion = 0;
	}

    private bool isBuilt = false;

	// Update is called once per frame
	private void Update () {
        //если выбрали юнита и кликнули куда ему идти то
        LaunchPathSearch();
        MoveAlongPath();
	}
}
