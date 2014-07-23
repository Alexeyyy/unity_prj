﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Grid_Manager : MonoBehaviour {
    private static Grid_Manager s_Instance = null;
    public static Grid_Manager S_Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(Grid_Manager)) as Grid_Manager;
                if (s_Instance == null)
                {
                    Debug.Log("Cannot notice the GridManager!");
                }
            }
            return s_Instance;
        }
    }

	public GameObject grid_cell; //"объект" сетки поля
	public GameObject background; //картинка на заднем плане
	private Vector2 initial_point; //стартовые координаты для формирования сетки 
	public int grid_width; //ширина сетки (число "квадратиков" сетки по ширине)
	public int grid_height; //высота сетки (число "квадратиков" сетки по высоте)

    private GameObject[,] grid_cells;
    public GameObject[,] Grid_Cells { get { return grid_cells; } }
	
    private int[,] grid; //игровое поле в виде int[,], где 0 - проходимый, 1 - непроходимый
	public int[,] Grid { get { return grid; } set { grid = value; } }
    
    private bool f_initialFormation = false; //для изначального формирования поля

    /*Функция построения сетки из пустых объектов с тегами gridCell.*/
    private void InitGrid()
    {
        string str = string.Empty;
        int row = grid_height - 1, col = 0;
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                var cell = Instantiate(grid_cell, new Vector3(grid_cell.GetComponent<BoxCollider2D>().size.x * j, 
                                                   grid_cell.GetComponent<BoxCollider2D>().size.y * i, 
                                                   0), Quaternion.identity);
                grid_cells[row, col] = cell as GameObject;
                grid_cells[row, col].GetComponent<Grid_ObjectDetection>().X_Pos = row;
                grid_cells[row, col].GetComponent<Grid_ObjectDetection>().Y_Pos = col;
                col++;
            }
            row--;
            col = 0;
        }
    }

    /*Функция заполнения сетки 1 и 0*/
    private void FillGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = grid_cells[i, j].GetComponent<Grid_ObjectDetection>().Detector;
            }
        }
    }
   
	void Awake() 
    {
        initial_point = new Vector2(0, 0);//background.transform.position;
        grid = new int[grid_height, grid_width];
        grid_cells = new GameObject[grid_height, grid_width];
	}

	// Use this for initialization
	void Start() 
    {
		InitGrid();
	}
    
	// Update is called once per frame
	void Update() 
    {
        if (!f_initialFormation)
        {
            FillGrid();
            //Servant.PrintArrayToFile(grid, "path.txt");
            f_initialFormation = true;
        }
	}
}
