using UnityEngine;
using System.Collections;
using System.IO;

public class GridScript : MonoBehaviour {
    //=======================ПЕРЕМЕННЫЕ=======================
    public Vector2 mapInitialPoint; //по умолчанию будет браться (0;0)
    public Vector2[,] mapGrid; //карта (состоит из 0 и 1)
    public double gridStep; //шаг сетки 
    public int width; //ширина сетки
    public int height; //высота сетки
    //=======================ПЕРЕМЕННЫЕ=======================

    //=======================ФУНКЦИИ=======================
    //***
    //Формирование сетки
    //***
    private Vector2[,] FormGrid(int fWidth, int fHeight, double fGridStep, Vector2 fInitialPoint)
    {
        Vector2[,] map = new Vector2[fWidth, fHeight];

        for (int i = 0; i < fHeight; i++)
        {
            for (int j = 0; j < fWidth; j++)
            {
                map[i, j] = new Vector2((float)(i * fGridStep + fGridStep/2), (float)(j * fGridStep + fGridStep/2));
            }
        }

        return map;
    }
    //=======================ФУНКЦИИ=======================

    //***
    //Печать карты в файл
    //***
    private void PrintMapToFile(Vector2[,] fMap, string fPath)
    {
        using (StreamWriter writer = new StreamWriter(fPath))
        {
            for (int i = 0; i < fMap.GetLength(0); i++)
            {
                writer.Write("\n");
                for (int j = 0; j < fMap.GetLength(1); j++)
                {
                    writer.Write(fMap[i,j].x + " ; " + fMap[i,j].y + "     ");
                }
            }
        }
    }

    //Пишем генератор карты по препятствиям

	void Start () {
        mapGrid = FormGrid(width, height, gridStep, mapInitialPoint);
        PrintMapToFile(mapGrid, "file.txt");
        Debug.Log("Formed!");
        
	}

	void Update () {
	
	}
}
