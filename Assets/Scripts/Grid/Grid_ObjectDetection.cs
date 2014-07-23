using UnityEngine;
using System.Collections;

public class Grid_ObjectDetection : MonoBehaviour {
    private int detector = 0;
    public int Detector { get { return detector; } }

    private int x_pos;
    public int X_Pos { get { return x_pos; } set { x_pos = value; } }
    private int y_pos;
    public int Y_Pos { get { return y_pos; } set { y_pos = value; } }

    /*Смотрим, что находится в данной клетке (препятствие и т.д.)*/
    private void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.tag == "Obstacle")
        {
            //Destroy(this.gameObject);
            detector = 1;
        }
	}

    //Срабатывает при клике на клетку игрового поля
    private void OnMouseDown()
    {
        //Debug.Log(name);
    }
}
