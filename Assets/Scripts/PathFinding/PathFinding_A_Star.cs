/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

//Класс, описывающий клетку на пути юнита
public class Node333 : IComparable
{
    public float nodeTotalCost; //G-параметр
    public float estimatedCost; //H-параметр
    public Node333 parent;
    public MapCell cell;

    public Node333()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.parent = null;
    }

    public Node333(MapCell fCell)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.parent = null;
        this.cell = fCell;
    }

    public int CompareTo(object fObj)
    {
        Node333 node = (Node333)fObj;
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
    
    public Node333 GetFirst()
    {
        if (this.nodes.Count > 0)
        {
            return (Node333)this.nodes[0];
        }
        return null;
    }

    public void Push(Node333 fNode)
    {
        this.nodes.Add(fNode);
        this.nodes.Sort();
    }

    public void Remove(Node333 fNode)
    {
        this.nodes.Remove(fNode);
        this.nodes.Sort();
    }
}

public class A_Star
{
    public static PriorityQueue closedList, openedList; //закрытый список узлов grid-a, открытый список узлов grid-a

    private static void AssignNeighbour(ArrayList fNeighbours, MapCell fCell)
    {
        if (fCell.Cell_Identificator != 1)
        {
            fNeighbours.Add(fCell);
        }
    }

    //Получение клеток-соседей текущей точки
    public static ArrayList GetCellNeighbours(MapCell fCell)
    {
        ArrayList neigbours = new ArrayList();
        int row = fCell.Cell_X,
            col = fCell.Cell_Y;


        //Ищем соседей (для начала сверху, снизу, слева и справа)
        //сверху
        if (row > 0)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row - 1, col]);
        }
        //справа
        if (col < Grid_Manager.S_Instance.grid_width - 1)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row, col + 1]);
         }

        //слева
        if (col > 0)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row, col - 1]);
        }

        //снизу
        if (row < Grid_Manager.S_Instance.grid_height - 1)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row + 1, col]);
        }

        return neigbours;
    }

    //расстоние до цели от текущей клетки
    public static float GetHeuristicEstimateCost(Node333 fCurrentCell, Node333 fGoalCell)
    {
        Vector3 vectorCost = fCurrentCell.cell.Cell_Object.transform.position - fGoalCell.cell.Cell_Object.transform.position;
        return vectorCost.magnitude;
    }

    //Поиск пути
    public static ArrayList FindPath(Node333 fStart, Node333 fGoal)
    {
        openedList = new PriorityQueue();
        openedList.Push(fStart);
        fStart.nodeTotalCost = 0.0f;
        fStart.estimatedCost = GetHeuristicEstimateCost(fStart, fGoal);

        closedList = new PriorityQueue();
        Node333 node = null;

        while (openedList.Length != 0)
        {
            node = openedList.GetFirst();
            Debug.Log(node.cell.Cell_X + " ::::::::::::: " + node.cell.Cell_Y);
            //Проверяем текущий Node333 не является ли целью
            if (node.cell.Cell_Object.transform.position == fGoal.cell.Cell_Object.transform.position)
            {
                Debug.Log("Calculated1");
                return CalculatePath(node);
            }

            //Заполняем текущий Node333 в закрытый список
            closedList.Push(node);
            //И удаляем его из открытого
            openedList.Remove(node);

            //Создаем список, хранящий всех соседей
            ArrayList neighbours = new ArrayList();
            neighbours = GetCellNeighbours(node.cell);

            for (int i = 0; i < neighbours.Count; i++)
            {
                Node333 neighbourNode = new Node333(neighbours[i] as MapCell);
                Debug.Log("-------------" + node.cell.Cell_X + " ::::::::::::: " + node.cell.Cell_Y);
                if (!closedList.Contains(neighbourNode))
                {
                    float cost = GetHeuristicEstimateCost(neighbourNode, fGoal);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = GetHeuristicEstimateCost(neighbourNode, fGoal);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;

                    if (!openedList.Contains(neighbourNode))
                    {
                        openedList.Push(neighbourNode);
                    }
                }
            }


        }

        if (node.cell.Cell_Object.transform.position != fGoal.cell.Cell_Object.transform.position)
        {
            Debug.Log("Error!");
            return null;
        }

        Debug.Log("Calculated2");
        return CalculatePath(node);
    }

    public static ArrayList CalculatePath(Node333 fNode)
    {
        ArrayList path = new ArrayList();
        while (fNode != null)
        {
            path.Add(fNode);
            fNode = fNode.parent;
        }
        path.Reverse();
        return path;
    }
   
}

public class PathFinding_A_Star : MonoBehaviour {
    private Vector2 start_point;
    private string current_grid_cell_name;
    private Vector2 goal_point;

    private bool iss = false;
    
	// Update is called once per frame
	void Update () {
        /*if (!iss && Grid_Manager.S_Instance.IsInitiallyFormed)
        {
            iss = true;

            ArrayList path = A_Star.FindPath(new Node333(Grid_Manager.S_Instance.Game_Field[0, 0]), new Node333(Grid_Manager.S_Instance.Game_Field[5, 9])); //36
            if (path == null)
                Debug.Log("NULL");
            else
                Debug.Log("Impressive! - " + path.Count);

            using (StreamWriter s = new StreamWriter("path.txt"))
            {
                for (int i = 0; i < path.Count; i++)
                {
                    s.WriteLine((path[i] as Node333).cell.Cell_X + " " + (path[i] as Node333).cell.Cell_Y);
                }
            }
        }*/
        
        /*string str = string.Empty;
        for (int i = 0; i < Grid_Manager.S_Instance.Game_Field.GetLength(0); i++)
        {
            for (int j = 0; j < Grid_Manager.S_Instance.Game_Field.GetLength(1); j++)
            {
                str += Grid_Manager.S_Instance.Game_Field[i, j].Cell_Identificator.ToString();
            }
            str += "\n";
        }
        Debug.Log(str);
	}
}*/
