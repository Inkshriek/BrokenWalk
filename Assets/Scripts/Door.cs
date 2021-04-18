using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {
    public Vector2 closedPosition;
    public Vector2 openPosition;
    public float animationSpeed = 0.5f;
    private bool activated = false;
    private BoxCollider2D col;
    private Vector2 originPosition;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();
        originPosition = transform.position;
    }

    private void FixedUpdate() {
        if (activated) {
            transform.position = Vector3.MoveTowards(transform.position, originPosition + openPosition, animationSpeed);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, originPosition + closedPosition, animationSpeed);
        }
    }
    
    public void SwitchDoor() {
        activated = !activated;
    }
}
