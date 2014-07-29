using UnityEngine;
using System.Collections;

public class Grid_ObjectDetection : MonoBehaviour {
    private int detector = 0;
    public int Detector { get { return detector; } }
    private int pos_x;
    public int Pos_X { get { return pos_x; } set { pos_x = value; } }
    private int pos_y;
    public int Pos_Y { get { return pos_y; } set { pos_y = value; } }

    /*Смотрим, что находится в данной клетке (препятствие и т.д.)*/
    private void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.tag == "Obstacle")
        {
            detector = 1;
        }
	}

    //Срабатывает при клике на клетку игрового поля
    private void OnMouseDown()
    {
        Debug.Log("Cell is down!");
        ClickedField.pos_x = this.pos_x;
        ClickedField.pos_y = this.pos_y;
        ClickedField.isClickedCellChanged = true;
    }
}
