using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PathFinder2 : MonoBehaviour
{
    public class Node2
    {
        public MapCell Cell { get; set; }
        public int DistanceFromStart { get; set; }
        public Node2 Parent { get; set; }
        public int HeuristicDistanceToGoal { get; set; }
        public int PredictableDistanceToGoal
        {
            get
            {
                return this.HeuristicDistanceToGoal + this.DistanceFromStart;
            }
        }

        public Node2(MapCell fCell, int fDistanceFromStart, Node2 fParent, int fHeuristicDistance)
        {
            Cell = fCell;
            DistanceFromStart = fDistanceFromStart;
            Parent = fParent;
            HeuristicDistanceToGoal = fHeuristicDistance;
        }
    }

    private int FindHeuristicDistance(MapCell fCellFrom, MapCell fCellTo)
    {
        return Mathf.Abs(fCellFrom.Cell_X - fCellTo.Cell_X) + Mathf.Abs(fCellFrom.Cell_Y - fCellTo.Cell_Y);
    }

    private Node2 FindMinimalHeuristicNode(List<Node2> fOpenedList)
    {
        Node2 node = fOpenedList[0];
        for (int i = 0; i < fOpenedList.Count; i++)
        {
            if (fOpenedList[i].PredictableDistanceToGoal < node.PredictableDistanceToGoal)
            {
                node = fOpenedList[i];
            }
        }
        return node;
    }

    private void AssignNeighbour(List<Node2> fNeighbours, MapCell fCell, Node2 fNode, Node2 fGoalNode)
    {
        if (fCell.Cell_Identificator != 1)
        {
            fNeighbours.Add(new Node2(fCell, fNode.DistanceFromStart + 1, fNode, FindHeuristicDistance(fCell, fGoalNode.Cell)));
        }
    }

    //Получение клеток-соседей текущей точки
    private List<Node2> GetCellNeighbours(Node2 fNode, Node2 fGoalNode)
    {
        List<Node2> neigbours = new List<Node2>();
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
    private bool CheckOnAlreadyWrittenCells(Node2 fNode, List<Node2> fNodesList)
    {
        foreach(Node2 node in fNodesList)
        {
            if (node.Cell == fNode.Cell)
            {
                return true;
            }
        }
        return false;
    }

    private Node2 CheckOnSamePositions(Node2 fNode, List<Node2> fNodesList)
    {
        foreach (Node2 node in fNodesList)
        {
            if (node.Cell.Cell_X == fNode.Cell.Cell_X && node.Cell.Cell_Y == fNode.Cell.Cell_Y)
            {
                return node;
            }
        }
        return null;
    }

    private int[,] CalculatePath(List<Node2> fClosedList)
    {
        List<Node2> path = new List<Node2>();
        Node2 node = fClosedList[fClosedList.Count - 1];

        while(node != null) 
        {
            path.Add(node);
            node = node.Parent;
        }

        path.Reverse();

        int[,] route = new int[path.Count, 2];

        for(int i = 0; i < path.Count; i++)
        {
            route[i, 0] = path[i].Cell.Cell_X;
            route[i, 1] = path[i].Cell.Cell_Y;
        }

        return route;
    }

    public int[,] FindPath(Node2 fStartNode, Node2 fGoalNode)
    {
        List<Node2> openedList = new List<Node2>();
        List<Node2> closedList = new List<Node2>();

        openedList.Add(fStartNode);

        while (openedList.Count > 0)
        {
            Node2 currentNode = FindMinimalHeuristicNode(openedList);

            if (currentNode.Cell == fGoalNode.Cell)
            {
                closedList.Add(currentNode);
                return CalculatePath(closedList);
            }

            openedList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node2> neighbours = GetCellNeighbours(currentNode, fGoalNode);

            foreach (Node2 neighbour in neighbours)
            {
                //если точка уже в закрытом списке, то пропускаем ее
                if(CheckOnAlreadyWrittenCells(neighbour, closedList))
                    continue;
                //проверяем на наличие точки в открытом списке
                Node2 openedNode = CheckOnSamePositions(neighbour, openedList);

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


    public bool h = false;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Grid_Manager.S_Instance.IsInitiallyFormed && !h)
        {
            
            /*Debug.Log(FindHeuristicDistance(Grid_Manager.S_Instance.Game_Field[0, 0], Grid_Manager.S_Instance.Game_Field[49, 49]));
            h = true;
            Debug.Log(GetCellNeighbours(Grid_Manager.S_Instance.Game_Field[0, 0]).Count);
            Debug.Log(CheckOnAlreadyWrittenCells(new Node2(Grid_Manager.S_Instance.Game_Field[0, 0], 100, null, 10), new List<Node2>(){new Node2(Grid_Manager.S_Instance.Game_Field[0, 0], 43,null, 20)}));*/
            DateTime n = DateTime.Now;
            int[,] route = FindPath(new Node2(Grid_Manager.S_Instance.Game_Field[0, 0], 0, null, FindHeuristicDistance(Grid_Manager.S_Instance.Game_Field[0, 0], Grid_Manager.S_Instance.Game_Field[20, 20])), 
                                    new Node2(Grid_Manager.S_Instance.Game_Field[20, 20], 0, null, 0));
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
    }
}
