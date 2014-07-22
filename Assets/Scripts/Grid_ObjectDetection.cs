using UnityEngine;
using System.Collections;

public class Grid_ObjectDetection : MonoBehaviour {
    public int detector = 0;
    public int Detector { get { return detector; } }

    private void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.tag == "Obstacle")
        {
            //Destroy(this.gameObject);
            detector = 1;
        }
	}
}
