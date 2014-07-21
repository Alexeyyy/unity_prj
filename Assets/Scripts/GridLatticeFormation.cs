using UnityEngine;
using System.Collections;

public class GridLatticeFormation : MonoBehaviour {
	public GameObject mapCell;
	public Vector2 initialPoint;
	public int count_width;
	public int count_height;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < count_width; i++) 
		{
			for(int j = 0; j < count_height; j++) {
				Instantiate(mapCell, new Vector3(0.8f * i, 0.8f * j, 0), Quaternion.identity);	
			}
		}
		Debug.Log (mapCell.collider2D.bounds.size.x);

	}



	// Update is called once per frame
	void Update () {
	
	}
}
