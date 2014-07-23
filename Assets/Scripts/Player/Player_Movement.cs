using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour {
    private Vector2 start;
    public Vector2 end;
    public float speed;
    private bool reached = false;
    public string name;
        
	// Use this for initialization
	void Start () {
        start = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector2.MoveTowards(this.transform.position, end, 0.1f * speed);
        if (this.transform.position.x == end.x && this.transform.position.y == end.y && ! reached)
        {
            Debug.Log(name + " is on position!");
            reached = true;
        }
	}
}
