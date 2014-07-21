using UnityEngine;
using System.Collections;

public class Grid_ObstacleDetection : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log ("!!!!!!");
		if(collider.tag == "Obstacle") 
		{
			Destroy(this.gameObject);
		}
	}
}
