using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {
    public Vector2 closedPosition;
    public Vector2 openPosition;
    public float animationTime = 0.5f;
    private bool activated = false;
    //private BoxCollider2D col;
    private Vector3 origin;

    private void Awake() {
        //col = GetComponent<BoxCollider2D>();
        origin = transform.position;
    }

    private void FixedUpdate() {
        if (activated) {
            transform.position = Vector3.MoveTowards(transform.position, origin + (Vector3)openPosition, animationTime);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, origin + (Vector3)closedPosition, animationTime);
        }
    }
    
    public void SwitchDoor() {
        activated = !activated;
    }
}
