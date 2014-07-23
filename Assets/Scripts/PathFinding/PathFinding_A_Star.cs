using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Класс, описывающий клетку на пути юнита
public class Node : IComparable
{
    public float nodeTotalCost; //G-параметр
    public float estimatedCost; //H-параметр
    public bool isObstacle;
    public Node parent;
    public GameObject cell;

    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.isObstacle = false;
        this.parent = null;
    }

    public Node(GameObject fPos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.isObstacle = false;
        this.parent = null;
        this.cell = fPos;
    }

    public void MarkAsObstacle() 
    {
        this.isObstacle = true;
    }

    public int CompareTo(object fObj)
    {
        Node node = (Node)fObj;
        //-1, значит, что fObj перед текущим
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        //1, значит, что объект fObj стоит после текущего объекта 
        if (this.estimatedCost > node.estimatedCost)
            return 1;
        return 0; //если ноды равнозначны
    }
}

//Класс, описывающий очередь узлов-нодов
public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();
    public int Length { get { return this.nodes.Count; } }
   
    public bool Contains (object fNode)
    {
        return this.nodes.Contains(fNode);
    }
    
    public Node GetFirst()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    public void Push(Node fNode)
    {
        this.nodes.Add(fNode);
        this.nodes.Sort();
    }

    public void Remove(Node fNode)
    {
        this.nodes.Remove(fNode);
        this.nodes.Sort();
    }
}

public class A_Star
{
    public static PriorityQueue closedList, openedList; //закрытый список узлов grid-a, открытый список узлов grid-a


    //Получение клеток-соседей текущей точки
    public static ArrayList GetCellNeighbours(GameObject fCell)
    {
        ArrayList neigbours = new ArrayList();
        int row = fCell.GetComponent<Grid_ObjectDetection>().X_Pos,
            col = fCell.GetComponent<Grid_ObjectDetection>().Y_Pos;

        //Ищем соседей (для начала сверху, снизу, слева и справа)
        //сверху
        if (row > 0)
        {
            neigbours.Add(Grid_Manager.S_Instance.Grid_Cells[row - 1, col]);
        }
        //справа
        if (col < Grid_Manager.S_Instance.grid_width)
        {
            neigbours.Add(Grid_Manager.S_Instance.Grid_Cells[row, col + 1]);
        }
        //снизу
        if (row < Grid_Manager.S_Instance.grid_height)
        {
            neigbours.Add(Grid_Manager.S_Instance.Grid_Cells[row + 1, col]);
        }
        //слева
        if (col > 0)
        {
            neigbours.Add(Grid_Manager.S_Instance.Grid_Cells[row, col - 1]);
        }

        return neigbours;
    }

    //расстоние до цели от текущей клетки
    public static float GetHeuristicEstimateCost(Node fCurrentCell, Node fGoalCell)
    {
        Vector3 vectorCost = fCurrentCell.cell.transform.position - fGoalCell.cell.transform.position;
        return vectorCost.magnitude;
    }

    //Поиск пути
    public static ArrayList FindPath(Node fStart, Node fGoal)
    {
        openedList = new PriorityQueue();
        openedList.Push(fStart);
        fStart.nodeTotalCost = 0.0f;
        fStart.estimatedCost = GetHeuristicEstimateCost(fStart, fGoal);

        closedList = new PriorityQueue();
        Node node = null;

        while (openedList.Length != 0)
        {
            node = openedList.GetFirst();
            //Проверяем текущий Node не является ли целью
            if (node.cell == fGoal.cell)
                return null; //...

            //Создаем список, хранящий всех соседей
            ArrayList neighbours = new ArrayList();
            neighbours = GetCellNeighbours(node.cell);

        }

        return null;
    }

   
}

public class PathFinding_A_Star : MonoBehaviour {
    
    
    private Vector2 start_point;
    private string current_grid_cell_name;
    private Vector2 goal_point;

    private bool iss = false;

    private void OnTriggerStay2D(Collider2D fCollider)
    {
        /*Получаем клетку, где в данный момент стоит игрок*/
        if (fCollider.tag == "gridCell" && this.transform.position.x < fCollider.transform.position.x + fCollider.GetComponent<BoxCollider2D>().size.x/2
                                        && this.transform.position.x > fCollider.transform.position.x - fCollider.GetComponent<BoxCollider2D>().size.x/2
                                        && this.transform.position.y > fCollider.transform.position.y - fCollider.GetComponent<BoxCollider2D>().size.y/2
                                        && this.transform.position.y < fCollider.transform.position.y + fCollider.GetComponent<BoxCollider2D>().size.y/2
            )
        {
            /*current_grid_cell_name = fCollider.GetComponent<Grid_ObjectDetection>().Name;
            Debug.Log("!" + current_grid_cell_name);*/
        }
    }

    private void GetStartCell()
    {
       
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!iss)
        {
            ArrayList objcts = A_Star.GetCellNeighbours(Grid_Manager.S_Instance.Grid_Cells[1, 1]);
            foreach (GameObject obj in objcts)
            {
                Debug.Log(obj.GetComponent<Grid_ObjectDetection>().X_Pos + " " + obj.GetComponent<Grid_ObjectDetection>().Y_Pos);
            }
            iss = true;
        }
	}
}
