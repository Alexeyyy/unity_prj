using UnityEngine;
using System.Collections;

public class Grid_LatticeFormation : MonoBehaviour {
	public GameObject grid_cell; //"объект" сетки поля
	public GameObject background; //картинка на заднем плане
	private Vector2 initial_point; //стартовые координаты для формирования сетки 
	public int grid_width; //ширина сетки (число "квадратиков" сетки по ширине)
	public int grid_height; //высота сетки (число "квадратиков" сетки по высоте)

	private int[,] grid; //игровое поле в виде int[,], где 0 - проходимый, 1- непроходимый
	public int[,] Grid { get; set; }

	public void F() {
		for(int i = 0; i < grid_width; i++) 
		{
			for(int j = 0; j < grid_height; j++) {
				Instantiate(grid_cell, new Vector3(0.8f * i, 0.8f * j, 0), Quaternion.identity);	
			}
		}
		Debug.Log (grid_cell.collider2D.bounds.size.x);
	}
	
	void Awake() {
		initial_point = background.transform.position;
	}

	// Use this for initialization
	void Start () {
		F();
	}



	// Update is called once per frame
	void Update () {
	
	}
}
