using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

//Класс, описывающий клетку, по которой кликнули
public static class ClickedField
{
    public static int pos_x = -1; //позиция по x
    public static int pos_y = -1; //позиция по y
    public static bool isClickedCellChanged = false; //изменилась ли кликнутая клетка
}

//Класс, описывающий точку поля
public class Node
{
    public MapCell Cell { get; set; }
    public int DistanceFromStart { get; set; }
    public Node Parent { get; set; }
    public int HeuristicDistanceToGoal { get; set; }
    public int PredictableDistanceToGoal
    {
        get
        {
            return this.HeuristicDistanceToGoal + this.DistanceFromStart;
        }
    }

    public Node(MapCell fCell, int fDistanceFromStart, Node fParent, int fHeuristicDistance)
    {
        Cell = fCell;
        DistanceFromStart = fDistanceFromStart;
        Parent = fParent;
        HeuristicDistanceToGoal = fHeuristicDistance;
    }
}

//Класс, описывающий алгоритм поиска пути
public static class A_Start_PathFinding
{
    //Нахождение расстояния (длины вектора) от текущей точки до конечной
    private static int FindHeuristicDistance(MapCell fCellFrom, MapCell fCellTo)
    {
        return Mathf.Abs(fCellFrom.Cell_X - fCellTo.Cell_X) + Mathf.Abs(fCellFrom.Cell_Y - fCellTo.Cell_Y);
    }

    //Поиск минимума среди Nodes в открытом списке
    private static Node FindMinimalHeuristicNode(List<Node> fOpenedList)
    {
        Node node = fOpenedList[0];
        for (int i = 0; i < fOpenedList.Count; i++)
        {
            if (fOpenedList[i].PredictableDistanceToGoal < node.PredictableDistanceToGoal)
            {
                node = fOpenedList[i];
            }
        }
        return node;
    }

    //Присвоение клетки-соседа
    private static void AssignNeighbour(List<Node> fNeighbours, MapCell fCell, Node fNode, Node fGoalNode)
    {
        if (fCell.Cell_Identificator != 1)
        {
            fNeighbours.Add(new Node(fCell, fNode.DistanceFromStart + 1, fNode, FindHeuristicDistance(fCell, fGoalNode.Cell))); //+1, потому что расстояние между соседними клетками 1
        }
    }

    //Получение клеток-соседей текущей точки
    private static List<Node> GetCellNeighbours(Node fNode, Node fGoalNode)
    {
        List<Node> neigbours = new List<Node>();
        int row = fNode.Cell.Cell_X,
            col = fNode.Cell.Cell_Y;

        //Ищем соседей (для начала сверху, снизу, слева и справа)
        //сверху
        if (row > 0)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row - 1, col], fNode, fGoalNode);
        }
        //справа
        if (col < Grid_Manager.S_Instance.grid_width - 1)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row, col + 1], fNode, fGoalNode);
        }

        //слева
        if (col > 0)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row, col - 1], fNode, fGoalNode);
        }

        //снизу
        if (row < Grid_Manager.S_Instance.grid_height - 1)
        {
            AssignNeighbour(neigbours, Grid_Manager.S_Instance.Game_Field[row + 1, col], fNode, fGoalNode);
        }

        return neigbours;
    }

    //проверка на уже проходимость
    private static bool CheckOnAlreadyWrittenCells(Node fNode, List<Node> fNodesList)
    {
        foreach (Node node in fNodesList)
        {
            if (node.Cell == fNode.Cell)
            {
                return true;
            }
        }
        return false;
    }

    //Получает Node, если он уже содержится в списке
    private static Node CheckOnSamePositions(Node fNode, List<Node> fNodesList)
    {
        foreach (Node node in fNodesList)
        {
            if (node.Cell.Cell_X == fNode.Cell.Cell_X && node.Cell.Cell_Y == fNode.Cell.Cell_Y)
            {
                return node;
            }
        }
        return null;
    }

    //Вычисление пути
    private static int[,] CalculatePath(List<Node> fClosedList)
    {
        List<Node> path = new List<Node>();
        Node node = fClosedList[fClosedList.Count - 1];

        while (node != null)
        {
            path.Add(node);
            node = node.Parent;
        }

        path.Reverse();

        int[,] route = new int[path.Count, 2];

        for (int i = 0; i < path.Count; i++)
        {
            route[i, 0] = path[i].Cell.Cell_X;
            route[i, 1] = path[i].Cell.Cell_Y;
        }

        return route;
    }

    public static int[,] FindPath(MapCell fStart, MapCell fGoal)
    {
        List<Node> openedList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startNode = new Node(Grid_Manager.S_Instance.Game_Field[fStart.Cell_X, fStart.Cell_Y], 0, null, FindHeuristicDistance(Grid_Manager.S_Instance.Game_Field[fStart.Cell_X, fStart.Cell_Y], Grid_Manager.S_Instance.Game_Field[fGoal.Cell_X, fGoal.Cell_Y]));
        Node goalNode = new Node(Grid_Manager.S_Instance.Game_Field[fGoal.Cell_X, fGoal.Cell_Y], 0, null, 0);

        openedList.Add(startNode);

        while (openedList.Count > 0)
        {
            Node currentNode = FindMinimalHeuristicNode(openedList);
            
            if (currentNode.Cell == goalNode.Cell)
            {
                closedList.Add(currentNode);
                return CalculatePath(closedList);
            }

            openedList.Remove(currentNode);
            closedList.Add(currentNode);
            
            List<Node> neighbours = GetCellNeighbours(currentNode, goalNode);

            foreach (Node neighbour in neighbours)
            {
                //если точка уже в закрытом списке, то пропускаем ее
                if (CheckOnAlreadyWrittenCells(neighbour, closedList))
                    continue;
                //проверяем на наличие точки в открытом списке
                Node openedNode = CheckOnSamePositions(neighbour, openedList);

                if (openedNode == null)
                    openedList.Add(neighbour);
                else
                {
                    openedNode.Parent = currentNode;
                    openedNode.DistanceFromStart = neighbour.DistanceFromStart;
                }
            }
        }

        return null;
    }
}

public class PathFinder2 : MonoBehaviour
{
   
    public bool h = false;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Grid_Manager.S_Instance.IsInitiallyFormed && !h)
        {
            DateTime n = DateTime.Now;
            int[,] route = A_Start_PathFinding.FindPath(Grid_Manager.S_Instance.Game_Field[0,0], Grid_Manager.S_Instance.Game_Field[49,49]);
            DateTime m = DateTime.Now;
            TimeSpan a = m-n;
            Debug.Log("Time is " + (a.Milliseconds));

            using (StreamWriter s = new StreamWriter("path.txt"))
            {
                for (int i = 0; i < route.GetLength(0); i++)
                {
                    s.WriteLine(route[i, 0] + "  " + route[i, 1]);
                }
            }
            h = true;
        }
    }*/
}
