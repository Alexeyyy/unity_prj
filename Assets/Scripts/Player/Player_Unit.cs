using UnityEngine;
using System.Collections;
using System.IO;

public class Player_Unit : MonoBehaviour {
    
    private Vector2 start;
    public Vector2 end;
    public float speed;
    private bool reached = false;
    public string name;
    public bool isSelected = false;

    //Поиск клетки, где в данный момент находится игрок
    public Node GetCurrentCellLocation()
    {
        Vector3 currentUnitPosition = this.transform.position;
        
        //Проверям, какой клетке принадлежит юнит
        for (int i = 0; i < Grid_Manager.S_Instance.Game_Field.GetLength(0); i++)
        {
            for (int j = 0; j < Grid_Manager.S_Instance.Game_Field.GetLength(1); j++)
            {
                if (currentUnitPosition.x < Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.x + Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.x / 2
                    && currentUnitPosition.x > Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.x - Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.x / 2
                    && currentUnitPosition.y > Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.y - Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.y / 2
                    && currentUnitPosition.y < Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.transform.position.y + Grid_Manager.S_Instance.Game_Field[i, j].Cell_Object.GetComponent<BoxCollider2D>().size.y / 2)
                {
                    return new Node(Grid_Manager.S_Instance.Game_Field[i, j]);
                }
            }
        }

        return null;
    }

    private void OnMouseDown()
    {
        Debug.Log("!");
        Debug.Log("Кликнутая клетка " + ClickedField.pos_x + "   " + ClickedField.pos_y);
        if (ClickedField.isClickedCellChanged && this.isSelected)
        {
            Debug.Log("Here!");
            Node startNode = GetCurrentCellLocation();
            Node goalNode = new Node(Grid_Manager.S_Instance.Game_Field[ClickedField.pos_x, ClickedField.pos_y]);

            //проверяем стоит ли применять A*, не является ли указанное поле препятствием
            if (goalNode.cell.Cell_Identificator != 1)
            {
                ArrayList path = A_Star.FindPath(startNode, goalNode);
                using (StreamWriter s = new StreamWriter("path.txt"))
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        s.WriteLine((path[i] as Node).cell.Cell_X + " " + (path[i] as Node).cell.Cell_Y);
                    }
                }
            }

            //Debug.Log("Кликнутая клетка " + ClickedField.pos_x + "   " + ClickedField.pos_y);
            ClickedField.isClickedCellChanged = false;
        }
    }

	// Use this for initialization
	private void Start () {
        start = this.transform.position;
        isSelected = true;
	}
	
	// Update is called once per frame
	private void Update () {
        //если выбрали юнита и кликнули куда ему идти то
        
	}
}
