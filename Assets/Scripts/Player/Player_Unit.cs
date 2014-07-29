using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Player_Unit : MonoBehaviour
{
    public float speed;
    public string name;
    public GameObject controller;

    public bool isSelected = false;
    private int[,] path = null;
    private int index_motion = 0;
    private bool isMotionOver = false;

    public bool IsMotionOver
    {
        get { return isMotionOver; }
        set { isMotionOver = true; }
    }

    public int[,] Path
    {
        get { return path; }
        set { path = value; }
    }

    public int Index_Motion
    {
        get { return index_motion; }
    }

    public bool IsSelected
    {
        get { return isSelected; }
    }
    
    public bool clickedChangeForUnit;
    public int clicked_x_pos;
    public int clicked_y_pos;

    //Функция движения по вычисленному А* маршруту (зависит от Update())
    private void MoveAlongPath()
    {
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
            }
        }
    }

    private void OnMouseDown()
    {
        isSelected = true;
    }

    // Use this for initialization
    private void Start()
    {
        index_motion = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (controller.GetComponent<Player_UnitsController>().isPathCalculated && path != null)
        {
            MoveAlongPath();
        }
    }
}
