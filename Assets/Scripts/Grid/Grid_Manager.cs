using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//Класс, описывающий клетку игрового поля
public class MapCell
{
    private GameObject cell_object;
    private int cell_identificator;
    private int cell_x;
    private int cell_y;

    public GameObject Cell_Object { get { return cell_object; } }
    public int Cell_Identificator { get { return cell_identificator; } set { cell_identificator = value; } }
    public int Cell_X { get { return cell_x; } }
    public int Cell_Y { get { return cell_y; } }

    public MapCell(GameObject fObj, int x, int y)
    {
        cell_identificator = -1;
        cell_object = fObj;
        cell_x = x;
        cell_y = y;
    }

    public MapCell(GameObject fObj, int fId, int x, int y)
    {
        cell_object = fObj;
        cell_identificator = fId;
        cell_x = x;
        cell_y = y;
    }
}

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
	private Vector2 initial_point; //стартовые координаты для формирования сетки 
	public int grid_width; //ширина сетки (число "квадратиков" сетки по ширине)
	public int grid_height; //высота сетки (число "квадратиков" сетки по высоте)
        
    private MapCell[,] game_field;
    public MapCell[,] Game_Field { get { return game_field; } }

    private bool isInitiallyFormed = false; //для изначального формирования поля
    public bool IsInitiallyFormed { get { return isInitiallyFormed; } }

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
                //устанавливаем позицию и в саму клетку
                (cell as GameObject).GetComponent<Grid_ObjectDetection>().Pos_X = row;
                (cell as GameObject).GetComponent<Grid_ObjectDetection>().Pos_Y = col;
                game_field[row, col] = new MapCell(cell as GameObject, row, col);
                col++;
            }
            row--;
            col = 0;
        }
    }

    /*Функция заполнения сетки 1 и 0*/
    private void FillGrid()
    {
        for (int i = 0; i < game_field.GetLength(0); i++)
        {
            for (int j = 0; j < game_field.GetLength(1); j++)
            {
                game_field[i, j].Cell_Identificator = game_field[i, j].Cell_Object.GetComponent<Grid_ObjectDetection>().Detector;
            }
        }
    }

    private void Awake()
    {
        initial_point = new Vector2(0, 0);//background.transform.position;
        game_field = new MapCell[grid_height, grid_width];
    }

	// Use this for initialization
	private void Start() 
    {
		InitGrid();
	}
    
	// Update is called once per frame
	private void Update() 
    {
        if (!isInitiallyFormed)
        {
            FillGrid();
            isInitiallyFormed = true;
        }
	}
}
