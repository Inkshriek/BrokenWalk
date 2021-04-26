using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pickup : MonoBehaviour {
    [SerializeField] private float weight = 5;
    [SerializeField] private bool held = false;
    private int holdDirection = 0;
    private LayerMask layerMask;
    private Rigidbody2D rb;
    public float Weight { get { return weight; } set { weight = value; } }
    public bool Held { get { return held; } private set { held = value; } }
    public int HoldDirection { get { return holdDirection; } set { holdDirection = (int)Mathf.Sign(value); }}

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        layerMask = (1 << 0) | LayerMask.GetMask("Environment");
        if (rb.isKinematic != true) Debug.LogError("A Pickup needs to have its rigidbody as kinematic to work properly.");
    }

    private void FixedUpdate() {
        if (Held) return;

        Vector2 position = rb.position;
        
        Vector2 move = new Vector2(0, -Weight) * Time.deltaTime;
        RaycastHit2D collisionCheck = Physics2D.Linecast(position, move + position, layerMask);
        if (collisionCheck.collider != null) position = collisionCheck.point;
        else position += move;

        //Updating the actual position with the temporary one.
        rb.MovePosition(position);
    }

    public void Drop() {
        Held = false;
        Vector2 current = transform.position;
        while (Physics2D.OverlapPoint(current, layerMask) && HoldDirection != 0) {
            current = new Vector2(current.x + 0.1f * -HoldDirection, current.y);
        }

        transform.position = current;
    }

    public void PickUp() {
        Held = true;
    }
}